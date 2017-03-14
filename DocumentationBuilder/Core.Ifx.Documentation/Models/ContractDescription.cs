using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ifx.Documentation.Models
{
    /// <summary>
    /// This cotnract represents a contract we want to document in confluence
    /// </summary>
    /// <seealso cref="Core.Ifx.Documentation.Models.IDescription" />
    [DataContract]
    public class ContractDescription : IDescription
    {
        public ContractDescription()
        {
            ContractProperties = new List<ContractProperty>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name of the cotnract.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the contract properties.
        /// </summary>
        /// <value>
        /// The contract properties.
        /// </value>
        [DataMember]
        public List<ContractProperty> ContractProperties { get; set; }

        /// <summary>
        /// Gets or sets the desription.
        /// </summary>
        /// <value>
        /// The desription of the contract from the summary comments.
        /// </value>
        public string Desription { get; set; }
    }
}
