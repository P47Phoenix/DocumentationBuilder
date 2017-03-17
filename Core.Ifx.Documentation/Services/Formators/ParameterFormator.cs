using System;
using System.Linq;
using System.Text;

namespace Core.Ifx.Documentation.Services.Formators
{
    public class ParameterFormator : IFormator
    {
        public string Format(IFormatorRequest formatorRequest)
        {
            if ((formatorRequest is ParameterFormatorRequest) == false)
            {
                throw new ArgumentException($"ParameterFormator exepects {typeof(ParameterFormatorRequest)}");
            }

            ParameterFormatorRequest parameterFormatorRequest = (ParameterFormatorRequest)formatorRequest;

            StringBuilder sb = new StringBuilder();

            var parameter = parameterFormatorRequest.Parameter;

            if (parameter.IsOut)
            {
                sb.Append("out ");
            }

            if (parameter.ParameterType.IsGenericType)
            {
                var genericArguments = parameter.ParameterType.GetGenericArguments();

                var genericArgumentsJoin = string.Join(", ", genericArguments.Select(genericType => genericType.Name));

                var typeName = parameter.ParameterType.Name.Replace($"`{genericArguments.Length}", "");

                sb.Append($"{typeName}<{genericArgumentsJoin}>");
            }
            else
            {
                sb.Append(parameter.ParameterType.Name.Replace("&", ""));
            }

            sb.Append(" ");

            sb.Append(parameter.Name);

            if (parameter.HasDefaultValue)
            {
                sb.Append($" = {parameter.DefaultValue}");
            }

            return sb.ToString();
        }
    }
}