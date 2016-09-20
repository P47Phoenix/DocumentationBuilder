using System;
using System.Collections.Generic;
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
        /// Try parse int
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [OperationContract]
        IReturnExample ExampleRun(ExampleArgs exampleArgs);
    }

    public interface IReturnExample
    {
        bool Success { get; set; }
    }

    public class ConcreteReturn : IReturnExample
    {
        public bool Success { get; set; }
    }

    public class ExampleArgs
    {
        public List<IExmapleArg> ExmapleArgs { get; set; }
    }

    public class ExampleArgString : IExmapleArg
    {
        public string StringArg { get; set; }
    }

    public interface IExmapleArg
    {
    }
}
