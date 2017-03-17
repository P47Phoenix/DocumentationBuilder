using System.Reflection;

namespace Core.Ifx.Documentation.Services.Questions
{
    public interface IDocumentMethodQuestion
    {
        bool ShouldDocumentMethod(MethodInfo method);
    }
}