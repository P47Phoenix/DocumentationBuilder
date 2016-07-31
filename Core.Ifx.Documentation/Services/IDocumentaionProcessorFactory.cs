using Core.Ifx.Documentation.Models;

namespace Core.Ifx.Documentation.Services
{
    public interface IDocumentaionProcessorFactory
    {
        IDocumentationProcessor Create(IDocumentationOptions documentationOptions);
    }

    
}