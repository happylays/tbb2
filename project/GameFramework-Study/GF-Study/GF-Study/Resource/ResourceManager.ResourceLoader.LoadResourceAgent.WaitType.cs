
namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceLoader
        {
            private partial class LoadResourceAgent
            {
                private enum WaitingType
                {
                    None = 0,
                    WaitForAsset,
                    WaitForDependencyAsset,
                    WaitForResource,
                }
            }
        }
    }
}