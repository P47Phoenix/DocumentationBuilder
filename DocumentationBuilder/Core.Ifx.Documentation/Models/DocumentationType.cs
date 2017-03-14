using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ifx.Documentation.Models
{
    /// <summary>
    /// This is an emum the represents the type of documentation we can run
    /// </summary>
    public enum DocumentationType : int
    {
        /// <summary>
        /// The contract
        /// </summary>
        Contract,

        /// <summary>
        /// The service
        /// </summary>
        Service
    }
}
