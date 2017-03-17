using System;

namespace Core.Ifx.Documentation.Services.Questions
{
    public class ApiControllerDocumentTypeAsServiceQuestion : IDocumentTypeAsServiceQuestion
    {
        public bool ShouldDocumentType(Type type)
        {
            if (type == typeof(object))
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            if (type.BaseType.Name.Equals("ApiController", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return ShouldDocumentType(type.BaseType);
        }
    }
}