
using System;

namespace GameFramework.ObjectPool
{
    public interface IObjectPool<T> where T : ObjectBase
    {
        string Name
        {
            get;
        }
        Type ObjectType
        {
            get;
        }
        int Count
        {
            get;
        }
        int CanReleaseCount
        {
            get;
        }
        bool AllowMultiSpawn
        {
            get;
        }
        float AutoReleaseInterval
        {
            get;
            set;
        }
        int Capacity
        {
            get;
            set;
        }

        void Register(T obj, bool spawned);
        void CanSpawn();
        void CanSpawn(string name);
        T Spawn();
        T Spawn(string name);
        void Upspawn(T obj);
        void Unspawn(object target);
        void SetPriority(T obj, int priority);
        void Release();
        void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback);
        void Release(int toReleaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback);
        void ReleaseAllUnused();


    }
}