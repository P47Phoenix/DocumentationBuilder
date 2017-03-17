using System.Reflection;

namespace Core.Ifx.Documentation.Services.Questions
{
    public class ControllerDocumentMethodQuestion : IDocumentMethodQuestion
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

            return method.ReflectedType == method.DeclaringType;
        }
    }
}