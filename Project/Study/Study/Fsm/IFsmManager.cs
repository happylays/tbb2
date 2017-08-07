using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Fsm
    public interface IFsmManager
    {
        int Count { get; }

        bool HasFsm<T>() where T : class;
        void GetFsm<T>(string name) where T : class;
        FsmBase[] GetAllFsms();
        IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class;
        bool DestroyFsm<T>() where T : class;
    }
}
