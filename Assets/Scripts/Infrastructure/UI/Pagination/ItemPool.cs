using System.Collections.Generic;
using UnityEngine;

namespace FormForge.Infrastructure.UI.Pagination
{
    public class ItemPool<T> where T : MonoBehaviour
    {
        private readonly T m_Prefab;
        private readonly Transform m_Parent;

        private readonly Stack<T> m_Available = new Stack<T>();
        private readonly List<T> m_Active = new List<T>();

        public ItemPool(T mPrefab, Transform mParent)
        {
            m_Prefab = mPrefab;
            m_Parent = mParent;
        }

        public T Get()
        {
            T item = m_Available.Count > 0 ? 
                m_Available.Pop() : Object.Instantiate(m_Prefab, m_Parent);

            item.gameObject.SetActive(true);
            m_Active.Add(item);

            return item;
        }

        public void ReleaseAll()
        {
            foreach (var item in m_Active)
            {
                item.gameObject.SetActive(false);
                m_Available.Push(item);
            }

            m_Active.Clear();
        }
    }
}