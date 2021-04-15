using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public static class DetailsRowUtils
    {
        // https://stackoverflow.com/questions/12420466/unable-to-cast-object-of-type-system-linq-expressions-unaryexpression-to-type/12420603#12420603
        public static PropertyInfo? GetPropertyInfo<T>(Expression<Func<T, Object>> expression)
        {
            if (expression.Body is MemberExpression expression1)
            {
                var memberAcessExpression =  expression1.Member;
                var propertyInfo = memberAcessExpression as PropertyInfo;
                return propertyInfo;
            }
            else
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                return ((MemberExpression)op).Member as PropertyInfo;
            }
        }

        // https://stackoverflow.com/questions/7723744/expressionfunctmodel-string-to-expressionactiontmodel-getter-to-sette
        public static Action<TObject, TPropertyOnObject> GetSetter<TObject, TPropertyOnObject>(Expression<Func<TObject, TPropertyOnObject>> getterExpression)
        {
            /*** SIMPLE PROPERTIES AND FIELDS ***/
            // check if the getter expression refers directly to a PROPERTY or FIELD
            if (getterExpression.Body is not MemberExpression memberAcessExpression)
            {
                var op = ((UnaryExpression)getterExpression.Body).Operand;
                memberAcessExpression = (MemberExpression)op;
            }


            if (memberAcessExpression != null)
            {
                //to here we assign the SetValue method of a property or field
                Action<object, object> propertyOrFieldSetValue = null;

                // property
                var propertyInfo = memberAcessExpression.Member as PropertyInfo;

                if (propertyInfo != null)
                {
                    propertyOrFieldSetValue = (declaringObjectInstance, propertyOrFieldValue) => propertyInfo.SetValue(declaringObjectInstance, propertyOrFieldValue);
                };

                // field
                var fieldInfo = memberAcessExpression.Member as FieldInfo;

                if (fieldInfo != null)
                {
                    propertyOrFieldSetValue = (declaringObjectInstance, propertyOrFieldValue) => fieldInfo.SetValue(declaringObjectInstance, propertyOrFieldValue);
                }

                // This is the expression to get declaring object instance.
                // Example: for expression "o=>o.Property1.Property2.CollectionProperty[3].TargetProperty" it gives us the "o.Property1.Property2.CollectionProperty[3]" part
                var memberAcessExpressionCompiledLambda = Expression.Lambda(memberAcessExpression.Expression, getterExpression.Parameters.Single()).Compile();
                void setter(TObject expressionParameter, TPropertyOnObject value)
                {
                    // get the object instance on which is the property we want to set
                    var declaringObjectInstance = memberAcessExpressionCompiledLambda.DynamicInvoke(expressionParameter);
                    Debug.Assert(propertyOrFieldSetValue != null, "propertyOrFieldSetValue != null");
                    // set the value of the property
                    propertyOrFieldSetValue(declaringObjectInstance, value);
                }

                return setter;
            }

            /*** COLLECTIONS ( IDictionary<,> and IList<,>) ***/
            /*
             * DICTIONARY:
             * Sample expression: 
             *      "myObj => myObj.Property1.ListProperty[5].AdditionalInfo["KEY"]"
             * Setter behaviour:
             *      The same as adding to a dictionary. 
             *      It does Add("KEY", <value to be set>) to the dictionary. It fails if the jey already exists.
             *      
             * 
             * LIST
             * Sample expression: 
             *      "myObj => myObj.Property1.ListProperty[INDEX]"
             * Setter behaviour:
             *      If INDEX >= 0 and the index exists in the collection it behaves the same like inserting to a collection.
             *      IF INDEX <  0 (is negative) it adds the value at the end of the collection.
             */
            if (
                getterExpression.Body is MethodCallExpression methodCallExpression && methodCallExpression.Object != null &&
                methodCallExpression.Object.Type.IsGenericType)
            {
                var collectionGetterExpression = methodCallExpression.Object as MemberExpression;
                Debug.Assert(collectionGetterExpression != null, "collectionGetterExpression != null");

                // This gives us the collection instance when it is invoked on the object instance whic the expression is for
                var collectionGetterCompiledLambda = Expression.Lambda(collectionGetterExpression, getterExpression.Parameters.Single()).Compile();

                // this returns the "KEY" which is the key (object) in case of Dictionaries and Index (integer) in case of other collections
                var collectionKey = ((ConstantExpression)methodCallExpression.Arguments[0]).Value;
                var collectionType = collectionGetterExpression.Type;

                // IDICTIONARY
                if (collectionType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    // Create an action which accepts the instance of the object which the "dictionary getter" expression is for and a value
                    // to be added to the dictionary.
                    void dictionaryAdder(TObject expressionParameter, TPropertyOnObject value)
                    {
                        try
                        {
                            var dictionaryInstance = (IDictionary)collectionGetterCompiledLambda.DynamicInvoke(expressionParameter);
                            dictionaryInstance.Add(collectionKey, value);
                        }
                        catch (Exception exception)
                        {
                            throw new Exception(
                                string.Format(
                                    "Addition to dictionary failed [Key='{0}', Value='{1}']. The \"adder\" was generated from getter expression: '{2}'.",
                                    collectionKey,
                                    value,
                                    getterExpression.ToString()), exception);
                        }
                    }

                    return dictionaryAdder;
                }

                // ILIST
                if (typeof(IList<>).MakeGenericType(typeof(bool)).IsAssignableFrom(collectionType.GetGenericTypeDefinition().MakeGenericType(typeof(bool))))
                {
                    // Create an action which accepts the instance of the object which the "collection getter" expression is for and a value
                    // to be inserted
                    void collectionInserter(TObject expressionParameter, TPropertyOnObject value)
                    {
                        try
                        {
                            var collectionInstance = (IList<TPropertyOnObject>)collectionGetterCompiledLambda.DynamicInvoke(expressionParameter);
                            var collectionIndexFromExpression = int.Parse(collectionKey.ToString());

                            // The semantics of a collection setter is to add value if the index in expression is <0 and set the item at the index
                            // if the index >=0.
                            if (collectionIndexFromExpression < 0)
                            {
                                collectionInstance.Add(value);
                            }
                            else
                            {
                                collectionInstance[collectionIndexFromExpression] = value;
                            }
                        }
                        catch (Exception invocationException)
                        {
                            throw new Exception(
                                string.Format(
                                    "Insertion to collection failed [Index='{0}', Value='{1}']. The \"inserter\" was generated from getter expression: '{2}'.",
                                    collectionKey,
                                    value,
                                    getterExpression.ToString()), invocationException);
                        }
                    }

                    return collectionInserter;
                }

                throw new NotSupportedException(
                    string.Format(
                        "Cannot generate setter from the given expression: '{0}'. Collection type: '{1}' not supported.",
                        getterExpression, collectionType));
            }

            throw new NotSupportedException("Cannot generate setter from the given expression: " + getterExpression);
        }
    }
}
