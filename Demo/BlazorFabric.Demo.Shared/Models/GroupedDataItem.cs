using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlazorFabric.Demo.Shared.Models
{
    public class GroupedDataItem : DataItem
    {
        public IEnumerable<GroupedDataItem> Data { get; set; }

        public GroupedDataItem(IGrouping<int, DataItem> grouping)
        {
            Data = grouping.Select(x => new GroupedDataItem(x)).ToList();
            Key = grouping.Key.ToString();
        }

        public GroupedDataItem(DataItem dataItem)
        {
            DisplayName = dataItem.DisplayName;
            Type = dataItem.Type;
            Key = dataItem.Key;
            Description = dataItem.Description;

        }

    }
}
