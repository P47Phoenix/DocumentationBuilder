using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Core.Example.Service
{
    /// <summary>
    /// Example Controller documentation
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    /// <seealso cref="Core.Example.Service.ITestService" />
    public class TemplateController : Controller, ITestService
    {
        /// <summary>
        /// Try parse int
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool TryParse(string s, out int value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add operation
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Add(int value, int addValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Iterate across threads
        /// </summary>
        /// <param name="number"></param>
        /// <param name="threads"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Iterate(List<int> number, int threads = 1)
        {
            throw new NotImplementedException();
        }
    }
}
