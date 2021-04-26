using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace BlazorFluentUI
{

    public interface IDetailsRowColumn<TItem>
    {
        string? AriaLabel { get; set; }
        double CalculatedWidth { get; set; }
        ColumnActionsMode ColumnActionsMode { get; set; }

        Func<object, bool>? FilterPredicate { get; set; }
        object? InternalColumnItemTemplate { get;  }
        Func<TItem, object>? FieldSelector { get; set; }
        Expression<Func<TItem, object>>? FieldSelectorExpression { get; set; }
        string? FilterAriaLabel { get; set; }
        
        string? GroupAriaLabel { get; set; }
        string? IconClassName { get; set; }
        string? IconName { get; set; }
        string? IconSrc { get; set; }
        int Index { get; set; }
        bool IsCollapsible { get; set; }
        bool IsFiltered { get; set; }
        bool IsGrouped { get; set; }
        bool IsIconOnly { get; set; }
        bool IsMenuOpen { get; set; }
        bool IsMultiline { get; set; }
        bool IsPadded { get; set; }
        bool IsResizable { get; set; }
        bool IsRowHeader { get; set; }
        bool IsSorted { get; set; }
        bool IsSortedDescending { get; set; }
        string? Key { get; set; }
        double MaxWidth { get; set; }
        double MinWidth { get; set; }
        string? Name { get; set; }
        Action<IDetailsRowColumn<TItem>>? OnColumnClick { get; set; }
        Action<IDetailsRowColumn<TItem>>? OnColumnContextMenu { get; set; }
        Type? PropType { get; }
        string? SortedAscendingAriaLabel { get; set; }
        string? SortedDescendingAriaLabel { get; set; }
        Type? Type { get; set; }
        IObservable<PropertyChangedEventArgs>? WhenPropertyChanged { get; }

        event PropertyChangedEventHandler? PropertyChanged;
    }
}