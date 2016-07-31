using System;
using Core.Ifx.Documentation;
using Core.Ifx.Documentation.Models;
using CommandLine;
using CommandLine.Text;

namespace Core.Agent.DocumentationBuilder
{
    public class DocumentParserOptions : IDocumentationOptions
    {
        [Option('d', HelpText = "The path to assembly documentation xml ")]
        public string AssemblyDocumationPath { get; set; }

        [Option('a', HelpText = "The path to the assembly we want documentation on")]
        public string AssemblyPath { get; set; }

        [Option('t', DefaultValue = DocumentationType.Contract, HelpText = "What type of documentation are you trying to create")]
        public DocumentationType DocumentationType { get; set; }

        [Option('n', "The namespaces to look at in the given assembly")]
        public string Namespace { get; set; }

        [Option('o', HelpText = "The output directory for the documantation")]
        public string OutputDirectory { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}