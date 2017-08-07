using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.ObjectPool
{
    public class ObjectPoolManager
    {
        private const int DefaultCapacity = int.MaxValue;

        private readonly Dictionary<string, ObjectPoolBase> m_ObjectPools;

        public ObjectPoolManager() {
            m_ObjectPools = new Dictionary<string, ObjectPoolBase>();
        }
        internal override void Update(float elapseSeconds, float realElapseSeconds) {
            foreach (KeyValuePair<string, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                objectPool.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }
        internal override void Shutdown() {
            foreach (KeyValuePair<string, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                objectPool.Value.Shutdown();
            }
            m_ObjectPools.Clear();
        }
        public bool HasObjectPool<T>(string name) where T : ObjectBase {
            
        }
        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase { }
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
                foreach (KeyValuePair<string, ObjectPoolBase> objectPool in m_ObjectPools) {
                    objectPools[index++] = objectPool.Value;
                }
                return objectPools;
            }
        }
        
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase { }
        public bool DestroyObjectPool<T>(string name) where T : ObjectBase {
            string fullName = Utility.Text.GetFullName(name);
            ObjectPool objectPool = null;
            if (m_ObjectPools.TryGetValue(fullName, objectPool))
            {
                ObjectPool.Shutdown();
                return m_ObjectPools.Remove(ObjectPool);
            }

            return false;
        }
        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        public void Release() {
            ObjectPoolBase[] objectPools = GetAllObjectPools();
            foreach (ObjectPoolBase objectPool in objectPools)
            {
                objectPool.Release();

            }
        }
        public void ReleaseAllUnused() { }
        private IObjectPool<T> CreateObjectPool<T>(string name, bool allowMultiSpawn, int capacity, float expireTime, int priority) where T : ObjectBase {
            if (HasObjectPool<T>(name))
            {
                throw new;
            }

            ObjectPool<T> objectPool = new ObjectPool<T>(name, allowMultiSpawn, capacity, expireTime, priority);
            m_ObjectPools.Add(Utility.Text.GetFullName<T>(name), objectPool);
            return objectPool;
        }
        private int ObjectPoolComparer(ObjectPoolBase a, ObjectPoolBase b) {
            return a.Priority.CompareTo(b.Priority);
        }
    }
}
