
namespace GameFramework
{
    internal abstract class GameFrameworkModule {
        internal virtual int Priority
        {
            get { return 0; }
        }

        internal abstract void Update();
        internal abstract void Shutdown();
    }

}