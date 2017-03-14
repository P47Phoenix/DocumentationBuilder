using Core.Ifx.Documentation.Models;

namespace Core.Ifx.Documentation.Services
{
    public interface IDocumentationWriter<T>
    {
        void WriteDocumenation(T description, string m_outputDirectory);
    }
}