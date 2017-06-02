using LoveDance.Client.Logic;
using System.Collections;

namespace LoveDance.Client.Logic.Scene
{
    public abstract class SceneSwitchBase
    {
        public virtual bool IsCreatingScene { set { } }	// 场景是否在创建中
        public virtual string SceneName { get { return null; } }		// 生成的场景名称
        public virtual bool CanSwitch { get { return true; } }			// true 能进行跳转

        /// <summary>
        /// 准备切换场景
        /// </summary>
        public virtual IEnumerator PrepareSwitch()
        {
            yield return null;
        }

        /// <summary>
        /// 准备切换场景
        /// </summary>
        public virtual IEnumerator BeginSwitch()
        {
            yield return null;
        }

        /// <summary>
        /// 加载场景动画
        /// </summary>
        public virtual IEnumerator LoadSceneAnimation()
        {
            yield return null;
        }

        /// <summary>
        /// 添加场景脚本
        /// </summary>
        /// <returns>用于调用PlayerEnterScene</returns>
        public virtual IScenceType AddComponent()
        {
            return null;
        }

        /// <summary>
        /// 准备切换场景
        /// </summary>
        public virtual IEnumerator CreateScenePlayer()
        {
            yield return null;
        }

        /// <summary>
        /// 准备UI和对应
        /// </summary>
        public virtual IEnumerator PrepareUIAndData()
        {
            yield return null;
        }

        /// <summary>
        /// 切换场景完成
        /// </summary>
        public virtual IEnumerator FinishSwitch()
        {
            yield return null;
        }

        public IEnumerator OnReleaseCachedData()
        {            
            yield return null;
        }
    }
}