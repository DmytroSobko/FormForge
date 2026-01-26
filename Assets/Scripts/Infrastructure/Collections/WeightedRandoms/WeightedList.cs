using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Prime.Collections
{
    /// <summary>
    /// A Generic list that has a weighted value for each entry.
    /// </summary>
    /// <typeparam name="T">Type for the list to use.</typeparam>
    [Serializable]
    public class WeightedList<T>
    {
        /// <summary>
        /// Object to hold the each weighted entry.
        /// </summary>
        [Serializable]
        internal class Entry
        {
            public T Item;
            public float Weight;
        }

        [SerializeField] private List<Entry> m_Entries = new List<Entry>();
        [SerializeField] private float m_AccumulatedWeight;

        private Random m_Random = new Random();

        /// <summary>
        /// Adds an item to the weighted list.
        /// </summary>
        /// <param name="item">Item to add to the weighted list.</param>
        /// <param name="weight">Weight of the added item.</param>
        public void Add(T item, float weight)
        {
            m_AccumulatedWeight += weight;
            m_Entries.Add(new Entry { Item = item, Weight = weight });
        }

        /// <summary>
        /// Remove an item from the weighted list. Recalculates the weights after removing.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public void Remove(T item)
        {
            m_Entries.RemoveAll(entry => EqualityComparer<T>.Default.Equals(entry.Item, item));
            RecalculateWeights();
        }

        /// <summary>
        /// Modify the weight of an existing item.
        /// </summary>
        /// <param name="item">Item to modify.</param>
        /// <param name="modifier">Value to multiply the current weight by.</param>
        public void ModifyWeight(T item, float modifier)
        {
            foreach (var entry in m_Entries)
            {
                if (EqualityComparer<T>.Default.Equals(entry.Item, item))
                {
                    entry.Weight *= modifier;
                    break;
                }
            }
            RecalculateWeights();
        }

        /// <summary>
        /// Sets the weight of an existing item.
        /// </summary>
        /// <param name="item">Item to modify.</param>
        /// <param name="newWeight">Value to set the weight to.</param>
        public void SetWeight(T item, float newWeight)
        {
            foreach (var entry in m_Entries)
            {
                if (EqualityComparer<T>.Default.Equals(entry.Item, item))
                {
                    entry.Weight = newWeight;
                    break;
                }
            }
            RecalculateWeights();
        }

        /// <summary>
        /// Recalculates the the total weight for all entries.
        /// </summary>
        public void RecalculateWeights()
        {
            m_AccumulatedWeight = 0;
            foreach (var entry in m_Entries)
            {
                m_AccumulatedWeight += entry.Weight;
            }
        }

        /// <summary>
        /// Returns a random item from the weighted list based off the items weights.
        /// </summary>
        /// <returns>Should always return a valid object. If return is null then something has modified the weights of the objects without recalculating.</returns>
        public T GetRandom()
        {
            double rand = m_Random.NextDouble() * m_AccumulatedWeight;
            float processedWeights = 0;

            foreach (Entry entry in m_Entries)
            {
                processedWeights += entry.Weight;

                if (processedWeights >= rand)
                {
                    return entry.Item;
                }
            }

            Debug.LogError(string.Format("Failed to find an entry of type {0} with the randomly generated weight. Weights were likely modified without being recalculated.", typeof(T).ToString()));

            return default(T);
        }
    }
}