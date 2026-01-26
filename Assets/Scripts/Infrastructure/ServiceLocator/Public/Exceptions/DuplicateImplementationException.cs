using System;

namespace FormForge.Core.Services
{
    /// <summary>
    /// Represents an exception thrown when a duplicate implementation is registered.
    /// </summary>
    public class DuplicateImplementationException : ServiceLocatorException
    {
        public DuplicateImplementationException(Type serviceType) :
            base($"Implementation of service {serviceType.FullName} already exists") { }            
    }
}