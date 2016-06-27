using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Core.Ifx.Documentation
{
    internal class ServiceDocumentationProcessor : IDocumentationProcessor
    {
        private XDocument assemblyDocumentation;
        private List<Type> typesInNamespaces;

        public ServiceDocumentationProcessor(XDocument assemblyDocumentation, List<Type> typesInNamespaces)
        {
            this.assemblyDocumentation = assemblyDocumentation;
            this.typesInNamespaces = typesInNamespaces;
        }

        public void CreateDocumentation()
        {
            throw new NotImplementedException();
        }
    }
}