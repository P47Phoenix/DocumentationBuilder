using System.Reflection;
using System.ServiceModel;

namespace Core.Ifx.Documentation.Services.Questions
{
    public class OperationContractDocumentMethodQuestion : IDocumentMethodQuestion
    {
        public bool ShouldDocumentMethod(MethodInfo method)
        {
            if (method.IsStatic)
            {
                return false;
            }

            if (method.IsPublic == false)
            {
                return false;
            }

            return method.GetCustomAttribute<OperationContractAttribute>() != null;
        }
    }
}