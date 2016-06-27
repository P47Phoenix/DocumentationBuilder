namespace Core.Ifx.Documentation.Models
{
    /// <summary>
    /// Core description interface
    /// </summary>
    public interface IDescription
    {
        /// <summary>
        /// Gets or sets the desription.
        /// </summary>
        /// <value>
        /// The desription.
        /// </value>
        string Desription { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }
    }
}