using UnityEngine;
using System.Collections;
using LoveDance.Client.Common;

namespace LoveDance.Client.Logic
{
    /// <summary>
    /// 场景要添加脚本类型
    /// </summary>
    public enum ScenceType : byte
    {
        None,
        CeremonyOut_Scene,
        CeremonyIn_Scene,
        Fairy_Scene,
        StarShow_Scene,
        CatWalkPK,
        Room_Scene,
    }

    public abstract class IScenceType : NetMonoBehaviour, IScene
    {
        public abstract IEnumerator IEPlayerEnterScene(bool bNewStyle);

        public virtual void OnReleaseScene()
        {
        }
    }
}
