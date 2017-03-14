using System;

namespace Core.Ifx.Documentation.Models
{
    /// <summary>
    /// Documentation description of a poperty on a public contract
    /// </summary>
    /// <seealso cref="Core.Ifx.Documentation.Models.IDescription" />
    public class ContractProperty : IDescription
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name of the property
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The Type of the property
        /// </value>
        public Type DataType { get; set; }

        /// <summary>
        /// Gets or sets the desription.
        /// </summary>
        /// <value>
        /// The description of the property loaded from the documentation xml file
        /// </value>
        public string Desription { get; set; }
    }
}