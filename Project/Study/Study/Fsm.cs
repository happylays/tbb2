using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Fsm
{
    public class Fsm : FsmBase, IFsm
    {
        T m_Owner;
        Dictionary<string, FsmState> m_States;
        Dictionary<string, Variable> m_Datas;
        FsmState m_CurrentState;
        float m_CurrentStateTime;
        bool m_IsDestroyed;

        public Fsm(string name, T Owner, FsmState<T> states) 
            : base(name)
        {
            if (owner == null)
            {
                throw new ;
            }

            if (states == null || states.Length < 1) {
                throw new;
            }

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
        public T Owner {
            get
            {
                return m_Owner;
            }
        }
        int OwnerType {
            get
            {
                return typeof(T);
            }
        }
        int FsmStateCount { }
        int IsRunning { }
        int IsDestroyed { }
        int CurrentState { }
        string CurrentStateName { }
        int CurrentStateTime { }
        internal override void Update(float elapseSeconds, float realElapseSeconds) {
            if (m_CurrentState == null)
            {
                return;
            }

            m_CurrentStateTime += elapseSeconds;
            m_CurrentState.OnUpdate(this, elapseSeconds, realElapseSeconds);
        }
        internal override void Shutdown() {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnLeave(this, true);
                m_CurrentState = null;
                m_CurrentStateTime = 0f;
            }

            foreach (KeyValuePair<string, FsmState<T>> state in m_States)
            {
                state.Value.OnDestroy(this);
            }

            m_States.Clear();
            m_Datas.Clear();

            m_IsDestroyed = true;
        }
        public void Start(Type stateType) {
            FsmState<T> state = GetState(stateType);

            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
        public bool void HasState<TState>() where TState : FsmState<T> {
            return HasState(typeof(TState));
        }
        public bool HasState(Type stateType) {
            return m_States.ContainsKey(stateType.FullName);
        }
        public TState GetState<TState>() where TState : FsmState<T> {
            return (TState)GetState(typeof(TState))；
        }
        public FsmState<T> GetState(Type stateType) {
            FsmState<T> state = null;
            if (m_States.TryGetValue(stateType.FullName, out state)) {
                return state;
            }

            return null;
        }

        public void FireEvent(object sender, int eventId) {
            if (m_CurrentState == null) {
                throw new ;
            }

            m_CurrentState.OnEvent(this, sender, eventId, null);
        }
        public bool HasData(string name) {
            if (string.IsNullOrEmpty(name)) {
                throw new GameFrameworkException("Data name is invalid");
            }

            return m_Datas.ContainsKey(name);
        }
        Variable GetData() {
            m_Datas.TryGetValue(name, out data);
            return data;
        }
        public void SetData<TData>(string name, TData data) where TData : Variable {
            m_Datas[name] = data;
        }
        void RemoveData(string name) {
            if (string.IsNullOrEmpty(name)) {
                throw new GameFrameworkException("Data name is invalid!")
            }
            return m_Datas.Remove(name);
        }

        internal void ChangeState<TState>() where TState : FsmState<T> {
            ChangeState(typeof(TState));
        }

        void ChangeState(Type stateType) { 
            FsmState<T> state = GetState(stateType);

            m_CurrentState.OnLeave(this, false);
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

    }
}
