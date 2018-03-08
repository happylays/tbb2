using System;

namespace GameFramework.ObjectPool
{
    internal partial class ObjectPoolManager
    {
        private sealed class Object<T> where T : ObjectBase
        {
            private readonly T m_Object;
            private int m_SpawnCount;

            public Object(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw;
                }
                m_Object = obj;
                m_SpawnCount = spawned ? 1 : 0;
                if (spawned)
                {
                    m_Object.OnSpawn();
                }
            }

            public string Name
            {
                get
                {
                    return m_Object.Name;
                }
            }
            public int Priority
            {
                get
                {
                    return m_Object.Priority;
                }
                internal set
                {
                    m_Object.Priority = value;
                }
            }
            public bool IsInUse
            {
                get
                {
                    return m_SpawnCount > 0;
                }
            }

            public T Peek()
            {
                return m_Object;
            }
            public T Spawn()
            {
                m_SpawnCount++;
                m_Object.LastUseTime = DateTime.Now;
                m_Object.OnSpawn();
                return m_Object;
            }
            public void UnSpawn()
            {
                m_Object.OnUnspawn();
                m_Object.LastUseTime = DateTime.Now;
                m_SpawnCount--;
                if (m_SpawnCount < 0)
                {
                    throw;
                }
            }

            public void Release()
            {
                m_Object.Release();
            }
        }
    }
}