using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study
{
    public class Fsm : FsmBase, IFsm
    {
        T m_Owner;
        Dictionary<string, FsmState> m_States;
        Dictionary<string, Variable> m_Datas;
        FsmState m_CurrentState;
        float m_CurrentStateTime;
        bool m_IsDestroyed;

        public Fsm(string name, T Owner, FsmState states) 
            : base(name)
        {
            m_Owner = Owner;
            m_States = new Dictionary<string, FsmState>();
            m_Dates = new Dictionary<string, Variable>();

            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new GameFrameworkException("FSM states is invalid.");
                }

                string stateName = state.GetType().FullName;
                if (m_States.ContainsKey(stateName))
                {
                    throw new GameFrameworkException(string.Format("FSM '{0}' state '{1}' is already exist.", Utility.Text.GetFullName<T>(name), stateName));
                }

                m_States.Add(stateName, state);
                state.OnInit(this);


            }
        }
        int Owner { }
        int OwnerType { }
        int FsmStateCount { }
        int IsRunning { }
        int IsDestroyed { }
        int CurrentState { }
        string CurrentStateName { }
        int CurrentStateTime { }
        void Update() { }
        void Shutdown() { }
        void Start() { }
        void HasState() { }
        void GetState() { }
        void FireEvent() { }
        void HasData() { }
        void GetData() { }
        void SetData() { }
        void RemoveData() { }
        void ChangeState() { }

    }
}
