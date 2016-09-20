namespace Core.Ifx.Documentation.Services
{
    /// <summary>
    /// A concrete example of a return contract
    /// </summary>
    public class ConcreteReturn : IReturnExample
    {
        /// <summary>
        /// A <see langword="bool"/> value indicate the state of the request
        /// </summary>
        public bool Success { get; set; }
    }
}