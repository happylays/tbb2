
namespace GameFramework
{
    public abstract class BaseEventArgs : GameFrameworkEventArgs, IReference {
        public abstract int Id
        {
            get;
        }
        public abstract void Clear();
    }

}