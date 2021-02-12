using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FluentUI.Models
{
    public class ResizeGroupData
    {
        public ObservableCollection<IRibbonItem> Items { get; set; } = new ObservableCollection<IRibbonItem>();
        public ObservableCollection<IRibbonItem> OverflowItems { get; set; } = new ObservableCollection<IRibbonItem>();

        public bool ShowDelimiter { get; set; }


        public event EventHandler Changed;
        IEnumerable<IRibbonItem> allItems;

        public ResizeGroupData(IEnumerable<IRibbonItem> allItems, bool isLastGroupInTab)// IEnumerable<IRibbonItem> items, IEnumerable<IRibbonItem> overflowItems, string cacheKey)
        {
            this.allItems = allItems;
            foreach(var item in allItems)
            {
                Items.Add(item);
            }
            ShowDelimiter = !isLastGroupInTab;
  
        }

        public double LowestPriorityInItems()
        {
            if (Items.Count() > 0)
            {
                return Items.Min(item => item.Priority);
            }
            else
            {
                return double.MaxValue;
            }
        }

        public double HighestPriorityInOverflowItems()
        {
            if (OverflowItems.Count() > 0)
            {
                return OverflowItems.Max(item => item.Priority);
            }
            else
            {
                return -1;
            }
        }

        public bool Shrink()
        {
            return Rearange(Items, OverflowItems,false);
        }

        public bool Grow()
        {
            return Rearange(OverflowItems, Items,true);
        }

        bool Rearange(ObservableCollection<IRibbonItem> source, ObservableCollection<IRibbonItem> destination, bool highPrio)
        {
            if (source.Count() > 0)
            {
                #region get the shrink item
                IRibbonItem? itemToMove = source[0];
                foreach (var item in source)
                {
                    if (highPrio)
                    {
                        if (item.Priority >= itemToMove.Priority)
                        {
                            itemToMove = item;
                        }
                    }
                    else
                    {
                        if (item.Priority <= itemToMove.Priority)
                        {
                            itemToMove = item;
                        }
                    }
                }
                #endregion

                // Remove the item from the direct visible elements
                //  var lastItem = Items.Last();
                source.Remove(itemToMove);

                #region get the insert index
                int indexOfItemToShrink = Array.IndexOf(allItems.ToArray(), itemToMove);
                int insertIndex = 0;
                for (; insertIndex < destination.Count; insertIndex++)
                {
                    int listIndex = Array.IndexOf(allItems.ToArray(), destination[insertIndex]);
                    if (listIndex > indexOfItemToShrink)
                    {
                        break;
                    }
                }
                destination.Insert(insertIndex, itemToMove);
                #endregion


                Changed?.Invoke(this, null);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
