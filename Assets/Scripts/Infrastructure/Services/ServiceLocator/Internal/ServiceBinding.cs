using System;
using System.Reflection;
using System.Collections.Generic;
using FormForge.Infrastructure.Services.Enums;

namespace FormForge.Infrastructure.Services.Internal
{
    /// <summary>
    /// Represents the binding between a service interface and its implementation.
    /// </summary>
    public class ServiceBinding
    {
        public Type ServiceType { get; set; }
        public Type ImplementationType { get; set; }
        public ServiceLifespan Lifespan { get; set; }
        public object Instance { get; set; }
        public IEnumerable<PropertyInfo> InjectServiceProperties { get; set; }
    }
}
