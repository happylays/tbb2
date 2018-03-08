using System;

namespace GameFramework
{
    internal sealed class EventManager : GameFrameworkModule, IEventManager
    {
        private readonly EventPool<GameEventArgs> m_EventPool;

        public EventManager()
        {
            m_EventPool = new EventPool<GameEventArgs>(EventPoolMode.AllowNoHandler | EventPoolMode.AllowMultiHandler);
        }

        public int Count
        {
            get
            {
                return m_EventPool.Count;
            }
        }

        internal override int Priority
        {
            get
            {
                return 100;
            }
        }

        internal override void Shutdown()
        {
            m_EventPool.Shutdown();
        }

        public bool Check(int id, EventHandler<GameEventArgs> handler)
        {
            return m_EventPool.Check(id, handler);
        }
        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            m_EventPool.Subscribe(id, handler);
        }
        public void Unscribe(int id, EventHandler<GameEventArgs> handler)
        {
            m_EventPool.UnSubscribe(id, handler);
        }
        public void Fire(object sender, GameEventArgs e)
        {
            m_EventPool.Fire(sender, e);
        }
        public void FireNow(object sender, GameEventArgs e)
        {
            m_EventPool.FireNow(sender, e);
        }
    }

}