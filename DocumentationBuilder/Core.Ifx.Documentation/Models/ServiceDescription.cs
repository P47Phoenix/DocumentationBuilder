using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Ifx.Documentation.Services;

namespace Core.Ifx.Documentation.Models
{
    /// <summary>
    /// This represents a service that we want to document in confluence.
    /// </summary>
    /// <seealso cref="Core.Ifx.Documentation.Models.IDescription" />
    public class ServiceDescription : IDescription
    {
        public ServiceDescription()
        {
            ServiceMethods = new List<ServiceMethod>();
        }

        public List<ServiceMethod> ServiceMethods { get; set; }

        /// <summary>
        /// Gets or sets the desription.
        /// </summary>
        /// <value>
        /// The desription of the service.
        /// </value>
        public string Desription { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name of hte service.
        /// </value>
        public string Name { get; set; }

        public string Sample { get; set; }
        public string Diagrams { get; set; }
        public List<Type> TypesServiceDependsOn { get; set; }
    }
}
