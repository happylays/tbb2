using System;

namespace GameFramework.ObjectPool
{
    public abstract class ObjectBase
    {
        private readonly string m_Name;
        private readonly object m_Target;
        private bool m_Locked;
        private int m_Priority;
        private DataTime m_LastUseTime;

        public ObjectBase(object target) 
            : this(null, target, false, 0)
        {
        }
        public ObjectBase(string name, object target) 
            : this(name, target, false, 0)
        { }

        public ObjectBase(string name, object target, bool locked) 
            : this(name, target, locked, 0)
        {

        }
        public ObjectBase(string name, object target, int priority)
            : this(name, target, false, priority)
        {

        }
        public ObjectBase(string name, object target, bool locked, int priority)
        {
            if (target == null)
            {
                throw;
            }

            m_Name = name ?? string.Empty;
            m_Target = target;
            m_Locked = locked;
            m_Priority = priority;
            m_LastUseTime = DateTime.Now;
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public object Target
        {
            get
            {
                return m_Target;
            }
        }
        public DateTime LastUseTime
        {
            get
            {
                return m_LastUseTime;
            }
            internal set
            {
                m_LastUseTime = value;
            }
        }
        protected internal virtual void OnSpawn()
        {

        }
        protected internal virtual void OnUnspawn()
        {

        }
        protected internal abstract void Release();

    }
}