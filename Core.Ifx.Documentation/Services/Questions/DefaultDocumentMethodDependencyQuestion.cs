using System;
using System.Collections.Generic;

namespace Core.Ifx.Documentation.Services.Questions
{
    public class DefaultDocumentMethodDependencyQuestion : IDocumentMethodDependencyQuestion
    {
        public bool ShouldDocument(Type type, List<Type> typesToAnalyize)
        {
            return typesToAnalyize.Contains(type);
        }
    }
}