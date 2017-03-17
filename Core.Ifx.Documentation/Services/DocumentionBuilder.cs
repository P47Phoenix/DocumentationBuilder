using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Core.Ifx.Documentation.Models;
using Core.Ifx.Documentation.Services.Formators;
using Core.Ifx.Documentation.Services.Questions;

namespace Core.Ifx.Documentation.Services
{
    public class DocumentionBuilder
    {

        private readonly IDocumentationProcessor m_documentationProcessor;

        public DocumentionBuilder()
        {
            m_documentationProcessor = new ServiceDocumentationProcessor(
                new ServiceTypeParser(
                    new List<IDocumentTypeAsServiceQuestion>
                    {
                        new ApiControllerDocumentTypeAsServiceQuestion(),
                        new ControllerDocumentTypeAsServiceQuestion(),
                        new ServiceContractDocumentTypeAsServiceQuestion()
                    },
                    new List<IDocumentMethodQuestion>
                    {
                        new ControllerDocumentMethodQuestion(),
                        new OperationContractDocumentMethodQuestion()
                    },
                    new MethodDependencyFinder(
                        new List<IDocumentMethodDependencyQuestion>
                        {
                            new DefaultDocumentMethodDependencyQuestion()
                        }),
                    new FormatorFactory()),
                new ServiceDocumentationWriter());
        }

        public bool TryCreateDocumentation(IDocumentationOptions documentationOptions)
        {
            try
            {
                var assembly = Assembly.LoadFile(documentationOptions.AssemblyPath);

                var directory = Path.GetDirectoryName(documentationOptions.AssemblyPath);

                ResolveEventHandler resolveEventHandler = (sender, args) =>
                {
                    var assemblyNameParts = args.Name.Split(',');

                    var assemblyName = string.Concat(assemblyNameParts[0], ".dll");

                    var assemblyNameFullpath = Path.Combine(directory, assemblyName);

                    if (File.Exists(assemblyNameFullpath))
                    {
                        return Assembly.LoadFile(assemblyNameFullpath);
                    }

                    return null;
                };

                AppDomain.CurrentDomain.AssemblyResolve += resolveEventHandler;

                try
                {

                    XDocument assemblyDocumentation = null;

                    if (File.Exists(documentationOptions.AssemblyDocumationPath))
                    {
                        assemblyDocumentation = XDocument.Load(documentationOptions.AssemblyDocumationPath);
                    }

                    var typesInAssembly = assembly.GetTypes().ToList();

                    m_documentationProcessor.CreateDocumentation(documentationOptions.OutputDirectory,
                        typesInAssembly, assemblyDocumentation);
                }
                finally
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= resolveEventHandler;
                }

                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());

                return false;
            }
        }
    }
}
