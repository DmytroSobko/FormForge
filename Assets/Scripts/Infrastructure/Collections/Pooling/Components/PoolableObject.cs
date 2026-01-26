namespace FormForge.Collections
{
    using System;
    using UnityEngine;
    
    public sealed class PoolableObject : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private bool m_disableOnDeallocation = false;
        [SerializeField]
        private bool m_disableCollidersOnDeallocation = true;
        [SerializeField]
        private bool m_disableRenderersOnDeallocation = false;

        /// <summary>
        /// Called whenever the object is retrieved from the pool for use.
        /// </summary>
        public Action ObjectAllocated;

        /// <summary>
        /// Called whenever the object is Recycled
        /// </summary>
        public Action ObjectDeallocated;

        private AbstractPool m_owner;
        private Collider[] m_colliders;
        private Collider2D[] m_2dColliders;
        private Renderer[] m_renderers;

        private bool EnableOnAllocation => !m_disableOnDeallocation;
        private bool EnableCollidersOnAllocation => !m_disableCollidersOnDeallocation;
        private bool EnableRenderersOnAllocation => !m_disableRenderersOnDeallocation;

        void IPoolable.Init(AbstractPool owner)
        {
            m_owner = owner;

            m_colliders = GetComponentsInChildren<Collider>();
            m_2dColliders = GetComponentsInChildren<Collider2D>();
            m_renderers = GetComponentsInChildren<Renderer>();
        }

        void IPoolable.OnAllocate()
        {
            if(EnableOnAllocation)
            {
                gameObject.SetActive(true);
            }
            else
            {
                if(EnableCollidersOnAllocation)
                {
                    EnableColliders();
                }

                if(EnableRenderersOnAllocation)
                {
                    EnableRenderers();
                }
            }

            if(ObjectAllocated != null)
            {
                ObjectAllocated();
            }
        }

        void IPoolable.OnDeallocate()
        {
            if (m_disableOnDeallocation)
            {
                gameObject.SetActive(false);
            }
            else
            {
                if (m_disableCollidersOnDeallocation)
                {
                    DisableColliders();
                }

                if (m_disableRenderersOnDeallocation)
                {
                    DisableRenderers();
                }
            }

            if (ObjectDeallocated != null)
            {
                ObjectDeallocated();
            }
        }

        void IPoolable.Destroy()
        {
            m_owner = null;
            m_colliders = null;
            m_2dColliders = null;
            m_renderers = null;

            ObjectAllocated = null;
            ObjectDeallocated = null;
        }

        /// <summary>
        /// Recycles this object calling OnDeallocate and returning it to the Pool
        /// </summary>
        public void Recycle()
        {
            if (m_owner.IsAlive)
            {
                m_owner.Recycle(this);
            }
        }

        /// <summary>
        /// Enables all colliders for this object and its children
        /// </summary>
        private void EnableColliders()
        {
            for (int i = 0, iCount = m_colliders.Length; i < iCount; ++i)
            {
                m_colliders[i].enabled = true;
            }

            for (int j = 0, jCount = m_2dColliders.Length; j < jCount; ++j)
            {
                m_2dColliders[j].enabled = true;
            }
        }

        /// <summary>
        /// Disables all colliders for this object and its children
        /// </summary>
        private void DisableColliders()
        {
            for (int i = 0, iCount = m_colliders.Length; i < iCount; ++i)
            {
                m_colliders[i].enabled = false;
            }

            for (int j = 0, jCount = m_2dColliders.Length; j < jCount; ++j)
            {
                m_2dColliders[j].enabled = false;
            }
        }

        /// <summary>
        /// Enables all renderers for this object and its children
        /// </summary>
        private void EnableRenderers()
        {
            for (int i = 0, count = m_renderers.Length; i < count; ++i)
            {
                m_renderers[i].enabled = true;
            }
        }

        /// <summary>
        /// Disables all renderers for this object and its children
        /// </summary>
        private void DisableRenderers()
        {
            for (int i = 0, count = m_renderers.Length; i < count; ++i)
            {
                m_renderers[i].enabled = false;
            }
        }
    }
}
