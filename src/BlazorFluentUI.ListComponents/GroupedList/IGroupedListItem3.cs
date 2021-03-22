namespace BlazorFluentUI.Lists
{
    public interface IGroupedListItem3<TItem> 
    {
        bool IsVisible { get;}
        string Name { get; }
        int Count { get; }
        int Depth { get; }
        TItem Item { get; }
        //ICollection<IGroupedListItem3<TItem>> Items { get; }
    }

    //public class GroupedListItem3<TItem>
    //{
    //    private BehaviorSubject<bool> _isVisibleSubject;
    //    public IObservable<bool> IsVisibleObservable => _isVisibleSubject.AsObservable();
    //    public bool IsVisible
    //    {
    //        get => _isVisibleSubject.Value;
    //        set
    //        {
    //            _isVisibleSubject.OnNext(value);
    //        }
    //    }

    //    private bool _isSelected;
    //    public bool IsSelected { get => _isSelected; set => _isSelected = value; }

    //    public TItem Item { get; set; }
    //    public string Name { get; set; }
    //    public int Index { get; set; }
    //    public int Depth { get; set; }
    //    //public string Key => GetGroupItemKey(this);
    //    public System.Collections.Generic.List<GroupedListItem<TItem>> Children { get; set; } = new System.Collections.Generic.List<GroupedListItem<TItem>>();

    //    public int RecursiveCount => Children.RecursiveCount();
        
        

    //    private static string GetGroupItemKey(GroupedListItem<TItem> groupedListItem)
    //    {
    //        string key = "";
    //        if (groupedListItem.Parent != null)
    //            key = GetGroupItemKey(groupedListItem.Parent) + "-";
    //        key += groupedListItem.Index;
    //        return key;
    //    }

    //    public HeaderItem3<TItem,TKey> Parent { get; set; }

    //    public GroupedListItem3(TItem item, HeaderItem<TItem> parent, int index, int depth)
    //    {
    //        _isVisibleSubject = new BehaviorSubject<bool>(true);

    //        Item = item;
    //        Index = index;
    //        Depth = depth;
    //        Parent = parent;

    //        Parent?.IsOpenObservable.CombineLatest(Parent.IsVisibleObservable, (open, visible) => !visible ? false : (open ? true : false)).Subscribe(shouldBeVisible =>
    //        {
    //            IsVisible = shouldBeVisible;
    //        });
    //    }
    //}

    //public class GroupedListItemComparer<TItem> : IComparer<GroupedListItem<TItem>>
    //{
    //    static int compareCount = 0;
    //    public int Compare(GroupedListItem<TItem> x, GroupedListItem<TItem> y)
    //    {
    //        compareCount++;

    //        if (x.Depth == y.Depth) 
    //        {
    //            return CompareItems(x, y);
    //        }
    //        else
    //        {
    //            //not same depth, rewind deeper one until we can compare parents
    //            if (x.Depth < y.Depth)
    //            {
    //                GroupedListItem<TItem> yparent = y;
    //                while (yparent.Depth != x.Depth && yparent.Parent != null)
    //                {
    //                    yparent = yparent.Parent;
    //                }
    //                var comparison = CompareItems(x, yparent);
    //                if (comparison == 0)
    //                {
    //                    return -1;
    //                }
    //                return comparison;
    //            }
    //            else
    //            {
    //                GroupedListItem<TItem> xparent = x;
    //                while (xparent.Depth != y.Depth && xparent.Parent != null)
    //                {
    //                    xparent = xparent.Parent;
    //                }
    //                var comparison = CompareItems(xparent, y);
    //                if (comparison == 0)
    //                {
    //                    return 1;
    //                }
    //                return comparison;
    //            }

    //        }
                        
    //    }

    //    private int CompareItems(GroupedListItem<TItem> x, GroupedListItem<TItem> y)
    //    {
    //        if (x != null & y != null)
    //        {
    //            if (x.Parent == y.Parent)
    //            {
    //                return x.Index.CompareTo(y.Index);
    //            }
    //            else
    //            {
    //                var parentCompare = CompareItems(x.Parent, y.Parent);
    //                if (parentCompare == 0)
    //                {
    //                    return x.Index.CompareTo(y.Index);
    //                }
    //                else
    //                {
    //                    return parentCompare;
    //                }
    //            }
    //        }
    //        else if (x== null && y == null)
    //        {
    //            return 0;
    //        }    
    //        else
    //        {
    //            return 0;
    //        }
    //    }
    //}
}
