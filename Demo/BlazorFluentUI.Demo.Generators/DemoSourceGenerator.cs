using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace BlazorFluentUI.Demo.Generators
{
    [Generator]
    public class DemoSourceGenerator : ISourceGenerator
    {

        public void Execute(GeneratorExecutionContext context)
        {
            Debug.WriteLine("Execute code generator");
            // begin creating the source we'll inject into the users compilation
            StringBuilder sourceBuilder = new StringBuilder($@"
using System;
using System.Collections.Generic;
using System.Linq;
namespace BlazorFluentUI.Demo
{{
    public static class DemoGenerated
    {{
        public static string GetRazor(string name) 
        {{

");
            var files = context.AdditionalFiles;
            var dictionary = files.ToDictionary(x => x.Path, x => x.GetText().ToString().Replace(@"""", @""""""));
            sourceBuilder.AppendLine("var metadata = new Dictionary<string,string>() {");
            foreach (var pair in dictionary)
            {
                sourceBuilder.AppendLine($@"{{ @""{pair.Key}"", @""{pair.Value}"" }},");
            }
            sourceBuilder.AppendLine("};");

            sourceBuilder.AppendLine($@"var foundPair = metadata.FirstOrDefault(x => x.Key.EndsWith(""\\"" + name + "".razor""));");

            sourceBuilder.AppendLine(@"return foundPair.Value;");

            // finish creating the source to inject
            sourceBuilder.Append(@"
        }
    }
}");
            // inject the created source into the users compilation
            context.AddSource("demoGenerated", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
#endif
        }
    }
}
