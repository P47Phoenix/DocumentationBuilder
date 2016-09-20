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
                }


            }
            return serviceDescriptions;
        }

        private string GetMethodSignature(MethodInfo method)
        {
            var returnParamName = method.ReturnType.Name;

            var paramaters = string.Join(", ", method.GetParameters().Select(param => param.ParameterType.Name + " " + param.Name));

            return string.Format("{0} {1}({2})", returnParamName, method.Name, paramaters);
        }
    }
}
