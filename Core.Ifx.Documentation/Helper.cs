using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ifx.Documentation
{
    public static class Helper
    {
        private const string c_XpathTemplateForType = @"//member[starts-with(@name, 'T:{0}')]";
        private const string c_XpathTemplateForProperty = @"//member[starts-with(@name, 'P:{0}.{1}')]/value";
        private const string c_XpathTemplateForMethod = @"//member[starts-with(@name, 'M:{0}.{1}')]/summary";

        internal static string GetXPathQueryForType(string fullName)
        {
            return string.Format(c_XpathTemplateForType, fullName);
        }

        internal static string GetXPathQueryForProperty(string fullName, string name)
        {
            return string.Format(c_XpathTemplateForProperty, fullName, name);
        }

        public static string GetXPathQueryForMethod(string fullName, string name)
        {
            return string.Format(c_XpathTemplateForMethod, fullName, name);
        }
    }
}
