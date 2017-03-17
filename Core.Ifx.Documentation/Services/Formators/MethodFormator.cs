using System;
using System.Linq;

namespace Core.Ifx.Documentation.Services.Formators
{
    public class MethodFormator : IFormator
    {
        private readonly IFormatorFactory m_formatorFactory;

        public MethodFormator(IFormatorFactory formatorFactory)
        {
            m_formatorFactory = formatorFactory;
        }

        public string Format(IFormatorRequest formatorRequest)
        {
            if ((formatorRequest is MethodFormatorRequest) == false)
            {
                throw new ArgumentException($"Expected {typeof(MethodFormatorRequest)}");
            }

            var methodFormatorRequest = (MethodFormatorRequest)formatorRequest;

            var returnParamName = methodFormatorRequest.MethodInfo.ReturnType.Name;
            
            IFormatorRequest paramFormatorRequest = new ListOfParameterFormatorRequest()
            {
                Parameters = methodFormatorRequest.MethodInfo.GetParameters().ToList()
            };

            var parameterFormator = m_formatorFactory.CreateFormator(paramFormatorRequest);

            var formatedParamaters = parameterFormator.Format(paramFormatorRequest);

            //var paramaters = string.Join(", ", methodFormatorRequest.MethodInfo.GetParameters().Select(GetParamNameValue));

            return string.Format("{0} {1}({2})", returnParamName, methodFormatorRequest.MethodInfo.Name, formatedParamaters);
        }
    }
}