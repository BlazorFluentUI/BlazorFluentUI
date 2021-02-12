using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FluentUI
{
    public interface IContextualMenuItem
    {
        string AriaLabel { get; set; }
        bool CanCheck { get; set; }
        bool Checked { get; set; }
        string ClassName { get; set; }
        object Data { get; set; }
        bool Disabled { get; set; }
        string Href { get; set; }
        //object IconProps { get; set; }
        string IconName { get; set; }
        string IconSrc { get; set; }
        ContextualMenuItemType ItemType { get; set; }
        string Key { get; set; }
        object KeytipProps { get; set; }
        Action<ItemClickedArgs> OnClick { get; set; }
        //onRender
        //onRenderIcon
        bool PrimaryDisabled { get; set; }
        string Rel { get; set; }
        string Role { get; set; }
        string SecondaryText { get; set; }
        //sectionProps
        string ShortCut { get; set; }
        bool Split { get; set; }
        string Style { get; set; }
        //subMenuProps
        //subMenuIconProps
        string Target { get; set; }
        string Text { get; set; }
        string Title { get; set; }

        ICommand Command { get; set; }
        object CommandParameter { get; set; }
        IEnumerable<IContextualMenuItem> Items { get; set; }
    }
}
