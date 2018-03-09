
using System;

namespace GameFramework.Fsm
{
    public abstract class FsmBase
    {
        private readonly string m_Name;

        public FsmBase()
            : this(null)
        {

        }
        public FsmBase(string name)
        {
            m_Name = name ?? string.Empty;
        }
        public abstract Type OwnerType
        {
            get;
        }
        public abstract int FsmStateCount
        {
            get;
        }

        internal abstract void Update();
        internal abstract void Shutdown();
    }
}