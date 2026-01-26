using System;

namespace FormForge.Core.Services
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