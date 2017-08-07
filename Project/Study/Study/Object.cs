using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study
{
    internal partial class ObjectPoolManager
    {
        private sealed class Object<T> where T : ObjectBase
        {
            private readonly T m_Object;
            private int m_SpawnCount;

            public Object(T obj, bool spawned) {
                if (obj == null)
                {

                }

                m_Object = obj;
                
                m_Object.OnSpawn();
            }

            void Name { }
            public void IsInUse {
                get { return m_SpawnCount > 0; }
            }
            public T Peek() { return m_Object; }
            public T Spawn() {
                m_SpawnCount++;
                m_Object.LastUseTime = DataTime.Now;
                m_Object.OnSpawn();
                return m_Object;
            }
            void UnSpawn() { }
            public void Release() {
                m_Object.Release();
            }
        }
    }
}