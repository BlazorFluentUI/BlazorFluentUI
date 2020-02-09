using DynamicData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace BlazorFabric
{
    public class HeaderItem<TItem> : GroupedListItem<TItem>
    {
        public bool IsOpen { get => isOpenSubject.Value; 
            set 
            {
                isOpenSubject.OnNext(value); 
            }
        }

        private BehaviorSubject<bool> isOpenSubject;
        public IObservable<bool> IsOpenObservable => isOpenSubject.AsObservable();
               

        public HeaderItem(TItem item, HeaderItem<TItem> parent, int index, int depth, Func<TItem,string> groupTitleSelector, SourceCache<GroupedListItem<TItem>, string> selectionObservable)
            : base(item, parent, index, depth, selectionObservable)
        {
            isOpenSubject = new BehaviorSubject<bool>(true);
            Name = groupTitleSelector(item);

        }
    }

    public class PlainItem<TItem> : GroupedListItem<TItem>
    {
        public PlainItem(TItem item, HeaderItem<TItem> parent, int index, int depth, SourceCache<GroupedListItem<TItem>, string> selectionObservable) 
            : base(item, parent, index, depth, selectionObservable)
        {
            
        }
    }

    public class GroupedListItem<TItem>
    {
        private BehaviorSubject<bool> _isVisibleSubject;
        public IObservable<bool> IsVisibleObservable => _isVisibleSubject.AsObservable();
        public bool IsVisible
        {
            get => _isVisibleSubject.Value;
            set
            {
                _isVisibleSubject.OnNext(value);
            }
        }

        private bool _isSelected;
        public bool IsSelected { get => _isSelected; set => _isSelected = value; }

        //private bool _isSelected;
        //public bool IsSelected { get => _isSelected; set { if (_isSelected != value) { _isSelected = value; isSelectedSubject?.OnNext(value); } } }
        //private Subject<bool> isSelectedSubject;
        //public IObservable<bool> IsSelectedObservable => isSelectedSubject.AsObservable();

        public TItem Item { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int Depth { get; set; }
        public string Key => GetGroupItemKey(this);

        
        private static string GetGroupItemKey(GroupedListItem<TItem> groupedListItem)
        {
            string key = "";
            if (groupedListItem.Parent != null)
                key = GetGroupItemKey(groupedListItem.Parent) + "-";
            key += groupedListItem.Index;
            return key;
        }

        public HeaderItem<TItem> Parent { get; set; }

        public GroupedListItem(TItem item, HeaderItem<TItem> parent, int index, int depth, SourceCache<GroupedListItem<TItem>, string> selectionCache)
        {
            _isVisibleSubject = new BehaviorSubject<bool>(true);
            //isSelectedSubject = new Subject<bool>();

            Item = item;
            Index = index;
            Depth = depth;
            Parent = parent;

            Parent?.IsOpenObservable.CombineLatest(Parent.IsVisibleObservable, (open, visible) => !visible ? false : (open ? true : false)).Subscribe(shouldBeVisible =>
            {
                Debug.WriteLine($"Setting item {index} to be visible: {shouldBeVisible}");
                IsVisible = shouldBeVisible;
            });

            //if (Parent != null)
            //{
            //    selectionCache.Connect()
            //        .Filter(x => x == Parent)
            //        .OnItemAdded(x =>
            //        {
            //            selectionCache.AddOrUpdate(this);
            //        })
            //        .OnItemRemoved(x =>
            //        {
            //            selectionCache.Remove(this);
            //        })
            //        .Subscribe();
            //}

        }

        //public bool ShouldAlsoToggle(GroupedListItem<TItem> selectedItem)
        //{
        //    if (this.Parent == selectedItem)
        //        return true;
        //    else if (this.Parent != null)
        //    {
        //        return this.Parent.ShouldAlsoToggle(selectedItem);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

    }

    public class GroupedListItemComparer<TItem> : IComparer<GroupedListItem<TItem>>
    {
        static int compareCount = 0;
        public int Compare(GroupedListItem<TItem> x, GroupedListItem<TItem> y)
        {
            compareCount++;

            if (x.Depth == y.Depth) 
            {
                return CompareItems(x, y);
            }
            else
            {
                //not same depth, rewind deeper one until we can compare parents
                if (x.Depth < y.Depth)
                {
                    GroupedListItem<TItem> yparent = y;
                    while (yparent.Depth != x.Depth && yparent.Parent != null)
                    {
                        yparent = yparent.Parent;
                    }
                    var comparison = CompareItems(x, yparent);
                    if (comparison == 0)
                    {
                        return -1;
                    }
                    return comparison;
                }
                else
                {
                    GroupedListItem<TItem> xparent = x;
                    while (xparent.Depth != y.Depth && xparent.Parent != null)
                    {
                        xparent = xparent.Parent;
                    }
                    var comparison = CompareItems(xparent, y);
                    if (comparison == 0)
                    {
                        return 1;
                    }
                    return comparison;
                }

            }
                        
        }

        private int CompareItems(GroupedListItem<TItem> x, GroupedListItem<TItem> y)
        {
            if (x.Parent == y.Parent)
            {
                return x.Index.CompareTo(y.Index);
            }
            else
            {
                var parentCompare = CompareItems(x.Parent, y.Parent);
                if (parentCompare == 0)
                {
                    return x.Index.CompareTo(y.Index);
                }
                else
                {
                    return parentCompare;
                }
            }
        }
    }
}
