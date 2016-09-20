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
        public List<ContractDescription> Parse(XDocument m_assemblyDocumentation, List<Type> m_typesInNamespaces)
        {
            var contractDescriptions = new List<ContractDescription>();

            foreach (var typeInNamespace in m_typesInNamespaces)
            {
                string xPathQueryforContract = Helper.GetXPathQueryForType(typeInNamespace.FullName);

                var documenationForContract = m_assemblyDocumentation.XPathSelectElement(xPathQueryforContract);

                string description = "";

                if (documenationForContract != null)
                    description = documenationForContract.Value;

                var contractDesciption = new ContractDescription
                {
                    Name = typeInNamespace.Name,
                    Desription = description
                };

                var properties = typeInNamespace.GetProperties();

                foreach (var property in properties)
                {
                    var xpathQueryForProperty = Helper.GetXPathQueryForProperty(property.DeclaringType.FullName, property.Name);

                    var documenationForProperty = m_assemblyDocumentation.XPathSelectElement(xpathQueryForProperty);

                    var contractProperty = new ContractProperty
                    {
                        DataType = property.PropertyType,
                        Name = property.Name,
                        Desription = documenationForProperty?.Value ?? string.Empty
                    };

                    contractDesciption.ContractProperties.Add(contractProperty);
                }

                contractDescriptions.Add(contractDesciption);
            }

            return contractDescriptions;
        }
    }
}
