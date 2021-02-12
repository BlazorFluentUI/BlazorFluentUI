using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace FluentUI
{
    public class DetailsRowColumn<TItem, TProp> : DetailsRowColumn<TItem>
    {
        private Func<TProp, bool>? _filterPredicate;
        public new Func<TProp, bool>? FilterPredicate
        {
            get => _filterPredicate;
            set
            {
                base.FilterPredicate = x => FilterPredicate != null ? FilterPredicate((TProp)x) : true;
                _filterPredicate = value;
            }
        }

        public DetailsRowColumn()
        {
            PropType = typeof(TProp);
            Initialize();
        }

        public DetailsRowColumn(string fieldName, Func<TItem, object> fieldSelector)
        {
            PropType = typeof(TProp);

            Name = fieldName;
            Key = fieldName;
            AriaLabel = fieldName;
            FieldSelector = fieldSelector;

            Initialize();
        }
    }

    public class DetailsRowColumn<TItem>
    {
        public DetailsRowColumn()
        { }
        public DetailsRowColumn(string fieldName, Func<TItem, IComparable> fieldSelector)
        {
            Name = fieldName;
            Key = fieldName;
            AriaLabel = fieldName;
            FieldSelector = fieldSelector;
        }

        public string AriaLabel { get; set; }
        public double CalculatedWidth { get; set; } = double.NaN;
        public ColumnActionsMode ColumnActionsMode { get; set; } = ColumnActionsMode.Clickable;
        public RenderFragment<object> ColumnItemTemplate { get; set; }
        public Func<TItem, object> FieldSelector { get; set; }
        public string FilterAriaLabel { get; set; }

        private Func<object, bool>? _filterPredicate;
        public Func<object, bool>? FilterPredicate
        {
            get => _filterPredicate;
            set
            {
                _filterPredicate = value; OnPropertyChanged();
                //if (_filterPredicate == value) return; else { _filterPredicate = value; OnPropertyChanged(); } 
            }
        }

        public string GroupAriaLabel { get; set; }
        public string IconClassName { get; set; }
        public string IconName { get; set; }
        public string IconSrc { get; set; }
        /// <summary>
        /// Forces columns to be in a particular order.  Useful for libraries (like DynamicData) that don't maintain order of collections internally.
        /// </summary>
        public int Index { get; set; }
        public bool IsCollapsible { get; set; }
        public bool IsFiltered { get; set; }
        public bool IsGrouped { get; set; }
        public bool IsIconOnly { get; set; }
        public bool IsMenuOpen { get; set; }
        public bool IsMultiline { get; set; }
        public bool IsPadded { get; set; }
        public bool IsResizable { get; set; }
        public bool IsRowHeader { get; set; }  // only one can be set, it's for the "role" (and a style is set, too)

        private bool _isSorted;
        public bool IsSorted { get => _isSorted; set { if (_isSorted == value) return; else { _isSorted = value; OnPropertyChanged(); } } }

        private bool _isSortedDescending;
        public bool IsSortedDescending { get => _isSortedDescending; set { if (_isSortedDescending == value) return; else { _isSortedDescending = value; OnPropertyChanged(); } } }
        public string Key { get; set; }
        public double MaxWidth { get; set; } = 300;
        public double MinWidth { get; set; } = 100;
        public string Name { get; set; }
        public Action<DetailsRowColumn<TItem>> OnColumnClick { get; set; }
        public Action<DetailsRowColumn<TItem>> OnColumnContextMenu { get; set; }
        public Type PropType { get; protected set; }
        public string SortedAscendingAriaLabel { get; set; }
        public string SortedDescendingAriaLabel { get; set; }
        public Type Type { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IObservable<PropertyChangedEventArgs> WhenPropertyChanged { get; private set; }

        protected void Initialize()
        {
            WhenPropertyChanged = Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
              handler =>
              {
                  PropertyChangedEventHandler changed = (sender, e) => handler(e);
                  return changed;
              },
              handler => PropertyChanged += handler,
              handler => PropertyChanged -= handler);
        }
    }

}
