using System;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ifx.Documentation.Services
{
    /// <summary>
    /// The worst service ever
    /// </summary>
    [ServiceContract]
    public interface ITestService
    {
        /// <summary>
        /// Example service method
        /// </summary>
        /// <param name="exampleArgs">Some args</param>
        /// <returns>return example</returns>
        [OperationContract]
        IReturnExample ExampleRun(ExampleArgs exampleArgs);
    }
}
