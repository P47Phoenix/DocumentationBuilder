using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Core.Ifx.Documentation.Models;

namespace Core.Ifx.Documentation.Services
{
	public class ServiceTypeParser : ITypeParser<ServiceDescription>
	{
		public List<ServiceDescription> Parse(XDocument m_assemblyDocumentation, List<Type> m_typesInNamespaces)
		{
			List<ServiceDescription> serviceDescriptions = new List<ServiceDescription>();

			foreach (var typesInNamespace in m_typesInNamespaces)
			{
				if (!typesInNamespace.IsInterface)
					continue;

				if (!typesInNamespace.GetCustomAttributes().OfType<ServiceContractAttribute>().Any())
					continue;

				var xPathQueryForType = Helper.GetXPathQueryForType(typesInNamespace.FullName);

				var documenationForService = m_assemblyDocumentation.XPathSelectElement(xPathQueryForType);

				var serviceDescription = new ServiceDescription
				{
					Name = typesInNamespace.Name,
					Desription = documenationForService.Value
				};

				serviceDescriptions.Add(serviceDescription);

				foreach (var method in typesInNamespace.GetMethods())
				{
					if (method.IsStatic || !method.IsPublic)
						continue;

					var xPathQueryForMethod = Helper.GetXPathQueryForMethod(typesInNamespace.FullName, method.Name);

					var documentationForMethod = m_assemblyDocumentation.XPathSelectElement(xPathQueryForMethod);

					serviceDescription.ServiceMethods.Add(new ServiceMethod
					{
						Name = method.Name,
						Description = documentationForMethod.Value.Trim(' ', '\n'),
						Signature = GetMethodSignature(method)
					});
				}
			}
			return serviceDescriptions;
		}

		private string GetMethodSignature(MethodInfo method)
		{
			var returnParamName = method.ReturnType.Name;

			var paramaters = string.Join(", ", method.GetParameters().Select(GetParamNameValue));

			return string.Format("{0} {1}({2})", returnParamName, method.Name, paramaters);
		}

		private static string GetParamNameValue(ParameterInfo param)
		{
			StringBuilder sb = new StringBuilder();

			if (param.IsOut)
			{
				sb.Append("out ");
			}

			if (param.ParameterType.IsGenericType)
			{
				var genericArguments = param.ParameterType.GetGenericArguments();

				var genericArgumentsJoin = string.Join(", ", genericArguments.Select(genericType => genericType.Name));
				
				var typeName = param.ParameterType.Name.Replace($"`{genericArguments.Length}", "");

				sb.Append($"{typeName}<{genericArgumentsJoin}>");
			}
			else
			{
				sb.Append(param.ParameterType.Name.Replace("&", ""));
			}

			sb.Append(" ");

			sb.Append(param.Name);

			if (param.HasDefaultValue)
			{
				sb.Append($" = {param.DefaultValue}");
			}

			return sb.ToString();
		}
	}
}
