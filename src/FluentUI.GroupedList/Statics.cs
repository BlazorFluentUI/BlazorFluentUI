using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;

namespace FluentUI
{
    public static class Statics
    {
        //public static IObservable<IChangeSet<GroupedListItem2<TItem>>> BufferForTime<TItem>(this IObservable<IChangeSet<GroupedListItem2<TItem>>> items)
        //{
        //    return items
        //        .Buffer(() => Observable.Interval(TimeSpan.FromMilliseconds(400),System.Reactive.Concurrency.ThreadPoolScheduler.Instance).Do(x=> { Debug.WriteLine($"Tick {x}"); }))
        //        //.Buffer(() => items.Where(x=>x is IChangeSet<PlainItem2<TItem>>))
        //        .Select(list =>
        //        {
                
        //         var count = list.SelectMany(x => x).Count();
        //         var cache = new ChangeAwareList<GroupedListItem2<TItem>>(count);
        //         //var mainSet = new ChangeSet<GroupedListItem2<TItem>>();
        //         foreach (var set in list)
        //         {
        //             foreach (var change in set)
        //             {
        //                 switch (change.Reason)
        //                 {
        //                     case ListChangeReason.Add:
        //                         cache.Add(change.Item.Current);
        //                         break;
        //                     case ListChangeReason.AddRange:
        //                         cache.AddRange(change.Range);
        //                         break;
        //                     case ListChangeReason.Remove:
        //                         cache.Remove(change.Item.Current);
        //                         break;
        //                     case ListChangeReason.RemoveRange:
        //                         cache.Remove(change.Range);
        //                         break;
        //                        case ListChangeReason.Replace:
        //                            if (change.Item.Previous.HasValue) 
        //                                cache.Replace(change.Item.Previous.Value, change.Item.Current);

        //                        break;
        //                     case ListChangeReason.Refresh:
        //                         cache.Refresh(change.Item.Current);
        //                         break;
        //                     case ListChangeReason.Moved:
        //                         cache.Move(change.Item.PreviousIndex, change.Item.CurrentIndex);
        //                         break;
        //                 }
        //             }
        //         }
        //         return cache.CaptureChanges();
        //     });
        //}

        //public static IObservable<IChangeSet<GroupedListItem2<TItem>,object>> FlatGroup<TItem>(this IObservable<IChangeSet<TItem, object>> items, IList<Func<TItem, object>> groupBy, int depth, IList<object> groupKeys, IObservable<IComparer<TItem>> itemSortExpression)
        //{

        //    if (groupBy != null && groupBy.Count > 0)
        //    {


        //        var firstGroupBy = groupBy.First();
        //        var groups = items.Group(firstGroupBy);
        //        var headerItems = groups.Transform(group =>
        //        {
        //            var tempGroupKeyList = groupKeys.ToList();
        //            tempGroupKeyList.Add(group.Key);
        //            //Debug.WriteLine($"Created Header {group.Key}  with parent: {groupKeys.Last()}");
        //            return new HeaderItem2<TItem>(default, tempGroupKeyList, depth, group.Key.ToString()) as GroupedListItem2<TItem>;
        //        });
        //        var subItems = groups.MergeMany(group =>
        //        {
        //            var tempGroupKeyList = groupKeys.ToList();
        //            tempGroupKeyList.Add(group.Key);
        //            var changeset = group.Cache.Connect().FlatGroup<TItem>(groupBy.Skip(1).ToList(), depth + 1, tempGroupKeyList, itemSortExpression);
        //            return changeset;
        //        });

        //        var flattenedItems = headerItems.Or(subItems);
        //        return flattenedItems;
        //    }
        //    else
        //    {
        //        return items.AutoRefreshOnObservable(x => itemSortExpression)

        //                .Sort(itemSortExpression, resetThreshold: 1)  // sort won't happen if only a few items change... so we need to make the threshold just 1 item. 

        //                .Transform((item, index) =>
        //                {
        //                    // Debug.WriteLine($"Group {string.Join(',', groupKeys.Select(x => x.ToString()).ToArray())} Index {index}: {System.Text.Json.JsonSerializer.Serialize<TItem>(item)}");
        //                    return new PlainItem2<TItem>(item, groupKeys, depth, index) as GroupedListItem2<TItem>;
        //                }, true);
        //    }

        //}

        //public static IObservable<IChangeSet<GroupedListItem2<TItem>>> FlatGroup<TItem>(this IObservable<IChangeSet<TItem>> items, IList<Func<TItem, object>> groupBy, int depth, IList<object> groupKeys, IObservable<IComparer<TItem>> itemSortExpression)
        //{

        //    if (groupBy != null && groupBy.Count > 0)
        //    {


        //        var firstGroupBy = groupBy.First();
        //        var groups = items.GroupOn(firstGroupBy);
        //        var headerItems = groups.Transform(group =>
        //        {
        //            var tempGroupKeyList = groupKeys.ToList();
        //            tempGroupKeyList.Add(group.GroupKey);
        //            //Debug.WriteLine($"Created Header {group.Key}  with parent: {groupKeys.Last()}");
        //            return new HeaderItem2<TItem>(default, tempGroupKeyList, depth, group.GroupKey.ToString()) as GroupedListItem2<TItem>;
        //        });
        //        var subItems = groups.MergeMany(group =>
        //        {
        //            var tempGroupKeyList = groupKeys.ToList();
        //            tempGroupKeyList.Add(group.GroupKey);
        //            var changeset = group.List.Connect().FlatGroup<TItem>(groupBy.Skip(1).ToList(), depth + 1, tempGroupKeyList, itemSortExpression);
        //            return changeset;
        //        });

        //        var changeAwareList = new ChangeAwareList<GroupedListItem2<TItem>>();

        //        var flattenedItems = headerItems.Or(subItems);
        //        return flattenedItems;
        //    }
        //    else
        //    {
        //        //var sourceCache = new SourceCache<GroupedListItem2<TItem>, object>(x => x.Item);
        //        //sourceCache.Connect()
        //        //    .Sort(SortExpressionComparer<GroupedListItem2<TItem>>.Ascending(x => x.Depth))
        //        //    .Transform()
                    
        //        return items.AutoRefreshOnObservable(x => itemSortExpression)
        //                .Sort(itemSortExpression, resetThreshold: 1)  // sort won't happen if only a few items change... so we need to make the threshold just 1 item.  
        //                .Transform((item, index) =>
        //                {
        //                    return new PlainItem2<TItem>(item, groupKeys, depth, index) as GroupedListItem2<TItem>;
        //                }, true);
        //    }

        //}



        //public static IObservable<IChangeSet<GroupedListItem2<TItem>>> FlatGroupOld<TItem>(this IObservable<IChangeSet<TItem>> items, IList<Func<TItem, object>> groupBy, int depth, IList<object> groupKeys, IObservable<IComparer<TItem>> itemSortExpression)
        //{

        //    if (groupBy != null && groupBy.Count > 0)
        //    {
        //        var firstGroupBy = groupBy.First();
        //        var groups = items.GroupOn(firstGroupBy);
        //        var headerItems = groups.Transform(group =>
        //        {
        //            var tempGroupKeyList = groupKeys.ToList();
        //            tempGroupKeyList.Add(group.GroupKey);
        //                //Debug.WriteLine($"Created Header {group.Key}  with parent: {groupKeys.Last()}");
        //                return new HeaderItem2<TItem>(default, tempGroupKeyList, depth, group.GroupKey.ToString()) as GroupedListItem2<TItem>;
        //        });
        //        var subItems = groups.MergeMany(group =>
        //        {
        //            var tempGroupKeyList = groupKeys.ToList();
        //            tempGroupKeyList.Add(group.GroupKey);
        //            var changeset = group.List.Connect().FlatGroup<TItem>(groupBy.Skip(1).ToList(), depth + 1, tempGroupKeyList, itemSortExpression);
        //            return changeset;
        //        });

        //        var flattenedItems = headerItems.Or(subItems);
        //        return flattenedItems;
        //    }
        //    else
        //    {
        //        return items.AutoRefreshOnObservable(x => itemSortExpression)
        //                .Sort(itemSortExpression)
        //                .Transform((item, index) =>
        //                {
        //                    Debug.WriteLine($"Group {string.Join(',', groupKeys.Select(x => x.ToString()).ToArray())} Index {index}: {System.Text.Json.JsonSerializer.Serialize<TItem>(item)}");
        //                    return new PlainItem2<TItem>(item, groupKeys, depth, index) as GroupedListItem2<TItem>;
        //                }, true);
        //    }

        //}

        public static IObservable<IChangeSet<TDestination, TKey>> TransformWithInlineUpdate<TObject, TKey, TDestination>(this IObservable<IChangeSet<TObject, TKey>> source,
            Func<TObject, TDestination> transformFactory,
            Action<TDestination, TObject> updateAction = null)
        {
            return source.Scan((ChangeAwareCache<TDestination, TKey>)null, (cache, changes) =>
            {
                if (cache == null)
                    cache = new ChangeAwareCache<TDestination, TKey>(changes.Count);

                foreach (var change in changes)
                {
                    switch (change.Reason)
                    {
                            case ChangeReason.Add:
                                cache.AddOrUpdate(transformFactory(change.Current), change.Key);
                                break;
                            case ChangeReason.Update:
                                {
                                    if (updateAction == null) continue;

                                    var previous = cache.Lookup(change.Key)
                                        .ValueOrThrow(() => new MissingKeyException($"{change.Key} is not found."));

                                    updateAction(previous, change.Current);

                                    //send a refresh as this will force downstream operators 
                                    cache.Refresh(change.Key);
                                }
                                break;
                            case ChangeReason.Remove:
                                cache.Remove(change.Key);
                                break;
                            case ChangeReason.Refresh:
                                cache.Refresh(change.Key);
                                break;
                            case ChangeReason.Moved:
                                //Do nothing !
                                break;
                    }
                }
                return cache;
            }).Select(cache => cache.CaptureChanges());
        }


        public static IObservable<IChangeSet<TItem, object>> Filter<TItem>(this IObservable<IChangeSet<TItem, object>> source, IEnumerable<IObservable<Func<TItem, bool>>> filterPredicates)
        {
            foreach (var filter in filterPredicates)
            {
                source = source.Filter(filter);
            }
            return source;
        }

        public static IObservable<IChangeSet<TItem, object>> Filter<TItem>(this IObservable<IChangeSet<TItem, object>> source, IEnumerable<Func<TItem, bool>> filterPredicates)
        {
            foreach (var filter in filterPredicates)
            {
                source = source.Filter(filter);
            }
            return source;
        }

        public static Func<TItem, IComparable> ConvertToIComparable<TItem>(this Func<TItem, object> func)
        {
            Func<TItem, IComparable> fa = (Func<TItem, IComparable>)Statics.Convert(func, typeof(TItem), typeof(IComparable));
            return fa;
        }

        // https://stackoverflow.com/questions/16590685/using-expression-to-cast-funcobject-object-to-funct-tret
        public static Delegate Convert<TItem>(Func<TItem, object> func, Type argType, Type resultType)
        {
            // If we need more versions of func then consider using params Type as we can abstract some of the
            // conversion then.

            Contract.Requires(func != null);
            Contract.Requires(resultType != null);

            var param = Expression.Parameter(argType);
            var convertedParam = new Expression[] { Expression.Convert(param, typeof(TItem)) };

            // This is gnarly... If a func contains a closure, then even though its static, its first
            // param is used to carry the closure, so its as if it is not a static method, so we need
            // to check for that param and call the func with it if it has one...
            Expression call;
            call = Expression.Convert(
                func.Target == null
                ? Expression.Call(func.Method, convertedParam)
                : Expression.Call(Expression.Constant(func.Target), func.Method, convertedParam), resultType);

            var delegateType = typeof(Func<,>).MakeGenericType(argType, resultType);
            return Expression.Lambda(delegateType, call, param).Compile();
        }




    }
}
