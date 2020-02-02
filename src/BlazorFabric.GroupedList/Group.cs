using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class Group<TItem>
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int StartIndex { get; set; } 
        public int GroupIndex { get; set; }
        public int Count { get; set; }
        public IEnumerable<Group<TItem>> Children { get; set; }  //distinction from react, these are original subitems rather than nested groups
        public int Level { get; set; }
        public TItem Item { get; set; }

        //no isSelected ... controlled by store

        public bool IsCollapsed { get; set; } = false;
        public bool IsShowingAll { get; set; }
        public bool IsDropEnabled { get; set; }
        public object Data { get; set; }
        public string AriaLabel { get; set; }
        public bool HasMoreData { get; set; }

        public delegate IEnumerable<Group<TItem>> CreateGroupsDelegate(
            IEnumerable<TItem> items,
            Func<TItem, object> groupKeySelector,
            Func<TItem, IEnumerable<TItem>> subGroupSelector,
            CreateGroupsDelegate createGroups,
            ref int index, 
            int currentLevel);

        public static IEnumerable<Group<TItem>> CreateGroups(
           IEnumerable<TItem> items,
           Func<TItem, object> groupKeySelector,
           Func<TItem, IEnumerable<TItem>> subGroupSelector,
           ref int index,
           int level)
        {
            if (items != null)
            {
                System.Collections.Generic.List<Group<TItem>> groups = new System.Collections.Generic.List<Group<TItem>>();
                var groupIndex = 0;
                foreach (var item in items)
                {
                    var group = new Group<TItem>(item, groupKeySelector, subGroupSelector, ref index, level);
                    group.GroupIndex = groupIndex++;
                    groups.Add(group);
                }
                return groups;
            }
            return null;
        }

        public Group()
        {  }

        public Group(
            TItem item, 
            Func<TItem, object> groupKeySelector, 
            Func<TItem, IEnumerable<TItem>> subGroupSelector,
            //CreateGroupsDelegate createGroups,
            ref int index, 
            int level = 0)
        {
            this.Item = item;
            this.Name = groupKeySelector(item).ToString();
            this.Key = groupKeySelector(item).ToString();
            var items = subGroupSelector(item);
            this.StartIndex = index;
            index++;
            this.Level = level;
            level++;
            this.Children = CreateGroups(items, groupKeySelector, subGroupSelector, ref index, level);
        }
    }
}
