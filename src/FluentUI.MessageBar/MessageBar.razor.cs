using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class MessageBar : FluentUIComponentBase
    {
        [Parameter]
        public bool IsMultiline { get; set; } = true;

        //[Parameter]
        //public MessageBarBase ComponentRef { get; set; }

        [Parameter]
        public MessageBarType MessageBarType { get; set; } = MessageBarType.Info;

        [Parameter]
        public bool Truncated { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment Actions { get; set; }

        [Parameter]
        public string DismissButtonAriaLabel { get; set; }

        [Parameter]
        public string OverflowButtonAriaLabel { get; set; }

        [Parameter]
        public EventCallback OnDismiss { get; set; }

        [Parameter]
        public MessageBar ComponentRef
        {
            get => componentRef;
            set
            {
                if (value == componentRef)
                    return;
                componentRef = value;
                MessageBarType = value.MessageBarType;
                ComponentRefChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<MessageBar> ComponentRefChanged { get; set; }

        private MessageBar componentRef;

        protected bool HasDismiss { get => (OnDismiss.HasDelegate); }

        protected bool HasExpand { get => (Truncated && Actions == null); }

        protected bool ExpandSingelLine { get; set; }

        protected void Truncate()
        {
            ExpandSingelLine = !ExpandSingelLine;
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
                ComponentRef = this as MessageBar;
            return base.OnAfterRenderAsync(firstRender);
        }

        protected string GetTypeCss()
        {
            switch (MessageBarType)
            {
                case MessageBarType.Warning:
                    return " ms-MessageBar--warning ";
                case MessageBarType.Error:
                    return " ms-MessageBar--error ";
                case MessageBarType.Blocked:
                    return " ms-MessageBar--blocked ";
                case MessageBarType.SevereWarning:
                    return " ms-MessageBar--severeWarning ";
                case MessageBarType.Success:
                    return " ms-MessageBar--success ";
                default:
                    return "";
            }
        }

      
    }
}
