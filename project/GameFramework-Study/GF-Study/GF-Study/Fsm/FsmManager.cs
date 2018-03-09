
using System;
using System.Collections.Generic;

namespace GameFramework.Fsm
{
    internal sealed class FsmManager : GameFrameworkModule, IFsmManager
    {
        private readonly Dictionary<string, FsmBase> m_Fsms;
        private readonly List<FsmBase> m_TempFsms;

        public FsmManager()
        {
            m_Fsms = Dictionary<string, FsmBase>();
            m_TempFsms = new List<FsmBase>();
        }

        internal override int Priority
        {
            get
            {
                return 60;
            }
        }
        internal override void Update()
        {
            m_TempFsms.Clear();
            if (m_Fsms.Count <= 0)
            {
                return;
            }

            foreach (KeyValuePair<string, FsmBase> fsm in m_Fsms)
            {
                m_TempFsms.Add(fsm.Value);
            }

            foreach (FsmBase fsm in m_TempFsms)
            {
                if (fsm.IsDestroyed)
                {
                    continue;
                }
                fsm.Update();
            }
        }

        internal override void Shutdown()
        {
            foreach (KeyValuePair<string, FsmBase> fsm in m_Fsms)
            {
                fsm.Value.Shutdown();
            }

            m_Fsms.Clear();
            m_TempFsms.Clear();
        }

        public bool HasFsm<T>() where T : class
        {
            return InternalHasFsm(Utility.Text.GetFullName<T>(string.Empty));
        }
        public IFsm<T> GetFsm<T>(string name) where T : class
        {
            return (IFsm < T > InternalGetFsm(Utility.Text.GetFullName<T>(name)));
        }

        public IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class
        {
            if (HasFsm<T>(name))
            {

            }
            Fsm<T> fsm = new Fsm<T>(name, owner, states);
            m_Fsms.Add(Utility.Text.GetFullName<T>(name), fsm);
            return fsm;
        }

        private bool InternalDestroyFsm(string fullName)
        {
            FsmBase fsm = null;
            if (m_Fsms.TryGetValue(fullName, out fsm))
            {
                fsm.Shutdown();
                return m_Fsms.Remove(fullName);
            }
            return false;
        }
    }

}