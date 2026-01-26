using System;

namespace FormForge.Utils
{
    /// <summary>
    /// A wrapper of the native <see cref="WeakReference"/> class to allow for us to use it with HashSet. 
    /// </summary>
    /// <remarks>
    /// This should not be used as an example of how to override GetHashCode or Equals. This method of doing it
    /// is highly specific to this use-case.
    /// </remarks>
    internal class WeakReferenceWrapper : IEquatable<WeakReferenceWrapper>
    {
        private readonly WeakReference m_weakReference;
        private readonly int m_hashCode;

        public bool IsAlive => m_weakReference.IsAlive;

        public WeakReferenceWrapper(Object target)
        {
            m_weakReference = new WeakReference(target);
            m_hashCode = target != null ? target.GetHashCode() : 0;
        }

        public bool TryGetTarget(out Object target)
        {
            if (m_weakReference.IsAlive)
            {
                target = m_weakReference.Target;
                return true;
            }
        
            target = null;
            return false;
        }

        public bool TryGetTarget<TTarget>(out TTarget target) where TTarget : class
        {
            if (m_weakReference.IsAlive)
            {
                target = m_weakReference.Target as TTarget;
                return true;
            }

            target = null;
            return false;
        }

        public override int GetHashCode()
        {
            return m_hashCode;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WeakReferenceWrapper);
        }

        public bool Equals(WeakReferenceWrapper other)
        {
            if (other == null)
            {
                return false;
            }

            return m_hashCode == other.m_hashCode;
        }
    }
}