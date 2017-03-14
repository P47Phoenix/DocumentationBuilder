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
        public List<ServiceDescription> Parse(XDocument m_assemblyDocumentation, List<Type> typesInAssembly)
        {
            List<ServiceDescription> serviceDescriptions = new List<ServiceDescription>();

            foreach (var typesInNamespace in typesInAssembly)
            {
                if (!typesInNamespace.IsInterface)
                {
                    continue;
                }

                if (!typesInNamespace.GetCustomAttributes().OfType<ServiceContractAttribute>().Any())
                {
                    continue;
                }

                var xPathQueryForType = Helper.GetXPathQueryForType(typesInNamespace.FullName);

                var documenationForService = m_assemblyDocumentation.XPathSelectElement(xPathQueryForType);

                var serviceDescription = new ServiceDescription
                {
                    Name = typesInNamespace.Name,
                    Desription = documenationForService.Value
                };

                serviceDescriptions.Add(serviceDescription);

                serviceDescription.TypesServiceDependsOn = new List<Type>();

                foreach (var method in typesInNamespace.GetMethods())
                {
                    if (method.IsStatic || !method.IsPublic)
                    {
                        continue;
                    }

                    var xPathQueryForMethod = Helper.GetXPathQueryForMethod(typesInNamespace.FullName, method.Name);

                    var documentationForMethod = m_assemblyDocumentation.XPathSelectElement(xPathQueryForMethod);

                    serviceDescription.TypesServiceDependsOn.Add(method.ReturnType);

                    var parameters = method.GetParameters();

                    foreach (var parameter in parameters)
                    {
                        serviceDescription.TypesServiceDependsOn.Add(parameter.ParameterType);
                    }

                    serviceDescription.ServiceMethods.Add(new ServiceMethod
                    {
                        Name = method.Name,
                        Description = documentationForMethod.Value.Trim(' ', '\n'),
                        Signature = GetMethodSignature(method)
                    });

                    this.FindDependencies(serviceDescription, typesInAssembly);
                }


            }
            return serviceDescriptions;
        }

        private void FindDependencies(ServiceDescription serviceDescription, List<Type> typesInAssembly)
        {
            var newTypes = FindDependenciesRecursively(serviceDescription.TypesServiceDependsOn, typesInAssembly).Concat(serviceDescription.TypesServiceDependsOn);

            serviceDescription.TypesServiceDependsOn = newTypes.Distinct().ToList();
        }

        private IEnumerable<Type> FindDependenciesRecursively(List<Type> serviceDescriptionTypesServiceDependsOn, List<Type> typesInAssembly)
        {
            foreach (var type in typesInAssembly)
            {
                foreach (var serviceDependantType in serviceDescriptionTypesServiceDependsOn)
                {
                    if (ShouldIncludeType(type, serviceDependantType))
                    {
                        yield return type;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serviceDependantType"></param>
        /// <returns>
        /// 
        /// </returns>
        private bool ShouldIncludeType(Type type, Type serviceDependantType, int currentRecursion = 0)
        {
            if (currentRecursion++ > 100)
            {
                throw new Exception("Max recursion exceed the max of a hundred. Contact you architect for help.");
            }
            if (type.BaseType == serviceDependantType)
            {
                return true;
            }

            if (type.GetInterfaces().Contains(serviceDependantType))
            {
                return true;
            }

            var propTypes = serviceDependantType.GetProperties().Select(info => info.PropertyType).ToList();

            foreach (var propType in propTypes)
            {
                if (propType.IsValueType)
                {
                    continue;
                }

                if (propType == type)
                {
                    return true;
                }

                if (ShouldIncludeType(type, propType, currentRecursion))
                {
                    return true;
                }
            }

            if (serviceDependantType.IsGenericType)
            {
                var genericArgs = serviceDependantType.GetGenericArguments();

                foreach (var genericArg in genericArgs)
                {
                    if (ShouldIncludeType(type, genericArg, currentRecursion))
                    {
                        return true;
                    }
                }

            }

            if (type.BaseType == null || type.BaseType == typeof(object))
            {
                return false;
            }

            return ShouldIncludeType(type.BaseType, serviceDependantType, currentRecursion);
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
