using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.ObjectPool
{
    internal partial class ObejctPoolManager
    {
        public class ObjectPool
        {
            private readonly LinkedList<Object<T>> m_Objects;
            private readonly int m_Capacity;
            private int m_Priority;

            public ObjectPool(string name, bool allowMultiSpawn, int capacity) {
                m_Objects = new LinkedList<Object<T>>();
                Capacity = capacity;
            }
            public override ObjectType { get {return typeof(T);}
            public override int Capacity {
                get {
                    return m_Capacity;
                }
                set {
                    if (value < 0) {}

                    if (m_Capacity == value) {
                        return;
                    }

                    m_Capacity = value;
                    Release();
                }
            }
            public void Register(T obj) {
                if (obj == null) 
                    return;
                
                Log.Debug(spawned ? "Object ")
                m_Objects.AddLast(obj);
                Release();
            }
            public T Spawn(string name) {
                foreach (Object<T> obj in m_Objects) {
                    if (obj.Name != name) {
                        continue;
                    }

                    if (!obj.IsInUse) {
                        return obj.Spawn();
                    }
                }
            }
            void UnSpawn(object target) {
                foreach(Object<T> obj in m_Objects) {
                    
                    if (obj.Peek().Target == target) {
                        obj.Unspawn();
                        Release();
                        return;
                    }
                }
            }
            public void Release() { 
                LinkedList<T> toReleaseObjects = GetCanReleaseObject();
                
                foreach (ObjectBase toReleaseObject in toReleaseObjects) {
                                        
                    foreach (Object<T> obj in m_Objects) {
                        if (obj.Peek() != toReleaseObject) {
                            m_Objects.Remove(obj);
                            obj.Release();
                            break;

                        }

                    }
                }
            }
            void ReleaseAllUnused() { 
                LinkedListNode<ObjectT>> current = m_Objects.First;
                while (current != null) {
                    if (current.Value.IsInUse)
                    { 
                        current = current.Next;
                        continue;
                    }

                    LinkedListNode<Object<T>> next = current.Next;
                    m_Objects.Remove(current);
                    current.Value.Release();
                    current = next;
                }
            }
            void Update() { 
                Release();
            }
            void Shutdown() { 
                LinkedListNode<Object<T>> current = m_Objects.First;
                while (current != null) {
                    LinkedListNode<Object<T>> next = current.Next;
                    m_Object.Remove(current);
                    current.Value.Release();
                    current = next;
                }
            }
            void GetCanReleaseObjects() { }
        }
    }
}