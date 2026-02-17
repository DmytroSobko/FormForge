using System;

namespace FormForge.Infrastructure.Services.Exceptions
{
    /// <summary>
    /// Represents a generic exception thrown when a service locator fails.
    /// </summary>
    public class ServiceLocatorException : Exception
    {
        public ServiceLocatorException(string message) : 
            base(message) { }
    }
}