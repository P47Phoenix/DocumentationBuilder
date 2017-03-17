using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Ifx.Documentation.Models;

namespace Core.Ifx.Documentation.Services
{
    public interface IMethodDependencyFinder
    {
        IEnumerable<Type> FindDependencies(MethodInfo method, List<Type> typesInAssembly);
    }
}