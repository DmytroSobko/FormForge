using System;
using System.Collections;
using System.Collections.Generic;

namespace FormForge.Collections
{
    /// <summary>
    /// A generic list that supports observation of changes, including item additions, removals, and full list changes.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <remarks>
    /// <para>
    /// This class acts similarly to a standard <see cref="List{T}"/>, but provides events for when items are added,
    /// removed, or when the list changes. This is useful for UI binding or reactive systems where you want to observe
    /// changes to the list state.
    /// </para>
    /// <para>
    /// Events:
    /// <list type="bullet">
    /// <item><description><see cref="ItemsAdded"/> — invoked with a list of newly added items.</description></item>
    /// <item><description><see cref="ItemsRemoved"/> — invoked with a list of removed items.</description></item>
    /// <item><description><see cref="Changed"/> — invoked after any change to the list (add, remove, clear, swap, etc.).</description></item>
    /// </list>
    /// </para>
    /// </remarks> 
    public class ObservableList<T> : IEnumerable<T>
    {
        private List<T> m_Items = new List<T>();

        /// <summary>
        /// Invoked when items are added to the list.
        /// </summary>
        public event Action<List<T>> ItemsAdded;

        /// <summary>
        /// Invoked when items are removed from the list.
        /// </summary>
        public event Action<List<T>> ItemsRemoved;

        /// <summary>
        /// Invoked after any change to the list.
        /// </summary>
        public event Action Changed;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to get.</param>
        public T this[int index] => m_Items[index];

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        public int Count => m_Items.Count;

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator() => m_Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Adds an item to the list and notifies observers.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            m_Items.Add(item);
            ItemsAdded?.Invoke(new List<T> { item });
            NotifyChanged();
        }

        /// <summary>
        /// Adds a range of items to the list and notifies observers.
        /// </summary>
        /// <param name="items">The list of items to add.</param>
        public void AddRange(List<T> items)
        {
            m_Items.AddRange(items);
            ItemsAdded?.Invoke(new List<T>(items));
            NotifyChanged();
        }

        /// <summary>
        /// Removes the specified item from the list and notifies observers.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was successfully removed; otherwise, false.</returns>
        public bool Remove(T item)
        {
            bool removed = m_Items.Remove(item);
            if (removed)
            {
                ItemsRemoved?.Invoke(new List<T> { item });
                NotifyChanged();
            }

            return removed;
        }

        /// <summary>
        /// Removes all items that match the specified predicate and notifies observers.
        /// </summary>
        /// <param name="match">The predicate that defines the conditions of the elements to remove.</param>
        public void RemoveAll(Predicate<T> match)
        {
            var removedItems = m_Items.FindAll(match);
            if (removedItems.Count > 0)
            {
                m_Items.RemoveAll(match);
                ItemsRemoved?.Invoke(removedItems);
                NotifyChanged();
            }
        }

        /// <summary>
        /// Determines whether the list contains a specific item.
        /// </summary>
        /// <param name="item">The item to locate in the list.</param>
        public bool Contains(T item) => m_Items.Contains(item);

        /// <summary>
        /// Returns the index of the specified item.
        /// </summary>
        /// <param name="item">The item to locate in the list.</param>
        /// <returns>The zero-based index of the item, or -1 if not found.</returns>
        public int IndexOf(T item) => m_Items.IndexOf(item);

        /// <summary>
        /// Returns a shallow copy of the list as a new <see cref="List{T}"/>.
        /// </summary>
        public List<T> ToList() => new List<T>(m_Items);

        /// <summary>
        /// Inserts an item at the specified index and notifies observers.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        public void Insert(int index, T item)
        {
            m_Items.Insert(index, item);
            ItemsAdded?.Invoke(new List<T> { item });
            NotifyChanged();
        }

        /// <summary>
        /// Removes the item at the specified index and notifies observers.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            var item = m_Items[index];
            m_Items.RemoveAt(index);
            ItemsRemoved?.Invoke(new List<T> { item });
            NotifyChanged();
        }

        /// <summary>
        /// Clears all items from the list and notifies observers.
        /// </summary>
        public void Clear()
        {
            ItemsRemoved?.Invoke(new List<T>(m_Items));
            m_Items.Clear();
            NotifyChanged();
        }

        /// <summary>
        /// Swaps two items in the list at the specified indices and notifies observers.
        /// </summary>
        /// <param name="i">The index of the first item.</param>
        /// <param name="j">The index of the second item.</param>
        public void Swap(int i, int j)
        {
            (m_Items[i], m_Items[j]) = (m_Items[j], m_Items[i]);
            NotifyChanged();
        }

        /// <summary>
        /// Notifies observers that the list has changed.
        /// </summary>
        private void NotifyChanged()
        {
            Changed?.Invoke();
        }
    }
}
