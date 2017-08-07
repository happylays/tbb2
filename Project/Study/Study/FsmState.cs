using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Fsm
{
    public abstract class FsmState<T> where T : class
    {
        private readonly Dictionary<int, FsmEventHandler<T>> m_EventHandlers;

        public FsmState() { 
            m_EventHandlers = new Dictionary<int, FsmEventHandler<T>>();
        }
        protected internal virtual void OnInit(IFsm<T> fsm) { }
        protected internal virtual void OnEnter(IFsm<T> fsm) { }
        protected internal virtual void OnUpdate(IFsm<T> fsm，float elapseSeconds, float realElapseSeconds) { }
        protected internal virtual void OnLeave(IFsm<T> fsm, bool isShutdown) { }
        protected internal virtual void OnDestroy(IFsm<T> fsm) { 
            m_EventHandlers.Clear();
        }
        protected void ChangeState(IFsm<T> fsm, Type stateType) { 
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null) {
                throw new;
            }
            fsmImplement.ChangeState(stateType);
        
        }
        protected void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T> {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            fsmImplement.ChangeState<TState>();
        }
        protected void SubscribeEvent(int eventId, FsmEventHandler<T> eventhandler) { 
            if (eventhandler == null) {
                throw new;
            }

            if (!m_EventHandlers.ContainsKey(eventId)) {
                m_EventHandlers = eventhandler;
            }
            else {
                m_EventHandlers += eventhandler;
            }
        }
        protected void UnSubscribe(int eventId, FsmEventHandler<T> eventhandler) { }
       
        protected internal virtual void OnLeave(IFsm<T> fsm, bool isShutdown) { }

        internal void OnEvent(IFsm<T> fsm, object sender, int eventId, object userData)
        {
            FsmEventHandler<T> eventHandlers = null;
            if (m_EventHandlers.TryGetValue(eventId, eventHandlers))
            {
                if (eventHandlers != null)
                {
                    eventHandlers(fsm, sender, userData);
                }
            }
        }
    }
}
