using System;

namespace GameFramework
{
    public interface IEventManager
    {
        int Count { get; }
        bool Check(int id, EventHandler<GameFrameworkEventArgs> handler);
        void Subscribe(int id, EventHandler<GameEventArgs> handler);
        void Unsubscribe(int id, EventHandler<GameEventArgs> handler);
        void Fire(object sender, GameEventArgs e);
        void FireNow(object sender, GameEventArgs e);
    }

}