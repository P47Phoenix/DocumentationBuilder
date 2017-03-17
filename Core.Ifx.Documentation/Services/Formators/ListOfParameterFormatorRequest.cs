using System.Collections.Generic;
using System.Reflection;

namespace Core.Ifx.Documentation.Services.Formators
{
    /// <summary>
    /// 
    /// </summary>
    public class ListOfParameterFormatorRequest : IFormatorRequest
    {
        public List<ParameterInfo> Parameters { get; set; }
    }
}