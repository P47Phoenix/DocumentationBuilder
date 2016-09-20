namespace Core.Ifx.Documentation.Services
{
    /// <summary>
    /// Example return type interface
    /// </summary>
    public interface IReturnExample
    {
        /// <summary>
        /// Determines if the request succeded
        /// </summary>
        bool Success { get; set; }
    }
}