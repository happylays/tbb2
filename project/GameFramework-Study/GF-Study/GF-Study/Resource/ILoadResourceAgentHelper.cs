
using System;

namespace GameFramework.Resource
{
    public interface ILoadResourceAgentHelper
    {
        event EventHandler<LoadResourceAgentHelperUpdateEventArgs> LoadResourceAgentHelperUpdate;
        void ReadFile(string fullPath);
        void ReadBytes(string fullPath, int loadType);
        void ParseBytes(byte[] bytes);
        void LoadAsset(object resource, string resourceChildName, bool isScene);
        void Reset();
    }
}