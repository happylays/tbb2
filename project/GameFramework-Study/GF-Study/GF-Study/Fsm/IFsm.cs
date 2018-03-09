
using System;
 namespace GameFramework.Fsm
{
    public interface IFsm<T> where T : class
    {
        string Name { get; }
        T Owner { get; }
        int FsmStateCount { get; }
        bool IsRunning { get; }
        bool IsDestroyed { get; }
        FsmState<T> CurrentState
        {
            get;
        }
        float CurrentStateTime
        {
            get;
        }
        void Start<TState>() where TState : FsmState<T>;
        void Start(Type stateType);
        bool HasState<TState>() where TState : FsmState<T>;
        bool HasStae(Type stateType);
        TState GetState<TState>() where TState : FsmState<T>;
        void FireEvent(object sender, int eventId);
        bool HasData(string name);
        Variable GetData(string name);
        void SetData<TData>(string name, TData data) where TData : Variable;
        bool RemoveData(string name);
    }
}