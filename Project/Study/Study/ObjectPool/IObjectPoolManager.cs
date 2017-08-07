using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// 对象池管理器
    /// </summary>
    public interface IObjectPoolManager
    {
        /// <summary>
        /// 获取对象池数量
        /// </summary>
        int Count { get; }
        bool HasObjectPool<T>() where T : ObjectBase;
        IObjectPool<T> GetObjectPool<T>() where T : ObjectBase;
        IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase;
        bool DestroyObjectPool<T>() where T : ObjectBase;
        void Release();
        void ReleaseAllUnused();

    }
}
