using Core.Ifx.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Ifx.Documentation.Services
{
    public class DocumentaionProcessorFactory : IDocumentaionProcessorFactory
    {
        public IDocumentationProcessor Create(IDocumentationOptions documentationOptions)
        {
            var assembly = Assembly.LoadFile(documentationOptions.AssemblyPath);

            var assemblyDocumentation = XDocument.Load(documentationOptions.AssemblyDocumationPath);

            var typesInAssembly = assembly.GetTypes();

            var typesInNamespaces = typesInAssembly.Where(type => type.Namespace.ToLower().StartsWith(documentationOptions.Namespace.ToLower())).ToList();

            switch (documentationOptions.DocumentationType)
            {
                case DocumentationType.Contract:
                    return new ContractDocumentationProcessor(assemblyDocumentation, typesInNamespaces, documentationOptions.OutputDirectory, new ContractTypeParser(), new ContractDocumentationWriter());
                case DocumentationType.Service:
                    return new ServiceDocumentationProcessor(assemblyDocumentation, typesInNamespaces);
                default:
                    return null;
            }

        }
    }
}
