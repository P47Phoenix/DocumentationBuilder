using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Core.Ifx.Documentation.Models;

namespace Core.Ifx.Documentation.Services
{
    internal class ServiceDocumentationProcessor : IDocumentationProcessor
    {
        private readonly XDocument m_AssemblyDocumentation;
        private readonly List<Type> m_TypesInNamespaces;
        private readonly IDocumentationWriter<ServiceDescription> m_DocumentationWriter;
        private readonly string m_OutputDirectory;
        private readonly ITypeParser<ServiceDescription> m_TypeParser;

        public ServiceDocumentationProcessor(XDocument assemblyDocumentation, List<Type> typesInNamespaces,
            string outputDirectory,
            ITypeParser<ServiceDescription> typeParser,
            IDocumentationWriter<ServiceDescription> documentationWriter)
        {
            this.m_AssemblyDocumentation = assemblyDocumentation;
            this.m_TypesInNamespaces = typesInNamespaces;
            this.m_OutputDirectory = outputDirectory;
            this.m_TypeParser = typeParser;
            this.m_DocumentationWriter = documentationWriter;

        }

        public void CreateDocumentation()
        {
            List<ServiceDescription> contractDescriptions = m_TypeParser.Parse(m_AssemblyDocumentation, m_TypesInNamespaces);

            foreach (ServiceDescription contractDescription in contractDescriptions)
            {
                m_DocumentationWriter.WriteDocumenation(contractDescription, m_OutputDirectory);
            }
        }
    }
}