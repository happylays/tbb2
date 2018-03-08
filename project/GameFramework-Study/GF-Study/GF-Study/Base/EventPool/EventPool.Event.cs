
namespace GameFramework
{
    internal partial class EventPool<T>
    {
        private sealed class Event
        {
            private readonly object m_Senders;
            private readonly T m_EventArgs;

            public Event(object sender, T e)
            {
                m_Senders = sender;
                m_EventArgs = e;
            }
            public object Sender
            {
                get
                {
                    return m_Senders;
                }
            }
            public T EventArgs
            {
                get
                {
                    return m_EventArgs;
                }
            }
        }
    }
}