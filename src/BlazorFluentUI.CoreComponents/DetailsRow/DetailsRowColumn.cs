using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlazorFluentUI
{
    public class DetailsRowColumn<TItem, TProp> : IDetailsRowColumn<TItem>
    {

        private Func<object, bool>? _filterPredicate;
        public Func<object, bool>? FilterPredicate
        {
            get => _filterPredicate;
            set
            {
                _filterPredicate = value;
                OnPropertyChanged();
                OnPropertyChanged("InternalFilterPredicate");
            }
        }

        //private Func<TProp, bool>? _filterPredicate;
        //public new Func<TProp, bool>? FilterPredicate
        //{
        //    get => _filterPredicate;
        //    set
        //    {
        //       // IDetailsRowColumn<TItem>.FilterPredicate = x => FilterPredicate == null || FilterPredicate((TProp)x);
        //        _filterPredicate = value;
        //        OnPropertyChanged();
        //        OnPropertyChanged("InternalFilterPredicate");
        //    }
        //}

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

        public string? AriaLabel { get; set; }
        public double CalculatedWidth { get; set; } = double.NaN;
        public ColumnActionsMode ColumnActionsMode { get; set; } = ColumnActionsMode.Clickable;
        public RenderFragment<TItem>? ColumnItemTemplate { get; set; }

        object? IDetailsRowColumn<TItem>.InternalColumnItemTemplate => ColumnItemTemplate;

        public Func<TItem, object>? FieldSelector { get; set; }

        Expression<Func<TItem, object>>? IDetailsRowColumn<TItem>.FieldSelectorExpression { get => null; set => _=value; }

        public string? FilterAriaLabel { get; set; }

        
        public string? GroupAriaLabel { get; set; }
        public string? IconClassName { get; set; }
        public string? IconName { get; set; }
        public string? IconSrc { get; set; }
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
        public string? Key { get; set; }
        public double MaxWidth { get; set; } = 300;
        public double MinWidth { get; set; } = 100;
        public string? Name { get; set; }
        public Action<IDetailsRowColumn<TItem>>? OnColumnClick { get; set; }
        public Action<IDetailsRowColumn<TItem>>? OnColumnContextMenu { get; set; }
        public Type? PropType { get; protected set; }
        public string? SortedAscendingAriaLabel { get; set; }
        public string? SortedDescendingAriaLabel { get; set; }
        public Type? Type { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IObservable<PropertyChangedEventArgs>? WhenPropertyChanged { get; private set; }
        
        protected void Initialize()
        {
            WhenPropertyChanged = Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
              handler =>
              {
                  void changed(object? sender, PropertyChangedEventArgs e) => handler(e);
                  return changed;
              },
              handler => PropertyChanged += handler,
              handler => PropertyChanged -= handler);
        }
    }

    public class DetailsRowColumn<TItem> : IDetailsRowColumn<TItem>
    {


        public Expression<Func<TItem, object>>? FieldSelectorExpression { get; set; }

        public string? AriaLabel { get; set; }
        public double CalculatedWidth { get; set; } = double.NaN;
        public ColumnActionsMode ColumnActionsMode { get; set; } = ColumnActionsMode.Clickable;
        public RenderFragment<DynamicAccessor<TItem>>? ColumnItemTemplate { get; set; }


        object? IDetailsRowColumn<TItem>.InternalColumnItemTemplate => ColumnItemTemplate;

        public Func<TItem, object>? FieldSelector { get; set; }
        public string? FilterAriaLabel { get; set; }

                
        private Func<object, bool>? _filterPredicate;
        public Func<object, bool>? FilterPredicate
        {
            get => _filterPredicate;
            set
            {
                _filterPredicate = value; 
                OnPropertyChanged();
                OnPropertyChanged("InternalFilterPredicate");
            }
        }

        public string? GroupAriaLabel { get; set; }
        public string? IconClassName { get; set; }
        public string? IconName { get; set; }
        public string? IconSrc { get; set; }
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
        public string? Key { get; set; }
        public double MaxWidth { get; set; } = 300;
        public double MinWidth { get; set; } = 100;
        public string? Name { get; set; }
        public Action<IDetailsRowColumn<TItem>>? OnColumnClick { get; set; }
        public Action<IDetailsRowColumn<TItem>>? OnColumnContextMenu { get; set; }
        public Type? PropType { get; protected set; }
        public string? SortedAscendingAriaLabel { get; set; }
        public string? SortedDescendingAriaLabel { get; set; }
        public Type? Type { get; set; }

        public DetailsRowColumn()
        { }

        public DetailsRowColumn(string fieldName, Expression<Func<TItem, object>> fieldSelectorExpression)
        {
            Name = fieldName;
            Key = fieldName;
            AriaLabel = fieldName;
            FieldSelectorExpression = fieldSelectorExpression;
            FieldSelector = fieldSelectorExpression.Compile();
            _ = DetailsRowUtils.GetPropertyInfo(fieldSelectorExpression);
            _ = DetailsRowUtils.GetSetter(fieldSelectorExpression);
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IObservable<PropertyChangedEventArgs>? WhenPropertyChanged { get; private set; }


        protected void Initialize()
        {
            WhenPropertyChanged = Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
              handler =>
              {
                  void changed(object? sender, PropertyChangedEventArgs e) => handler(e);
                  return changed;
              },
              handler => PropertyChanged += handler,
              handler => PropertyChanged -= handler);
        }
    }

}
