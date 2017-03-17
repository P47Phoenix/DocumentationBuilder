using System;
using System.Diagnostics;
using Core.Ifx.Documentation.Services;
using Newtonsoft.Json;

namespace Core.Agent.DocumentationBuilder
{
    public class Program
    {
        static int Main(string[] args)
        {
            Program p = new Program();

            DocumentParserOptions options = p.ParseArgs(args);

            return p.Run(options);
        }

        private DocumentParserOptions ParseArgs(string[] args)
        {
            DocumentParserOptions options = new DocumentParserOptions();

            CommandLine.Parser.Default.ParseArguments(args, options);

            WriteValueConsole(options);
            WriteValueDebug(options);

            return options;
        }

        [Conditional("DEBUG")]
        private void WriteValueDebug(DocumentParserOptions options)
        {
            Debug.Write($"Args: {JsonConvert.SerializeObject(options, Formatting.Indented)}");
        }

        private static void WriteValueConsole(DocumentParserOptions options)
        {
            Console.WriteLine($"Args: {JsonConvert.SerializeObject(options, Formatting.Indented)}");
        }

        private int Run(DocumentParserOptions options)
        {
            var builder = new DocumentionBuilder();

            var success = builder.TryCreateDocumentation(options);

            if (success)
            {
                return 0;
            }
            else
            {
                if (Debugger.IsAttached)
                {
                    Console.WriteLine("Press enter to clode");
                    Console.ReadLine();
                }
                
                return 1;
            }

        }
    }
}
