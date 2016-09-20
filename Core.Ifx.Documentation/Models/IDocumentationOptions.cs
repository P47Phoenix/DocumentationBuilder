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
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>
        /// The output directory.
        /// </value>
        string OutputDirectory { get; set; }
    }
}