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
        bool TryParse(string s, out int value);

		/// <summary>
		/// Add operation
		/// </summary>
		/// <param name="value"></param>
		/// <param name="addValue"></param>
		/// <returns></returns>
		[OperationContract]
	    int Add(int value, int addValue);

		/// <summary>
		/// Iterate across threads
		/// </summary>
		/// <param name="number"></param>
		/// <param name="threads"></param>
		[OperationContract]
		void Iterate(List<int> number, int threads = 1);
    }
}
