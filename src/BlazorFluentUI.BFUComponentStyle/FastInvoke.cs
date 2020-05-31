using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BlazorFluentUI
{
    // https://stackoverflow.com/questions/17660097/is-it-possible-to-speed-this-method-up/17669142#17669142
    public static class FastInvoke
    {
        public static Func<T, object> BuildUntypedGetter<T>(PropertyInfo propertyInfo)
        {
            var targetType = propertyInfo.DeclaringType;
            var methodInfo = propertyInfo.GetGetMethod();
            var exInstance = Expression.Parameter(targetType, "t");

            //var exMemberAccess = Expression.MakeMemberAccess(exInstance, memberInfo);       // t.PropertyName
            var exBody = Expression.Call(exInstance, methodInfo);
            var exConvertToObject = Expression.Convert(exBody, typeof(object));     // Convert(t.PropertyName, typeof(object))
            var lambda = Expression.Lambda<Func<T, object>>(exConvertToObject, exInstance);

            var action = lambda.Compile();
            return action;

        }
    }
}
