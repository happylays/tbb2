
using System;

namespace GameFramework.ObjectPool
{
    public abstract class ObjectPoolBase
    {
        private readonly string m_Name;
        public ObjectPoolBase() 
            : this(null)
        {

        }
        public ObjectPoolBase(string name)
        {

        }
        public string Name
        {
            get
            {
                return m_Name;
            }
        }
        public abstract Type ObjectType
        {
            get;
        }
        public abstract int Count
        {

        }
        public abstract int CanReleaseCount
        {
            get;
        }
        public abstract float AutoReleaseInterval
        {
            get;
            set;
        }
        public abstract float ExpireTime
        {
            get;
            set;
        }
        public abstract void Release();
        public abstract void Release(int toReleaseCount);
        public abstract void ReleaseAllUnused();
        public abstract ObjectInfo[] GetAllObjectInfos();
        internal abstract void Update();
        internal abstract void Shutdown();
    }
}