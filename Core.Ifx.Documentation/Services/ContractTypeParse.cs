using Core.Ifx.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Core.Ifx.Documentation.Services
{
    public class ContractTypeParser : ITypeParser<ContractDescription>
    {
        public List<ContractDescription> Parse(List<Type> typesInAssembly, XDocument m_assemblyDocumentation = null)
        {
            var contractDescriptions = new List<ContractDescription>();

            foreach (var typeInNamespace in typesInAssembly)
            {
                string xPathQueryforContract = Helper.GetXPathQueryForType(typeInNamespace.FullName);

                var documentationForContract = m_assemblyDocumentation?.XPathSelectElement(xPathQueryforContract);

                string description = documentationForContract?.Value ?? string.Empty;

                var contractDescription = new ContractDescription
                {
                    Name = typeInNamespace.Name,
                    Desription = description
                };

                var properties = typeInNamespace.GetProperties();

                foreach (var property in properties)
                {
                    var xpathQueryForProperty = Helper.GetXPathQueryForProperty(property?.DeclaringType?.FullName, property?.Name);

                    var documentationForProperty = m_assemblyDocumentation?.XPathSelectElement(xpathQueryForProperty);

                    var contractProperty = new ContractProperty
                    {
                        DataType = property?.PropertyType,
                        Name = property?.Name,
                        Desription = documentationForProperty?.Value ?? string.Empty
                    };

                    contractDescription.ContractProperties.Add(contractProperty);
                }

                contractDescriptions.Add(contractDescription);
            }

            return contractDescriptions;
        }
    }
}
