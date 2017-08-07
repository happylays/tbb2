using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Fsm
{
    internal sealed class FsmManager : IFsmManager
    {
        private readonly Dictionary<int, FsmBase> m_Fsms;
        private readonly List<FsmBase> m_TempFsms;

        public FsmManager() {
            m_Fsms = new Dictionary<int, FsmBase>();
            m_TempFsms = new List<FsmBase>();
        }

        internal override int Priority
        {
            get { return 60; }
        }

        public int Count
        {
            get { return m_Fsms.Count; }
        }

        void Update(float elapseTime, float realElapseTime)
        {
            m_TempFsms.Clear();
            if (m_Fsms.Count < 0)
                return;

            foreach (KeyValuePair<int, FsmBase> fsm in m_Fsms)
            {
                m_TempFsms.Add(fsm.Value);
            }

            foreach (FsmBase fsm in m_TempFsms)
            {
                if (fsm.IsDestroyed)
                    continue;

                fsm.Update(elapseTime, realElapseTime);
            }
        }
        

        internal override void Shutdown() {
            foreach (KeyValuePair<int, FsmBase> fsm in m_Fsms)
            {
                fsm.Value.Shutdown();
            }

            m_Fsms.Clear();
            m_TempFsms.Clear();
        }
        public bool HasFsm<T>() where T : class {
            return HasFsm<T>(string.Empty);
        }
        public bool HasFsm<T>(string name) where T : class
        {
            return m_Fsms.ContainsKey(Utility.Text.GetFullName<T>(name));
        }
        public IFsm<T> GetFsm<T>(string name) where T : class {
            FsmBase fsm = null;
            if (m_Fsms.TryGetValue(Utility.Text.GetFullName<T>(name), out fsm))
            {
                return (IFsm<T>)fsm;
            }

            return null;
        }
        public IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[]) { 
            return CreateFsm(string.Empty, owner, states);
        }
        public IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[]) { 
            if (HasFsm<T>(name))
                throw new;
            
            Fsm<T> fsm = new Fsm<T>(name, owner, states);
            m_Fsms.Add(Utility.Text.GetFullName(name), fsm);
            return fsm;
        }
        public bool DestroyFsm<T>(string name) where T : class {
            FsmBase fsm = null;
            if (m_Fsms.TryGetValue(name, out fsm))
            {
                fsm.Shutdown();
                return m_Fsms.Remove(name);
                
            }
            return false;
        }
    }
}
