using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study
{
    public class ResourceLoader
    {
        void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            if (!CheckAsset(assetName, out resourceInfo, out dependencyAssetNames))
            {

            }

            LoadAssetTask mainTask = new LoadAssetTask(assetName, resourceInfo.Value, dependencyAssetNames);
            foreach (string dependencyAssetName in dependencyAssetNames)
            {
                if (!LoadDependencyAsset(dependencyAssetName, mainTask))
                {

                }
            }

            m_TaskPool.AddTask(mainTask);
        }
    }
}
