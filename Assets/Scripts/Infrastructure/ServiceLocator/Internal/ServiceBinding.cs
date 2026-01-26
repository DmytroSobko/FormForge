using System;
using System.Collections.Generic;
using System.Reflection;

namespace FormForge.Core.Services.Internal
{
    /// <summary>
    /// Represents the binding between a service interface and its implementation.
    /// </summary>
    internal class ServiceBinding
    {
        public Type ServiceType { get; set; }
        public Type ImplementationType { get; set; }
        public ServiceLifespan Lifespan { get; set; }
        public object Instance { get; set; }
        public IEnumerable<PropertyInfo> InjectServiceProperties { get; set; }
    }
}
