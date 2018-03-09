 namespace GameFramework.Fsm
{
    public delegate void FsmEventHandler<T>(IFsm<T> fsm, object sender, object userData) where T : class;
}