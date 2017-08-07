using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.ObjectPool
{
    public interface IObjectPool<T> where T : ObjectBase
    {
        string Name { get; }
        Type ObjectType { get; }
        int Priority { get; set; }

        void Register(T obj, bool spawned);
        void CanSpawn();
        T Spawn();
        void UnSpawn(T obj);
        void SetPriority(T obj, int priority);
        void Release();
        void ReleaseAllUnused();
    }
}
