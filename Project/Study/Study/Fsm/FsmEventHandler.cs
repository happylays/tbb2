using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Fsm
{
    public delegate void FsmEventHandler<T>(IFsm<T> fsm, object sender, object userData) where T : class
}
