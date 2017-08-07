using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study
{
    public abstract class ObjectBase
    {
        private readonly string m_Name;
        private readonly int m_Target;
        private bool m_Locked;
        private int m_Priority;
        private DataTime m_LastUseTime;

        public ObjectBase(string name, object target)
        {
            if (target == null) {
                throw new;
            }

            m_Name = name ?? string.Empty;
            m_Target = target;
        }
        public object Target { get { return m_Target; } }

        protected internal virtual void OnSpawn();
        protected internal virtual void OnUnspawn();
        protected internal virtual void Release();
    }
}
