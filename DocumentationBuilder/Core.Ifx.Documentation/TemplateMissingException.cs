using System;

namespace Core.Ifx.Documentation.Services
{
    public class TemplateMissingException : Exception
    {
        public TemplateMissingException(string message) : base(message)
        {
        }
    }
}