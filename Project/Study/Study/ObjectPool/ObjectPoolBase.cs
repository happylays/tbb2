using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.ObjectPool
{
    public abstract class ObjectPoolBase
    {
        private readonly string m_Name;

        public ObjectPoolBase()
            : this(null)
        {
            
        }
        public abstract void Release();
        public abstract ObjectInfo[] GetAllObjectInfos();
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);
        internal abstract void Shutdown();
    }
}
