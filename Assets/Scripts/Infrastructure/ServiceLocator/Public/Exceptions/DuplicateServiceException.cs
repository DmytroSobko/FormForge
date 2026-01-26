using System;

namespace FormForge.Core.Services
{
    /// <summary>
    /// Represents an exception thrown when a duplicate service is registered.
    /// </summary>
    public class DuplicateServiceException : ServiceLocatorException
    {
        public DuplicateServiceException(Type serviceType) : 
            base($"Service of type {serviceType.FullName} already exists") { }
    }
}
