using System;
using System.Reflection;
using System.ServiceModel;

namespace Core.Ifx.Documentation.Services.Questions
{
    public class ServiceContractDocumentTypeAsServiceQuestion : IDocumentTypeAsServiceQuestion
    {
        public bool ShouldDocumentType(Type type)
        {
            if (type.BaseType == null)
            {
                return type.GetCustomAttribute<ServiceContractAttribute>() != null;
            }

            return false;
        }
    }
}