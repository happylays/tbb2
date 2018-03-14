
using GameFramework.Download;
using GameFramework.ObjectPool;
using System;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private static readonly char[] PackageListHeader = new char[] { 'E', 'L', 'P' };
        private static readonly char[] VersionListHeader = new char[] { 'E', 'L', 'V' };
        private static readonly char[] ReadOnlyListHeader = new char[] { 'E', 'L', 'R' };
        private static readonly char[] ReadWriteListHeader = new char[] { 'e', 'l', 'w' };
        private const string VersionListFileName = "version";
        private const string ResourceListFileName = "list";
        private const string BackupFileSuffixName = ".bak";
        private const byte ReadWriteListVersionHeader  0;

        private readonly Dictionary<string, AssetInfo> m_AssetInfos;
        private readonly Dictionary<string, AssetDependencyInfo> m_AssetDependencyInfos;
        private readonly Dictionary<ResourceName, ResourceInfo> m_ResourceInfos;

    }

}