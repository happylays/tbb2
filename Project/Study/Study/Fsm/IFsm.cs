using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Fsm
{
    public interface IFsm<T> where T : class
    {
        string Name { get; }
        T Owner { get; }
        int FsmStateCount { get; }
        bool IsRunning { get; }
        bool IsDestroyed { get; }
        FsmState<T> CurrentState { get; }
        float CurrentStateTime { get; }

        void Start(Type stateType);
        bool HasState<TState>() where TState : FsmState<T>;
        bool HasState(Type stateType);
        TState GetState<TState>() where TState : FsmState<T>;
        FsmState<T> GetState(Type stateType);
        void FireEvent(object sender, int eventId);
        void FireEvent(object sender, int eventId, object userData);
        bool HasData(string name);
        Variable GetData(string name);
        TData GetData<TData>(string name) where TData : Variable;
        TData SetData<TData>(string name, TData data) where TData : Variable;
        bool RemoveData(string name);
    }
}
