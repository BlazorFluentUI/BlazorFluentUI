using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI.Demo.Shared.Components
{
    public partial class Demo
    {
        [Inject] ThemeProvider? ThemeProvider { get; set; }
        //[Inject] HttpClient? HttpClient { get; set; }

        ITheme Theme => ThemeProvider!.Theme;

        string codeLabel = "Show Code";
        bool showCode = false;

        bool showCCode = false;

        string codeLiteral = "";
        string ccodeLiteral = "";

        [Parameter] public string? Header { get; set; }

        [Parameter] public string? MetadataPath { get; set; }

        [Parameter] public int Key { get; set; } = -1;//This is required and must be unique.

        [Parameter] public RenderFragment? ChildContent { get; set; }

        protected override async Task OnInitializedAsync()
        {
            
            //HelloWorld.SayHello();
            if (Key == -1)
                throw new Exception("Must set Key with an integer 0 or greater and must be unique within page.");

            // Source inspired by AntDesign (https://github.com/Append-IT/ant-design-blazor/blob/master/docs/Append.AntDesign.Documentation/Infrastructure/DocumentationService.cs)
            //string demoMetaData = await HttpClient!.GetStringAsync($"md/{MetadataPath}.md");
            var demoMetaData = BlazorFluentUI.Demo.DemoGenerated.GetRazor(MetadataPath);
            var start = 0;
            var end = demoMetaData.Length;
            bool found = false;
            while ((start < end) && (!found))
            {
                int indexOfDemoBegin = demoMetaData.IndexOf("<Demo", start) + 6;
                int keyValueBegin = demoMetaData.IndexOf("Key=\"", indexOfDemoBegin) + 5;
                int keyValueEnd = demoMetaData.IndexOf("\"", keyValueBegin);
                int keyValue = 0;
                int.TryParse(demoMetaData.Substring(keyValueBegin, keyValueEnd - keyValueBegin), out keyValue);

                if (keyValue == Key)
                {
                    found = true;
                    //var codeBegin = demoMetaData.IndexOf(">", indexOfDemoBegin) + 1;
                    var codeBegin = demoMetaData.IndexOf(Environment.NewLine, indexOfDemoBegin) + 1;
                    var codeEnd = demoMetaData.IndexOf("</Demo>", indexOfDemoBegin);
                    codeLiteral = demoMetaData.Substring(codeBegin, codeEnd - codeBegin);
                    codeLiteral = codeLiteral.Replace("                ", "");
                }

                start = indexOfDemoBegin;
            }

            int ccodeIndex = demoMetaData.IndexOf("@code{");
            if (ccodeIndex == -1)
            {
                ccodeIndex = demoMetaData.IndexOf("@code ");
                if (ccodeIndex == -1)
                {
                    await base.OnParametersSetAsync();
                    return; //can't find code block
                }
                ccodeIndex = demoMetaData.IndexOf("{", ccodeIndex) + 1;
            }
            else
            {
                ccodeIndex += 6;
            }
            var lastIndex = demoMetaData.LastIndexOf("}");

            ccodeLiteral = "@code {\n" + demoMetaData.Substring(ccodeIndex, lastIndex - ccodeIndex) + "\n}";


            await base.OnInitializedAsync();
        }

    }
}
