namespace BlazorFluentUI
{
    public interface IBFUPivotItem
    {
        string LinkText {get;set;}
        string HeaderText {get;set;}
        string ItemKey {get;set;}
        string ItemCount {get;set;}
        string ItemIcon {get;set;}
        string KeyTip {get;set;}
    }
}