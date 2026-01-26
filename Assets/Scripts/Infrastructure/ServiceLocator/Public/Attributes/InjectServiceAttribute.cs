using System;

namespace FormForge.Core.Services
{
    /// <summary>
    /// Attribute that marks a property as a service that should be injected by the service locator
    /// </summary>
    /// <remarks>
    /// Properties with this attribute will be automatically injected by the service locator when <see cref="ServiceLocator.ResolveServiceInjection"/> is called
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class InjectServiceAttribute : Attribute
    {

    }
}
