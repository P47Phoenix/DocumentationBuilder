using System;

namespace Core.Ifx.Documentation
{
    public class TemplateMissingException : Exception
    {
        public TemplateMissingException(string message) : base(message)
        {
        }
    }
}