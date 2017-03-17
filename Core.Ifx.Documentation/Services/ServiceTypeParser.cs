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
using Core.Ifx.Documentation.Services.Formators;
using Core.Ifx.Documentation.Services.Questions;

namespace Core.Ifx.Documentation.Services
{
    public class ServiceTypeParser : ITypeParser<ServiceDescription>
    {
        private readonly List<IDocumentTypeAsServiceQuestion> m_documentTypeQuestions;
        private readonly List<IDocumentMethodQuestion> m_documentMethodQuestions;
        private readonly IMethodDependencyFinder m_methodDependencyfinder;
        private readonly IFormatorFactory m_formatorFactory;

        public ServiceTypeParser(
            List<IDocumentTypeAsServiceQuestion> documentTypeQuestions,
            List<IDocumentMethodQuestion> documentMethodQuestions,
            IMethodDependencyFinder methodDependencyfinder,
            IFormatorFactory formatorFactory)
        {
            m_documentTypeQuestions = documentTypeQuestions;
            m_documentMethodQuestions = documentMethodQuestions;
            m_methodDependencyfinder = methodDependencyfinder;
            m_formatorFactory = formatorFactory;
        }

        public List<ServiceDescription> Parse(List<Type> typesInAssembly, XDocument m_assemblyDocumentation = null)
        {
            var serviceDescriptions = new List<ServiceDescription>();

            foreach (var type in typesInAssembly)
            {
                if (m_documentTypeQuestions.Any(question => question.ShouldDocumentType(type)) == false)
                {
                    continue;
                }

                var xPathQueryForType = Helper.GetXPathQueryForType(type.FullName);

                var documentationForService = m_assemblyDocumentation?.XPathSelectElement(xPathQueryForType);

                var serviceDescription = new ServiceDescription
                {
                    Name = type.Name,
                    Desription = documentationForService?.Value
                };

                serviceDescriptions.Add(serviceDescription);

                foreach (var method in type.GetMethods())
                {

                    if (m_documentMethodQuestions.Any(question => question.ShouldDocumentMethod(method)) == false)
                    {
                        continue;
                    }

                    IFormatorRequest formatorRequest = new MethodFormatorRequest()
                    {
                        MethodInfo = method
                    };

                    IFormator formator = m_formatorFactory.CreateFormator(formatorRequest);

                    var xPathQueryForMethod = Helper.GetXPathQueryForMethod(type.FullName, method.Name);

                    var documentationForMethod = m_assemblyDocumentation?.XPathSelectElement(xPathQueryForMethod);

                    serviceDescription.ServiceMethods.Add(new ServiceMethod
                    {
                        Name = method.Name,
                        Description = documentationForMethod?.Value.Trim(' ', '\n'),
                        Signature = formator.Format(formatorRequest),  //GetMethodSignature(method)
                    });

                    serviceDescription.TypesServiceDependsOn = m_methodDependencyfinder.FindDependencies(method, typesInAssembly).ToList();
                }
            }
            return serviceDescriptions;
        }

        
    }
}
