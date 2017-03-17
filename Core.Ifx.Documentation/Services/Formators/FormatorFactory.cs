using System;

namespace Core.Ifx.Documentation.Services.Formators
{
    public class FormatorFactory : IFormatorFactory
    {
        public IFormator CreateFormator(IFormatorRequest formatorRequest)
        {
            if (formatorRequest is MethodFormatorRequest)
            {
                return new MethodFormator(this);
            }

            if (formatorRequest is ParameterFormatorRequest)
            {
                return new ParameterFormator();
            }

            if (formatorRequest is ListOfParameterFormatorRequest)
            {
                return new ListOfParameterFormator(this);
            }

            throw new NotSupportedException($"FormatorFactory does not support {formatorRequest.GetType().Name}");
        }
    }
}