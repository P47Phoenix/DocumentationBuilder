using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Core.Ifx.Documentation.Services
{
    public interface IDocumentationProcessor
    {
        void CreateDocumentation(string outputDirectory, List<Type> typesInNamespaces, XDocument assemblyDocumentation = null);
    }

}