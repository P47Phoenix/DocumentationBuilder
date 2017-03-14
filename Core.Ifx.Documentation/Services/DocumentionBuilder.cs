using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Core.Ifx.Documentation.Models;

namespace Core.Ifx.Documentation.Services
{
    public class DocumentionBuilder
    {

        private readonly IDocumentationProcessor m_documentationProcessor;

        public DocumentionBuilder()
        {
            m_documentationProcessor = new ServiceDocumentationProcessor(new ServiceTypeParser(), new ServiceDocumentationWriter());
        }

        public void CreateDocumentation(IDocumentationOptions documentationOptions)
        {
            var assembly = Assembly.LoadFile(documentationOptions.AssemblyPath);

            XDocument assemblyDocumentation = null;
            if (File.Exists(documentationOptions.AssemblyDocumationPath))
            {
                assemblyDocumentation = XDocument.Load(documentationOptions.AssemblyDocumationPath);
            }
            var typesInAssembly = assembly.GetTypes().ToList();

            m_documentationProcessor.CreateDocumentation(documentationOptions.OutputDirectory,
                typesInAssembly, assemblyDocumentation);
        }

    }
}
