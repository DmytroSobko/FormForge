namespace FormForge.Collections
{
    using System;
    using System.Reflection;
    using UnityEngine.Assertions;

    /// <summary>
    /// Default object generator for the Pool
    /// </summary>
    public class DefaultGenerator<T> : IGenerator
    {
        private AbstractPool m_owner;

        public DefaultGenerator(AbstractPool owner)
        {
            m_owner = owner;
        }

        public IPoolable CreateInstance()
        {
            if (m_owner.PoolType.IsAbstract)
            {
                throw new ArgumentException($"Cannot create an instance of Abstract type {m_owner.PoolType.Name}");
            }

            ConstructorInfo constructorInfo = m_owner.PoolType.GetConstructor(Type.EmptyTypes);
            Assert.IsNotNull(constructorInfo, $"Type {m_owner.PoolType.Name} must have a parameterless constructor in order to be created by the Default Generator");

            return (IPoolable)constructorInfo.Invoke(null);
        }
    }
}
