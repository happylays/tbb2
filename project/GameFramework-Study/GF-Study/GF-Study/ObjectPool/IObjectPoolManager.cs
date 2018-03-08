
using System;
using System.Collections.Generic;

namespace GameFramework.ObjectPool
{
    internal sealed partial class ObjectPoolManager : GameFrameworkModule, IObjectPoolManager
    {
        private const int DefaultCapacity = int.MaxValue;
        private const float DefaultExpireTime = float.MaxValue;
        private readonly Dictionary<string, ObjectPoolBase> m_ObjectPools;

        public ObjectPoolManager()
        {
            m_ObjectPools = new Dictionary<string, ObjectPoolBase>();
        }
        internal override int Priority
        {
            get
            {
                return 90;
            }
        }

        int Count
        {
            get
            {
                return m_ObjectPools.Count;
            }
        }
        internal override void Update()
        {
            foreach (KeyValuePair<string, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                objectPool.Value.Update();
            }   
        }
        internal override void Shutdown()
        {
            foreach (KeyValuePair<string, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                objectPool.Value.Shutdown();
            }
            m_ObjectPools.Clear();
        }

        public bool HasObjectPool<T>() where T : ObjectBase
        {
            return InternalHasObjectPool(Utility.Text.GetFullName<T>(string.Empty));
        }
        bool HasObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw;
            }
            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw;
            }
            return InternalHasObjectPool(Utility.Text.GetFullName(objectType, string.Empty));
        }
        bool HasObjectPool<T>(string name) where T : ObjectBase;
        bool HasObjectPool(Type objectType, string name);
        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase
        {
            return (IObjectPool<T>)InternalHasObjectPool(Utility.Text.GetFullName<T>(string.Empty));
        }
        public ObjectPoolBase GetObjectPool(Type objectTpe)
        {
            return InternalGetObjectPool(Utility.Text.GetFullName(objectType, string.Empty));
        }
        IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase;
        public ObjectPoolBase[] GetAllObjectPools(bool sort)
        {
            if (sort)
            {
                List<ObjectPoolBase> objectPools = new List<ObjectPoolBase>(m_ObjectPools.Values);
                objectPools.Sort(ObjectPoolComparer);
                return objectPools.ToArray();
            }
            else
            {
                int index = 0;
                ObjectPoolBase[] objectPools = new ObjectPoolBase[m_ObjectPools.Count];
                foreach (KeyValuePair<string, ObjectPoolBase> objectPool in m_ObjectPools)
                {
                    objectPools[index++] = objectPool.Value;
                }
                return objectPools;
            }
        }
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, DefaultCapacity, DefalutExpireTime, DefaultPriority);
        }

        private bool InternalHasObjectPool(string fullName)
        {
            return m_ObjectPools.ContainsKey(fullName);
        }
        private ObjectPoolBase InternalGetObjectPool(string fullName)
        {
            ObjectPoolBase objectPool = null;
            if (m_ObjectPools.TryGetValue(fullName, out objectPool))
            {
                return objectPool;
            }
            return null;
        }
        private int ObjectPoolComparer(ObjectPoolBase a, ObjectPoolBase b)
        {
            return a.Priority.CompareTo(b.Priority);
        }
        private IObjectPool<T> InternalCreateObjectPool<T>(string name, bool allowMultiSpawn, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            if (HashObjectPool<T>(name))
            {
                throw;
            }

            ObjectPool<T> objectPool = new ObjectPool<T>(name, allowMultiSpawn, capacity, expireTime, priority);
            m_ObjectPools.Add(Utility.Text.GetFullName<T>(name, objectPool));
            return objectPool;
        }
        public bool DestroyObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw;
            }
            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw;
            }

            return InternalDestroyObjectPool(Utility.Text.GetFullName(objectType, string.Empty));
        }
        private bool InternalDestroyObjectPool(string fullName)
        {
            ObjectPoolBase objectPool = null;
            if (m_ObjectPools.TryGetValue(fullName, out objectPool))
            {
                objectPool.Shutdown();
                return m_ObjectPools.Remove(fullName);
            }

            return false;
        }
        public void Release()
        {
            ObjectPoolBase[] objectPools = GetAllObjectPools(true);
            foreach (ObjectPoolBase objectPool in objectPools)
            {
                objectPool.Release();
            }
        }
        public void ReleaseAllUnused()
        {
            ObjectPoolBase[] objectPools = GetAllObjectPools(true);
            foreach(ObjectPoolBase objectPool in objectPools)
            {
                objectPool.ReleaseAllUnused();
            }
        }
    }
}