namespace FormForge.Collections
{
    using UnityEngine;

    /// <summary>
    /// Unity specific generator that allows for hte creationg of new GameObjects based on a template
    /// </summary>
    public class UnityGenerator : IGenerator
    {
        private AbstractPool m_owner;
        private GameObject m_template;
        private Transform m_container;

        public UnityGenerator(AbstractPool owner, GameObject template, Transform container)
        {
            m_owner = owner;
            m_template = template;
            m_container = container;
        }

        public IPoolable CreateInstance()
        {
            GameObject obj = Object.Instantiate<GameObject>(m_template, m_container);
            obj.transform.localPosition = Vector3.zero;
            return obj.GetComponent<IPoolable>();
        }
    }
}
