using Core.Ifx.Documentation.Models;

namespace Core.Ifx.Documentation.Services
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
