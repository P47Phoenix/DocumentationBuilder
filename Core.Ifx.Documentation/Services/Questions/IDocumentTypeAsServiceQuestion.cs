using System;

namespace Core.Ifx.Documentation.Services.Questions
{
    public interface IDocumentTypeAsServiceQuestion
    {
        bool ShouldDocumentType(Type type);
    }
}