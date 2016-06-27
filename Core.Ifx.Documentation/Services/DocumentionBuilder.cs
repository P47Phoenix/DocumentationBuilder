using Core.Ifx.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Core.Ifx.Documentation
{
    public class DocumentionBuilder
    {

        private readonly IDocumentaionProcessorFactory m_documentaionProcessorFactory;

        public DocumentionBuilder(IDocumentaionProcessorFactory documentaionProcessorFactory)
        {
            m_documentaionProcessorFactory = documentaionProcessorFactory;
        }

        public void CreateDocumentation(IDocumentationOptions documentationOptions)
        {
            IDocumentationProcessor documentationProcessor = m_documentaionProcessorFactory.Create(documentationOptions);
            documentationProcessor.CreateDocumentation();        }

    }
}
