using Core.Ifx.Documentation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Agent.DocumentationBuilder
{
    public class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            DocumentParserOptions options = p.ParseArgs(args);

            p.Run(options);
        }

        private DocumentParserOptions ParseArgs(string[] args)
        {
            DocumentParserOptions options = new DocumentParserOptions();

            CommandLine.Parser.Default.ParseArguments(args, options);

            return options;
        }

        private void Run(DocumentParserOptions options)
        {
            var builder = new Ifx.Documentation.DocumentionBuilder(new DocumentaionProcessorFactory());

            builder.CreateDocumentation(options);
        }
    }
}
