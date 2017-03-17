using System.Reflection;

namespace Core.Ifx.Documentation.Services.Formators
{
    public class MethodFormatorRequest : IFormatorRequest
    {
        public MethodInfo MethodInfo { get; set; }
    }
}