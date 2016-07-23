using Core.Ifx.Documentation.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Core.Ifx.Documentation.Services
{
    public class ContractDocumentationProcessor : IDocumentationProcessor
    {
        private readonly XDocument m_assemblyDocumentation;
        private readonly string m_outputDirectory;
        private readonly List<Type> m_typesInNamespaces;
        private readonly ITypeParser<ContractDescription> m_typePaser;
        private readonly IDocumentationWriter<ContractDescription> m_documentationWriter;

        public ContractDocumentationProcessor(
            XDocument assemblyDocumentation,
            List<Type> typesInNamespaces,
            string outputDirectory,
            ITypeParser<ContractDescription> typePaser,
            IDocumentationWriter<ContractDescription> documentationWriter)
        {
            this.m_assemblyDocumentation = assemblyDocumentation;
            this.m_typesInNamespaces = typesInNamespaces;
            this.m_outputDirectory = outputDirectory;
            this.m_typePaser = typePaser;
            this.m_documentationWriter = documentationWriter;
        }

        public void CreateDocumentation()
        {
            List<ContractDescription> contractDescriptions = m_typePaser.Parse(m_assemblyDocumentation, m_typesInNamespaces);

            foreach (ContractDescription contractDescription in contractDescriptions)
            {
                m_documentationWriter.WriteDocumenation(contractDescription, m_outputDirectory);
            }
        }
    }
}
