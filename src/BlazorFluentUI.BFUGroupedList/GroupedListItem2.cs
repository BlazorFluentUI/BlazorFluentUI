using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;


namespace BlazorFluentUI
{
    public class GroupedListItem2<TItem> : IComparable
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

        public TItem Item { get; set; }
        public string Name { get; set; }
        //public int Index { get; set; }
        public int Depth { get; set; }
        //public string Key => GetGroupItemKey(this);
        public System.Collections.Generic.List<GroupedListItem<TItem>> Children { get; set; } = new System.Collections.Generic.List<GroupedListItem<TItem>>();

        public int RecursiveCount => Children.RecursiveCount();


        public IList<object> ParentGroupKeys { get; set; }

        public GroupedListItem2(TItem item, IList<object> parentGroupKeys, int depth)
        {
            _isVisibleSubject = new BehaviorSubject<bool>(true);

            Item = item;
            //Index = index;
            Depth = depth;
            ParentGroupKeys = parentGroupKeys;

        }

        public int CompareTo(object obj)
        {
            if (obj is GroupedListItem2<TItem>)
            {
                var b = (GroupedListItem2<TItem>)obj;

                if (this.ParentGroupKeys.Count > b.ParentGroupKeys.Count)
                {
                    var result = this.ParentGroupKeys[b.ParentGroupKeys.Count - 1].ToString().CompareTo(b.ParentGroupKeys[b.ParentGroupKeys.Count - 1].ToString());
                    if (result == 0)
                        return 1;
                    else
                        return result;
                }
                else if (this.ParentGroupKeys.Count < b.ParentGroupKeys.Count)
                {
                    var result = this.ParentGroupKeys[this.ParentGroupKeys.Count - 1].ToString().CompareTo(b.ParentGroupKeys[this.ParentGroupKeys.Count - 1].ToString());
                    if (result == 0)
                        return -1;
                    else
                        return result;
                }
                else
                {
                    //compare each key starting from first
                    for (var i = 0; i < this.ParentGroupKeys.Count; i++)
                    {
                        var result = this.ParentGroupKeys[i].ToString().CompareTo(b.ParentGroupKeys[i].ToString());
                        if (result != 0)
                        {
                            return result;
                        }
                    }
                    // if here, then groups all matched.
                    if (this is HeaderItem2<TItem> && b is PlainItem2<TItem>)
                        return -1;  //header comes before the items
                    else if (this is PlainItem2<TItem> && b is HeaderItem2<TItem>)
                        return 1; //items go after the header
                    else if (this is PlainItem2<TItem> && b is PlainItem2<TItem>)
                    {
                        var c = (PlainItem2<TItem>)this;
                        var d = (PlainItem2<TItem>)b;

                        return (c.Index.CompareTo(d.Index));
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
                return 0; //not groupedItem
        }
    }

    public class HeaderItem2<TItem> : GroupedListItem2<TItem>
    {
        public bool IsOpen
        {
            get => isOpenSubject.Value;
            set
            {
                isOpenSubject.OnNext(value);
            }
        }

        private BehaviorSubject<bool> isOpenSubject;
        public IObservable<bool> IsOpenObservable => isOpenSubject.AsObservable();


        public HeaderItem2(TItem item, IList<object> parentGroupKeys, int depth, string name)
            : base(item, parentGroupKeys, depth)
        {
            isOpenSubject = new BehaviorSubject<bool>(true);
            Name = name;
        }

    }

    public class PlainItem2<TItem> : GroupedListItem2<TItem>
    {

        public int Index { get; set; }

        public PlainItem2(TItem item, IList<object> parentGroupKeys, int depth, int index)
            : base(item, parentGroupKeys, depth)
        {
            Index = index;

        }
    }
}