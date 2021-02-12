using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
// ReSharper disable InconsistentNaming

namespace FluentUI
{
    public partial class DocumentCardActivity : FluentUIComponentBase
    {
        /// <summary>
        /// Describes the activity that has taken place, such as "Created Feb 23, 2016".
        /// </summary>
        [Parameter] public string? Activity { get; set; }

        /// <summary>
        /// One or more people who are involved in this activity.
        /// </summary>
        [Parameter] public DocumentCardActivityPerson[]? People { get; set; }

        public bool MultiPeople => People != null && People.Length > 1;

        private ICollection<IRule> DocumentCardActivityLocalRules { get; set; } = new List<IRule>();

        private Rule DetailsRule = new Rule();

        private const int VERTICAL_PADDING = 8;
        private const int HORIZONTAL_PADDING = 16;
        private const int IMAGE_SIZE = 32;
        private const int PERSONA_TEXT_GUTTER = 8;

        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
        {
            {"root", "ms-DocumentCardActivity"},
            {"multiplePeople", "ms-DocumentCardActivity--multiplePeople"},
            {"details", "ms-DocumentCardActivity-details"},
            {"name", "ms-DocumentCardActivity-name"},
            {"activity", "ms-DocumentCardActivity-activity"},
            {"avatars", "ms-DocumentCardActivity-avatars"},
            {"avatar", "ms-DocumentCardActivity-avatar"}
        };

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            ClassName = $".{GlobalClassNames["root"]} {(People != null && People.Length > 1 ? GlobalClassNames["multiplePeople"] : "")}";
            SetStyle();
        }

        protected override Task OnInitializedAsync()
        {
            CreateLocalCss();
            SetStyle();
            return base.OnInitializedAsync();
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
        }

        private void SetStyle()
        {
            DetailsRule.Properties = new CssString()
            {
                Css = $"left: {(People != null && People.Length > 1 ? $"{HORIZONTAL_PADDING + IMAGE_SIZE * 1.5 + PERSONA_TEXT_GUTTER}px" : $"{HORIZONTAL_PADDING + IMAGE_SIZE + PERSONA_TEXT_GUTTER}px")};" +
                      $"height: {IMAGE_SIZE}px;" +
                      "position: absolute;" +
                      $"top: {VERTICAL_PADDING}px;" +
                      $"width:calc(100% - {HORIZONTAL_PADDING - IMAGE_SIZE + PERSONA_TEXT_GUTTER + HORIZONTAL_PADDING}px)"
            };
        }

        private void CreateLocalCss()
        {
            DetailsRule.Selector = new ClassSelector() { SelectorName = $"{GlobalClassNames["details"]}" };
            DocumentCardActivityLocalRules.Add(DetailsRule);
        }

        public string? GetNameString()
        {
            if (People == null || People.Length == 0)
                return string.Empty;

            string? name = People[0].Name;

            if (People.Length >= 2)
            {
                name += " +" + (People.Length - 1);
            }
            
            return name;
        }

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var documentCardActivityRules = new HashSet<IRule>();

            documentCardActivityRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["root"]}" },
                Properties = new CssString()
                {
                    Css = $"padding: {VERTICAL_PADDING}px {HORIZONTAL_PADDING}px;" +
                         "position: relative;"
                }
            });

            documentCardActivityRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["avatars"]}" },
                Properties = new CssString()
                {
                    Css = $"margin-left: -2px;" +
                          "height: 32px;"
                }
            });

            documentCardActivityRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["avatar"]}" },
                Properties = new CssString()
                {
                    Css = $"display: inline-block;" +
                          "vertical-align: top;" +
                          "position: relative;" +
                          "text-align: center;" +
                          $"width: {IMAGE_SIZE}px;" +
                          $"height: {IMAGE_SIZE}px;"
                }
            });

            documentCardActivityRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["avatar"]}&:after" },
                Properties = new CssString()
                {
                    Css = $"content: ' ';" +
                          "position: absolute;" +
                          "left: -1px;" +
                          "top: -1px;" +
                          "right: -1px;" +
                          "bottom: -1px;" +
                          $"border: 2px solid {theme.Palette.White};" +
                          $"border-radius: 50%;"
                }
            });
            documentCardActivityRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["avatar"]}:nth-of-type(2)" },
                Properties = new CssString()
                {
                    Css = $"margin-left: -16px;"
                }
            });
            
            documentCardActivityRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["name"]}" },
                Properties = new CssString()
                {
                    Css = $"display: block;" +
                         $"font-size: {theme.FontStyle.FontSize.Small};" +
                         $"line-height: 15px;" +
                         $"height: 15px;" +
                         $"overflow: hidden;" +
                         $"text-overflow: ellipsis;" +
                         $"white-space: nowrap;" +
                         $"color: {theme.Palette.NeutralPrimary};" +
                         $"font-weight: {theme.FontStyle.FontWeight.SemiBold};"
                }
            }); 
            
            documentCardActivityRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["activity"]}" },
                Properties = new CssString()
                {
                    Css = $"display: block;" +
                         $"font-size: {theme.FontStyle.FontSize.Small};" +
                         $"line-height: 15px;" +
                         $"height: 15px;" +
                         $"overflow: hidden;" +
                         $"text-overflow: ellipsis;" +
                         $"white-space: nowrap;" +
                         $"color: {theme.Palette.NeutralSecondary};" 
                }
            });


            return documentCardActivityRules;
        }
    }
}
