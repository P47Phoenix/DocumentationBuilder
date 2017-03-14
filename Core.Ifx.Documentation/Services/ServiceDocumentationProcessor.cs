using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Core.Ifx.Documentation.Models;

namespace Core.Ifx.Documentation.Services
{
    internal class ServiceDocumentationProcessor : IDocumentationProcessor
    {
        private readonly IDocumentationWriter<ServiceDescription> m_DocumentationWriter;
        private readonly ITypeParser<ServiceDescription> m_TypeParser;
        private readonly IDocumentationProcessor m_ContractDocumentationProcesor;

        public ServiceDocumentationProcessor(
            ITypeParser<ServiceDescription> typeParser,
            IDocumentationWriter<ServiceDescription> documentationWriter)
        {
            this.m_TypeParser = typeParser;
            this.m_DocumentationWriter = documentationWriter;
            this.m_ContractDocumentationProcesor = new ContractDocumentationProcessor(new ContractTypeParser(), new ContractDocumentationWriter());
        }

        public void CreateDocumentation(XDocument assemblyDocumentation, string outputDirectory, List<Type> typesInNamespaces)
        {
            List<ServiceDescription> contractDescriptions = m_TypeParser.Parse(assemblyDocumentation, typesInNamespaces);

            foreach (ServiceDescription contractDescription in contractDescriptions)
            {
                m_DocumentationWriter.WriteDocumenation(contractDescription, outputDirectory);

                m_ContractDocumentationProcesor.CreateDocumentation(assemblyDocumentation, outputDirectory, contractDescription.TypesServiceDependsOn);
            }
        }
    }
}