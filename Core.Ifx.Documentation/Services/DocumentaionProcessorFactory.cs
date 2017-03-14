using Core.Ifx.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
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

            var typesInNamespaces = typesInAssembly
                .Where(type => type.GetCustomAttribute<ServiceContractAttribute>() != null)
                .ToList();

            return new ServiceDocumentationProcessor(assemblyDocumentation, typesInNamespaces, documentationOptions.OutputDirectory, new ServiceTypeParser(), new ServiceDocumentationWriter());
        }
    }
}
