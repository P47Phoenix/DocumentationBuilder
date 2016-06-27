using Core.Ifx.Documentation.Models;

namespace Core.Ifx.Documentation.Models
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDocumentationOptions
    {
        /// <summary>
        /// Gets or sets the assembly documation path.
        /// </summary>
        /// <value>
        /// The assembly documation path.
        /// </value>
        string AssemblyDocumationPath { get; set; }
        /// <summary>
        /// Gets or sets the assembly path.
        /// </summary>
        /// <value>
        /// The assembly path.
        /// </value>
        string AssemblyPath { get; set; }
        /// <summary>
        /// Gets or sets the type of the documentation.
        /// </summary>
        /// <value>
        /// The type of the documentation.
        /// </value>
        DocumentationType DocumentationType { get; set; }
        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>
        /// The namespace.
        /// </value>
        string Namespace { get; set; }
        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>
        /// The output directory.
        /// </value>
        string OutputDirectory { get; set; }
    }
}