

using System;

namespace GameFramework.Fsm
{
    public interface IFsmManager
    {
        int Count { get; }
        bool HasFsm<T>() where T : class;
        bool HasFsm(Type ownerType);
        bool HasFsm<T>(string name) where T : class;
        IFsm<T> GetFsm<T>() where T : class;
        FsmBase GetFsm(Type ownerType, string name);
        IFsm<T> CreateFsm(T owner, params FsmState<T> states) where T : class;
        bool DestroyFsm<T>() where T : class;
        bool DestroyFsm(FsmBase fsm);
    }
}