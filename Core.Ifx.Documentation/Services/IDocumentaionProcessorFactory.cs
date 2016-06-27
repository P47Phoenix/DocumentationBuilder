using Core.Ifx.Documentation.Models;

namespace Core.Ifx.Documentation
{
    public interface IDocumentaionProcessorFactory
    {
        IDocumentationProcessor Create(IDocumentationOptions documentationOptions);
    }

    
}