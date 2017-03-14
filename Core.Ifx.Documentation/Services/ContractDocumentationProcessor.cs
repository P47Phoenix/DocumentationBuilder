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
        private readonly ITypeParser<ContractDescription> m_typePaser;
        private readonly IDocumentationWriter<ContractDescription> m_documentationWriter;

        public ContractDocumentationProcessor(
            ITypeParser<ContractDescription> typePaser,
            IDocumentationWriter<ContractDescription> documentationWriter)
        {
            this.m_typePaser = typePaser;
            this.m_documentationWriter = documentationWriter;
        }

        public void CreateDocumentation(string outputDirectory, List<Type> typesInNamespaces, XDocument assemblyDocumentation = null)
        {
            List<ContractDescription> contractDescriptions = m_typePaser.Parse(typesInNamespaces, assemblyDocumentation);

            foreach (ContractDescription contractDescription in contractDescriptions)
            {
                m_documentationWriter.WriteDocumenation(contractDescription, outputDirectory);
            }
        }
    }
}
