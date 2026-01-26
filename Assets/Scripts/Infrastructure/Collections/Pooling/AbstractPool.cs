namespace FormForge.Collections
{
    using System;
    using System.Collections.Generic;

    public abstract class AbstractPool
    {
        protected IGenerator Generator;
        protected HashSet<IPoolable> Pool;
        protected HashSet<IPoolable> AllocatedObjects;
        protected Queue<IPoolable> AvailableObjects;

        /// <summary>
        /// The total size of the Object Pool
        /// </summary>
        public int Size
        {
            get
            {
                return Pool.Count;
            }
        }

        /// <summary>
        /// The total number of available Objects in the pool.
        /// Should you run out, additional Objects will be created for you as needed.
        /// </summary>
        public int AvailableItems
        {
            get
            {
                return AvailableObjects.Count;
            }
        }

        public bool IsAlive
        {
            get
            {
                return Pool != null;
            }
        }

        /// <summary>
        /// The Type of the Pool that is being created
        /// </summary>
        public abstract Type PoolType
        {
            get;
        }

        /// <summary>
        /// Returns an object to the pool making it available for re-use.
        /// </summary>
        /// <param name="obj">The object to return to the pool. Must have originated from the pool.</param>
        public virtual void Recycle(IPoolable obj)
        {
            if (Pool == null)
            {
                throw new InvalidOperationException("The Pool has been destroyed. Objects can no longer be Recycled into it.");
            }

            if (Pool.Contains(obj))
            {
                if (AllocatedObjects.Contains(obj))
                {
                    obj.OnDeallocate();
                    AllocatedObjects.Remove(obj);
                    AvailableObjects.Enqueue(obj);
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot Recycle Objects into a Pool that did not originate from it.");
            }
        }

        /// <summary>
        /// Recycle all allocated objects.
        /// </summary>
        public void RecycleAll()
        {
            if (Pool == null)
            {
                throw new InvalidOperationException("Pool has already been destroyed.");
            }

            IPoolable[] allocated = new IPoolable[AllocatedObjects.Count];
            AllocatedObjects.CopyTo(allocated);
            foreach (IPoolable item in allocated)
            {
                Recycle(item);
            }

            AllocatedObjects.Clear();
        }

        /// <summary>
        /// Cleans up the Pool, prepping it for garbage collection. This will orphan any objects that are currently allocated.
        /// If you would like to also destroy the allocated objects see <see cref="RecycleAndDestroyAll"/>
        /// </summary>
        public virtual void Destroy()
        {
            if (Pool == null)
            {
                throw new InvalidOperationException("Pool has already been destroyed.");
            }

            while (AvailableObjects.Count > 0)
            {
                AvailableObjects.Dequeue().Destroy();
            }

            Pool = null;
            AllocatedObjects = null;
            AvailableObjects = null;
        }

        /// <summary>
        /// Recycles all allocated objects and cleans up the Pool, prepping it for garbage collection.
        /// </summary>
        public virtual void RecycleAndDestroyAll()
        {
            if (Pool == null)
            {
                throw new InvalidOperationException("Pool has already been destroyed.");
            }

            foreach (IPoolable item in AllocatedObjects)
            {
                item.OnDeallocate();
            }

            foreach (IPoolable item in Pool)
            {
                item.Destroy();
            }

            Pool = null;
            AllocatedObjects = null;
            AvailableObjects = null;
        }

        /// <summary>
        /// Aquires an unallocated object from the pool and provides it for use. If no unallocated objects are available, a new one will be created.
        /// </summary>
        /// <returns>An object of type IPoolable for use</returns>
        protected IPoolable Acquire()
        {
            if (Pool == null)
            {
                throw new InvalidOperationException("The Pool has been destroyed. Objects can no longer be Aquired from it.");
            }

            if (AvailableObjects.Count == 0)
            {
                CreateNewInstance();
            }

            IPoolable obj = AvailableObjects.Dequeue();
            obj.OnAllocate();
            AllocatedObjects.Add(obj);
            return obj;
        }

        /// <summary>
        /// Creates the Pool containers and fills them to the specified size.
        /// </summary>
        /// <param name="initialSize">Initial size of the Pool</param>
        protected void InitializePool(uint initialSize)
        {
            Pool = new HashSet<IPoolable>();
            AllocatedObjects = new HashSet<IPoolable>();
            AvailableObjects = new Queue<IPoolable>();

            for (int i = 0; i < initialSize; ++i)
            {
                CreateNewInstance();
            }
        }

        /// <summary>
        /// Creates and adds a new instance of the poolable object to the pool.
        /// Once this is done the size of the pool will have increased by one and a new object will be available within it.
        /// </summary>
        protected void CreateNewInstance()
        {
            IPoolable newObj = Generator.CreateInstance();
            Pool.Add(newObj);
            AvailableObjects.Enqueue(newObj);
            newObj.Init(this);
        }
    }
}
