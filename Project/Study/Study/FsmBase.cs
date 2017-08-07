using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Fsm
{
    public class FsmBase
    {
        private readonly string m_Name;

        FsmBase() : this(null) { }
        public FsmBase(string name) {
            m_Name = name ?? string.Empty;
        }
        public string Name
        {
            get { return m_Name; }
        }
        public abstract Type OwnerType { get; }
        public abstract int FsmStateCount { get; }
        public abstract bool IsRunning { get; }
        public abstract bool IsDestroyed { get; }
        public abstract string CurrentStateName { get; }
        public abstract float CurrentStateTime { get; }
        internal abstract void Update(float elapseSeconds, float realElapseSeconds) { }
        internal abstract void Shutdown() { }
    }
}
