using System;
using System.Collections.Generic;

namespace Core.Ifx.Documentation.Services.Questions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDocumentMethodDependencyQuestion
    {
        bool ShouldDocument(Type type, List<Type> typesToAnalyize);
    }
}