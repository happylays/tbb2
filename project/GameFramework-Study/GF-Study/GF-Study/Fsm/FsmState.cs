

using System;
using System.Collections.Generic;

namespace GameFramework.Fsm
{
    public abstract class FsmState<T> where T : class
    {
        private readonly Dictionary<int, FsmEventHandler<T>> m_EventHandlers;

        public FsmState()
        {
            m_EventHandlers = new Dictionary<int, FsmEventHandler<T>>();
        }

        protected internal virtual void OnInit(IFsm<T> fsm) { }
        protected internal virtual void OnEnter(IFsm<T> fsm) { }
        protected internal virtual void OnUpdate(IFsm<T> fsm, float sec) { }
        protected internal virtual void OnLeave(IFsm<T> fsm, bool isShutdown) { }
        protected internal virtual void OnDestroy(IFsm<T> fsm)
        {
            m_EventHandlers.Clear();
        }
        protected void SubscribeEvent(int eventId, FsmEventHandler<T> eventHandler)
        {
            if (eventHandler)
            {
                throw;
            }
            if (!m_EventHandlers.ContainsKey(eventId))
            {
                m_EventHandlers[eventId] = eventHandler;
            }
            else
            {
                m_EventHandlers[eventId] += eventHandler;
            }
        }

        protected void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            fsmImplement.ChangeState<TState>();
        }

        protected void ChangeState(IFsm<T> fsm, Type stateType)
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {

            }
            fsmImplement.ChangeState(stateType);
        }

        internal void OnEvent(IFsm<T> fsm, object sender, int eventId, object userData)
        {
            FsmEventHandler<T> eventHandlers = null;
            if (m_EventHandlers.TryGetValue(eventId, out EventHandler))
            {
                if(eventHandlers !=null)
                {
                    eventHandlers(fsm, sender, userData);
                }
            }
        }
    }
}