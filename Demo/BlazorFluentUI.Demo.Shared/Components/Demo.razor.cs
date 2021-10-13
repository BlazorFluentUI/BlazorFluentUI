using Microsoft.AspNetCore.Components;

namespace BlazorFluentUI.Demo.Shared.Components
{
    public partial class Demo : ComponentBase
    {
        [Inject] ThemeProvider? ThemeProvider { get; set; }

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
            if (Key == -1)
                throw new Exception("Must set Key with an integer 0 or greater and must be unique within page.");

            string demoMetaData = BlazorFluentUI.Demo.DemoGenerated.GetRazor(MetadataPath!);
            int start = 0;
            int end = demoMetaData.Length;
            bool found = false;

            while ((start < end) && (!found))
            {
                int indexOfDemoBegin = demoMetaData.IndexOf("<Demo", start) + 6;
                int keyValueBegin = demoMetaData.IndexOf("Key=\"", indexOfDemoBegin) + 5;
                int keyValueEnd = demoMetaData.IndexOf("\"", keyValueBegin);

                if (int.TryParse(demoMetaData[keyValueBegin..keyValueEnd], out int keyValue) && keyValue == Key)
                    found = true;

				int codeBegin = demoMetaData.IndexOf('\n', indexOfDemoBegin) + 1;
				int codeEnd = demoMetaData.IndexOf("</Demo>", indexOfDemoBegin);
				codeLiteral = demoMetaData[codeBegin..codeEnd].Replace("                ", "");
				start = codeEnd;
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

            ccodeLiteral = "@code {\n" + demoMetaData[ccodeIndex..lastIndex] + "\n}";


            //await base.OnInitializedAsync();
        }

    }
}