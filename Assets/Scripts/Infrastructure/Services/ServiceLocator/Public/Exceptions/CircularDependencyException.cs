using System;
using System.Collections.Generic;

namespace FormForge.Infrastructure.Services.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when a circular dependency is detected.
    /// </summary>
    public class CircularDependencyException : ServiceLocatorException
    {
        public CircularDependencyException(Type type, IEnumerable<string> path) : 
            base($"Circular dependency detected for type {type.FullName}. Path: {string.Join(" -> ", path)}") { }
    }
}