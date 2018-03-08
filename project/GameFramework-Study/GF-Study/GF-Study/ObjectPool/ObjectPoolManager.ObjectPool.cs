
using System;
using System.Collections.Generic;

namespace GameFramework.ObjectPool
{
    internal partial class ObjectPoolManager
    {
        private sealed class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
        {
            private readonly LinkedList<Object<T>> m_Objects;
            private readonly bool m_AllowMultiSpawn;
            private int m_Capacity;
            private float m_AutoReleaseInterval;
            private float m_ExpireTime;
            private int m_Priority;
            private float m_AutoReleaseTime;

            public ObjectPool(string name, bool allowMultiSpawn, int capacity, float expireTime, int priority)
               : base(name)
            {
                m_Objects = new LinkedList<Object<T>>();
                m_AllowMultiSpawn = allowMultiSpawn;
                m_AutoReleaseInterval = expireTime;
                Capacity = capacity;
                ExpireTme = expireTime;
                m_AutoReleaseTime = 0f;
            }

            public override Type ObjectType
            {
                get
                {
                    return typeof(T);
                }
            }

            public override int CanReleaseCount
            {
                get
                {
                    return GetCanReleaseObjects().Count;
                }
            }

            public override int Capacity
            {
                get
                {
                    return m_Capacity;
                }
                set
                {
                    if (value < 0)
                    {
                        throw;
                    }
                    if (m_Capacity == value)
                    {
                        return;
                    }
                    Log.Debug();
                    m_Capacity = value;
                    Release();
                }
            }

            public void Release(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw;
                }
                Log.Debug();
                m_Objects.AddLast(new Object<T>(obj, spawned));

                Release();
            }
            public bool CanSpawn(string name)
            {
                foreach (Object<T> obj in m_Objects)
                {
                    if (obj.Name != name)
                    {
                        continue;
                    }
                    if (m_AllowMultiSpawn || !obj.IsInUse)
                    {
                        return true;
                    }
                }

                return false;
            }

            public T Spawn()
            {
                return Spawn(string.Empty);
            }
            public T Spawn(string name)
            {
                foreach(Object<T> obj in m_Objects)
                {
                    if (obj.Name != name)
                    {
                        continue;
                    }
                    if (m_AllowMultiSpawn || !obj.IsInUse)
                    {
                        Log.Debug();
                        return obj.Spawn();
                    }
                }

                return null;
            }

            public void Unspawn(object target)
            {
                if (target == null)
                {
                    throw;
                }
                foreach(Object<T> obj in m_Objects)
                {
                    if (obj.Peek().Target == target)
                    {
                        Log.Debug();
                        obj.UnSpawn();
                        Release();
                        return;
                    }
                }

                throw new Excep;
            }

            public override void Release()
            {
                Release(m_Objects.Count - m_Capacity, DefaultReleaseObjectFilterCallback);
            }

            public void Release(int toReleaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                if (releaseObjectFilterCallback == null)
                {
                    throw;
                }
                m_AutoReleaseTime = 0f;
                if (toReleaseCount < 0)
                {
                    toReleaseCount = 0;
                }

                LinkedList<T> canReleaseObjects = GetCanReleaseObjects();
                LinkedList<T> toReleaseObjects = releaseObjectFilterCallback(canReleaseObjects, toReleaseCount, expireTime);
                if (toReleaseObjects == null || toReleaseObjects.Count < 0)
                {
                    return;
                }

                foreach (ObjectBase toReleaseObject in toReleaseObjects)
                {
                    if (toReleaseObject == null)
                    {
                        throw;
                    }

                    bool found = false;
                    foreach(Object<T> obj in m_Objects)
                    {
                        if (obj.Peek() != m_Objects)
                        {
                            continue;
                        }

                        m_Objects.Remove(obj);
                        obj.Release();
                        Log.Debug();
                        found = true;
                        break;
                    }

                    if (!found)
                    {
                        throw;
                    }
                }
            }

            public override void ReleaseAllUnused()
            {
                LinkedListNode<Object<T>> current = m_Objects.First;
                while (current != null)
                {
                    if (current.Value.IsInUse || current.Value.Locked)
                    {
                        current = current.Next;
                        continue;
                    }

                    LinkedListNode<Object<T>> next = current.Next;
                    m_Objects.Remove(current);
                    current.Value.Release();
                    Log.Debug();
                    current = next;
                }
            }

            internal override void Update()
            {
                m_AutoReleaseTime += realElapseSeconds;
                if (m_AutoReleaseTime < m_AutoReleaseInterval)
                {
                    return;
                }

                Release();
            }

            internal override void Shutdown()
            {
                
            }

            private LinkedList<T> DefaultReleaseObjectFilterCallback(LinkedList<T> candiateObjects, int toReleaseObject, DateTime expireTime)
            {
                LinkedList<T> toReleaseObjects = new LinkedList<T>();

                if (expireTime > DateTime.MinValue)
                {
                    LinkedListNode<T> current = candiateObjects.First;
                    while (current != null)
                    {
                        if (current.Value.LastUseTime <= expireTime)
                        {
                            toReleaseObject.AddLast(current.Value);
                            LinkedListNode<T> next = current.Next;
                            candiateObjects.Remove(current);
                            current = next;
                            continue;
                        }

                        current = current.Next;
                    }

                    toReleaseCount -= toReleaseObjects.Count;
                }

                for (LinkedListNode<T> i = candiateObjects.First; toReleaseObject > 0 && i != null; i = i.Next)
                {
                    for (LinkedListNode<T> j = i.Next; j != null; j = j.Next)
                    {
                        if (i.Value.Priority > j.Value.Priority || i.Value.LastUseTime > j.Value.LastUseTime)
                        {
                            T temp = i.Value;
                            i.Value = j.Value;
                            j.Value = temp;
                        }
                    }

                    toReleaseObjects.AddLast(i.Value);
                    toReleaseCount--;
                }

                return toReleaseObjects;
            }
        }
    }
}