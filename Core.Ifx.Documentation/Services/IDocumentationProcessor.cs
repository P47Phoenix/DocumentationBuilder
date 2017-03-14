using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Core.Ifx.Documentation.Services
{
    public interface IDocumentationProcessor
    {
        void CreateDocumentation(XDocument assemblyDocumentation, string outputDirectory, List<Type> typesInNamespaces);
    }

}