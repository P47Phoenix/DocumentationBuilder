using System.Reflection;

namespace Core.Ifx.Documentation.Services.Formators
{
    public class ParameterFormatorRequest : IFormatorRequest
    {
        public ParameterInfo Parameter { get; set; }
    }
}