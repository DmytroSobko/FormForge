using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using FormForge.Core.Services.Internal;

namespace FormForge.Core.Services
{
    /// <summary>
    /// <para>Service locator class for defining and distributing services.</para>
    /// <para>
    /// Service definitions can be registered by calling <see cref="RegisterService{TService, TImplementation}(ServiceLifespan)"/>, 
    /// or by using the <see cref="ServiceDefinitionAttribute"/> attribute on a class and calling <see cref="CollectDefinitions"/> on the startup of your program.
    /// </para>
    /// <para>
    /// There are three types of service lifespans: <see cref="ServiceLifespan.Transient"/>, <see cref="ServiceLifespan.Singleton"/>, and <see cref="ServiceLifespan.LazySingleton"/>.
    /// </para>
    /// <para>
    /// You can register an existing instance of a service by calling <see cref="RegisterSingletonService{TService}(TService)"/>.
    /// </para>
    /// <para>
    /// Services can be retrieved by calling <see cref="GetService{TService}"/> or <see cref="GetService(Type)"/>,
    /// or by using the <see cref="InjectServiceAttribute"/> attribute on a property and calling <see cref="ResolveServiceInjection"/>.
    /// </para>  
    /// </summary>
    /// <remarks>
    /// <para>* When registering a service, the service type must be an interface and the implementation type must implement the service type.</para>
    /// <para>* When resolving service dependencies, the property must be an interface and have a private setter.</para>
    /// <para>* When accessing through <see cref="GetService{TService}"/> or <see cref="GetService(Type)"/>, the service type must be an interface.</para>
    /// </remarks>
    public static class ServiceLocator
    {
        internal static Dictionary<Type, ServiceBinding> ServiceBindings = new Dictionary<Type, ServiceBinding>();

        /// <summary>
        /// Register a service with its implementation
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="lifespan"></param>
        /// <exception cref="DuplicateServiceException"></exception>
        /// <exception cref="DuplicateImplementationException"></exception>
        /// <exception cref="ServiceBindingException"></exception>
        public static void RegisterService<TService, TImplementation>(ServiceLifespan lifespan = ServiceLifespan.Transient) 
            where TImplementation : TService
        {
            AddBinding(typeof(TService), typeof(TImplementation), lifespan);
        }

        /// <summary>
        /// Register a singleton service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DuplicateServiceException"></exception>
        /// <exception cref="DuplicateImplementationException"></exception>
        /// <exception cref="ServiceBindingException"></exception>
        public static void RegisterSingletonService<TService>(TService instance)
        {
            if (instance == null) 
            { 
                throw new ArgumentNullException(nameof(instance)); 
            }
            AddSingletonBinding(typeof(TService), instance);
        }

        /// <summary>
        /// <para>Returns a service of the given type if it is registered.</para>
        /// </summary>
        /// <remarks>
        /// Service type must be an interface.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="CircularDependencyException"></exception>
        /// <exception cref="ServiceLocatorException"></exception>
        public static T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        /// <summary>
        /// Resolve service dependencies for an object by locating Properties decorated with <see cref="InjectServiceAttribute"/> and setting their value.
        /// </summary>
        /// <param name="instance">The object for which the dependencies will be injected.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="CircularDependencyException"></exception>
        /// <exception cref="ServiceLocatorException"></exception>
        public static void ResolveServiceInjection(object instance)
        {
            if (instance == null) 
            {
                throw new ArgumentNullException(nameof(instance)); 
            }

            Type type = instance.GetType();
            IEnumerable<PropertyInfo> properties = ServiceBindings.TryGetValue(type, out ServiceBinding binding) 
                ? binding.InjectServiceProperties 
                : GetInjectServiceProperties(type);

            List<string> dependencyChain = new List<string>();
            if (HasCircularDependency(type, properties, ref dependencyChain))
            {
                throw new CircularDependencyException(type, dependencyChain);
            }

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(instance, GetService(property.PropertyType));
            }
        }

        private static void AddBinding(Type serviceType, Type implementationType, ServiceLifespan lifespan)
        {
            AddBinding(new ServiceBinding {
                ServiceType             = serviceType,
                ImplementationType      = implementationType,
                Lifespan                = lifespan,
                InjectServiceProperties = GetInjectServiceProperties(implementationType)
            });
        }

        private static void AddSingletonBinding(Type serviceType, object instance)
        {
            AddBinding(new ServiceBinding { 
                ServiceType             = serviceType, 
                ImplementationType      = instance.GetType(), 
                Lifespan                = ServiceLifespan.Singleton, 
                Instance                = instance,
                InjectServiceProperties = GetInjectServiceProperties(instance.GetType())
            });
        }

        /// <summary>
        /// <para>Returns a service of the given type if it is registered.</para>
        /// </summary>
        /// <remarks>
        /// Service type must be an interface.
        /// </remarks>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        /// <exception cref="CircularDependencyException"></exception>
        /// <exception cref="ServiceLocatorException"></exception>
        internal static object GetService(Type serviceType)
        {
            if (!serviceType.IsInterface)
            {
                throw new ServiceLocatorException($"Service {serviceType.Name} is not an interface. Services are accessed by their interfaces.");
            }
            if (!HasService(serviceType)) 
            {
                throw new ServiceLocatorException($"Service {serviceType.Name} is not registered.");
            }

            var binding = ServiceBindings[serviceType];

            if (binding.Instance != null && !binding.Instance.Equals(null))
            {
                return binding.Instance;
            }

            object instance = Activator.CreateInstance(binding.ImplementationType);

            if (binding.Lifespan != ServiceLifespan.Transient)
            {
                binding.Instance = instance;
            }

            ResolveServiceInjection(instance);

            return instance;
        }

        internal static void AddBinding(ServiceBinding binding)
        {
            if (!binding.ServiceType.IsInterface)
            {
                throw new ServiceLocatorException($"Service {binding.ServiceType.Name} is not an interface. Services must be interfaces.");
            }
            if (!binding.ServiceType.IsAssignableFrom(binding.ImplementationType))
            {
                throw new ServiceLocatorException($"Service {binding.ServiceType.Name} is not assignable from implementation {binding.ImplementationType.Name}");
            }
            if (binding.ImplementationType.IsInterface)
            {
                throw new ServiceLocatorException($"Implementation {binding.ImplementationType.Name} an invalid type {binding.ImplementationType.Name}. Implementations must be concrete classes.");
            }    
            if (HasService(binding.ServiceType))
            {
                throw new DuplicateServiceException(binding.ServiceType);
            }
            if (HasImplementationBinding(binding.ImplementationType))
            {
                throw new DuplicateImplementationException(binding.ImplementationType);
            } 
            ServiceBindings.Add(binding.ServiceType, binding);
        }

        internal static IEnumerable<PropertyInfo> GetInjectServiceProperties(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            List<PropertyInfo> properties = new List<PropertyInfo>();
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (property.GetCustomAttribute<InjectServiceAttribute>() == null) 
                { 
                    continue; 
                }
                if (!property.PropertyType.IsInterface)
                { 
                    throw new ServiceLocatorException($"Property {property.Name} in {type.Name} is not an interface. Cannot inject services into non-interface properties.");
                }
                if (property.GetSetMethod() != null)
                {
                    throw new ServiceLocatorException($"Property {property.Name} in {type.Name} decorated with InjectService cannot have a public setter.");
                }
                
                properties.Add(property);
            }
            return properties;
        }

        internal static bool HasCircularDependency(Type type, IEnumerable<PropertyInfo> properties, ref List<string> dependencyChain)
        {
            dependencyChain.Add(type.Name);

            foreach (PropertyInfo property in properties)
            {
                if (dependencyChain.Contains(property.PropertyType.Name))
                {
                    return true;
                }

                if (ServiceBindings.TryGetValue(property.PropertyType, out ServiceBinding binding))
                {
                    if (HasCircularDependency(binding.ServiceType, binding.InjectServiceProperties, ref dependencyChain))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Determine if a given service type is registered
        /// </summary>
        /// <remarks>
        /// This method will return `true` if there is an implementation available for the type `serviceType`, or false if not.
        /// </remarks>
        public static bool HasService(Type serviceType)
        {
            return ServiceBindings.ContainsKey(serviceType);
        }

        internal static bool HasImplementationBinding(Type implementationType)
        {
            return ServiceBindings.Values.Any(binding => binding.ImplementationType == implementationType);
        }
    }
}
