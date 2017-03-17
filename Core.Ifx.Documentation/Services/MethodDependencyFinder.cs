using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Ifx.Documentation.Services.Questions;

namespace Core.Ifx.Documentation.Services
{
    public class MethodDependencyFinder : IMethodDependencyFinder
    {
        private readonly List<IDocumentMethodDependencyQuestion> m_documentMethodDependencyQuestion;

        public MethodDependencyFinder(List<IDocumentMethodDependencyQuestion> documentMethodDependencyQuestion)
        {
            m_documentMethodDependencyQuestion = documentMethodDependencyQuestion;
        }

        public IEnumerable<Type> FindDependencies(MethodInfo method, List<Type> typesInAssembly)
        {
            List<Type> typesToAnalyize = new List<Type>();
            typesToAnalyize.Add(method.ReturnType);

            foreach (var parameterInfo in method.GetParameters())
            {
                typesToAnalyize.Add(parameterInfo.ParameterType);
            }

            foreach (var type in typesInAssembly)
            {
                if (m_documentMethodDependencyQuestion.Any(question => question.ShouldDocument(type, typesToAnalyize)))
                {
                    yield return type;
                }

            }
        }

        private IEnumerable<Type> FindDependenciesRecursively(List<Type> serviceDescriptionTypesServiceDependsOn, List<Type> typesInAssembly)
        {
            foreach (var type in typesInAssembly)
            {
                foreach (var serviceDependantType in serviceDescriptionTypesServiceDependsOn)
                {
                    if (ShouldIncludeType(type, serviceDependantType))
                    {
                        yield return type;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serviceDependantType"></param>
        /// <returns>
        /// 
        /// </returns>
        private bool ShouldIncludeType(Type type, Type serviceDependantType, int currentRecursion = 0)
        {
            if (currentRecursion++ > 100)
            {
                throw new Exception("Max recursion exceed the max of a hundred. Contact you architect for help.");
            }
            if (serviceDependantType.IsClass == false)
            {
                return false;
            }

            if (serviceDependantType.DeclaringType != type)
            {
                return false;
            }

            if (type.BaseType == serviceDependantType)
            {
                return true;
            }

            if (type.GetInterfaces().Contains(serviceDependantType))
            {
                return true;
            }

            var propTypes = serviceDependantType.GetProperties().Select(info => info.PropertyType).ToList();

            foreach (var propType in propTypes)
            {
                if (propType.IsValueType)
                {
                    continue;
                }

                if (propType == type)
                {
                    return true;
                }

                if (ShouldIncludeType(type, propType, currentRecursion))
                {
                    return true;
                }
            }

            if (serviceDependantType.IsGenericType)
            {
                var genericArgs = serviceDependantType.GetGenericArguments();

                foreach (var genericArg in genericArgs)
                {
                    if (ShouldIncludeType(type, genericArg, currentRecursion))
                    {
                        return true;
                    }
                }

            }

            if (type.BaseType == null || type.BaseType == typeof(object))
            {
                return false;
            }

            return ShouldIncludeType(type.BaseType, serviceDependantType, currentRecursion);
        }
    }
}