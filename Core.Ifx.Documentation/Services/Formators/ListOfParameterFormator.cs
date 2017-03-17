using System;
using System.Linq;

namespace Core.Ifx.Documentation.Services.Formators
{
    public class ListOfParameterFormator : IFormator
    {
        private readonly IFormatorFactory m_formatorFactory;

        public ListOfParameterFormator(IFormatorFactory formatorFactory)
        {
            m_formatorFactory = formatorFactory;
        }

        public string Format(IFormatorRequest formatorRequest)
        {
            if ((formatorRequest is ListOfParameterFormatorRequest) == false)
            {
                throw new ArgumentException($"Parameter formator only takes a {typeof(ListOfParameterFormatorRequest).Name}");
            }

            ListOfParameterFormatorRequest listOfParameterFormatorRequest = (ListOfParameterFormatorRequest)formatorRequest;

            var formatedParamaters = listOfParameterFormatorRequest.Parameters.Select(param =>
            {
                IFormatorRequest parameterFormatorRequest = new ParameterFormatorRequest
                {
                    Parameter = param
                };

                IFormator formator = m_formatorFactory.CreateFormator(parameterFormatorRequest);

                return formator.Format(parameterFormatorRequest);

            });

            return string.Join(", ", formatedParamaters);
        }
    }
}