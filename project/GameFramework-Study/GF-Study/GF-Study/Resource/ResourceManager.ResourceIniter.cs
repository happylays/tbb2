
using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private sealed class ResourceIniter
        {
            private readonly ResourceManager m_ResourceManager;
            private string m_CurrentVariant;

            public GameFrameworkAction ResourceInitComplete;

            public ResourcenIniter(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_CurrentVariant = null;

                ResourceInitComplete = null;
            }

            public void Shutdown()
            {

            }
            public void InitResource(string currentVariant)
            {
                m_CurrentVariant = currentVariant;
                if (m_ResourceManager.m_ResourceHelper == null)
                {
                    throw;
                }
                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(m_ResourceManager.m_ReadOnlyPath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName)), ParsePackageList);

            }

            private void ParsePackageList(string fileUri, byte[] bytes, string errorMessage)
            {
                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes);
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                    {
                        memoryStream = null;
                        char[] header = binaryReader.ReadChar(3);
                        if (header[0] != PackageListHeader[0])
                        {
                            throw;
                        }
                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);
                            m_ResourceManager.m_ApplicableGameVersion = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));
                            m_ResourceManager.m_InternalResourceVersion = binaryReader.ReadInt32();

                            int resourceCount = binaryReader.ReadInt32();
                            string[] names = new string[resourceCount];
                            string[] variants = new string[resourceCount];
                            int[] lengths = new int[resourceCount];
                            Dictionary<string, string[]> dependencyAssetNamesCollection = new Dictionary<string, string[]>();
                            for (int i = 0; i < resouceCount; i++)
                            {
                                names[i] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));

                                variants[i] = null;
                                byte variantLength = binaryReader.ReadByte();
                                if (varintLegnth > 0)
                                {
                                    variants[i] Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(variantLength), encryptBytes));

                                }

                                LoadType loadType = (LoadType)binaryReader.ReadByte();
                                lengths[i] = binaryReader.ReadInt32();
                                int hashCode = BinaryReader.ReadInt32();

                                int assetNamesCount = binaryReader.ReadInt32();
                                string[] assetNames = new string[assetNamesCount];
                                for (int j = 0; j < assetNamesCount; j++)
                                {
                                    assetNames[j] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), Utility.Converter.GetBytes(hashCode)));

                                    int dependencyAssetNamesCount = binaryReader.ReadInt32();
                                    string[] dependencyAssetNames = new string[dependencyAssetNamesCount];
                                    for (int k = 0; k < dependencyAssetNames; k++)
                                    {
                                        dependencyAssetNames[k] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), Utility.Converter.GetBytes(hashCode)));

                                    }

                                    if (variants[i] == null)
                                    {
                                        dependencyAssetNamesCollection.Add(assetNames[j], dependencyAssetNames);
                                    }
                                }

                                if (variants[i] == null)
                                {
                                    ResourceName resourceName = new ResourceName(names[i], variants[i]);
                                    ProcessAssetInfo(resourceName, assetNames);
                                    ProcessResourceInfo(resourceName, loadType, lengths[i]);
                                }

                            }

                            ProcessAssetDependencyInfo(dependencyAssetNamesCollection);

                            ResourceGroup resourceGroupAll = m_ResourceManager.GetResourceGroup(string.Empty);
                            for (int i = 0; i < resourceCount; i++)
                            {
                                resourceGroupAll.AddResource(names[i], variants[i], lengths[i]);
                            }

                            int resourceGroupCount = binaryReader.ReadInt32();
                            for (int i = 0; i < resourceGroupCount; i++)
                            {
                                string groupName = Utiliey.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));
                                resourceGroupCount resourceGroup = m_ResourceManager.GetResourceGroup(groupName);
                                int groupResourceCount = binaryReader.ReadInt32();
                                for (int j = 0; j < groupResourceCount; j++)
                                {
                                    ushort versionIndex = binaryReader.ReadUInt16();
                                    if (versionIndex >= resourceCount)
                                    {

                                    }
                                    resourceGroup.AddResource(names[versionIndex], variants[versionIndex], lengths[versionIndex]);
                                }
                            }
                        }

                    }

                    ResourceInitComplete();
                }
                catch (Exception e)
                {
                    if (e is GameFrameworkExcepton)
                    {
                        throw;
                    }
                    throw;
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
                
            }

            private void ProcessAssetInfo(ResourceName resourceName, string[] assetNames)
            {
                foreach (string assetName in assetNames)
                {
                    m_ResourceManager.m_AssetInfos.Add(assetName, new AssetInfo(assetName, resourceName));
                }
            }

            private void ProcessAssetDependencyInfo(Dictionary<string, string[]> dependencyAssetNamesCollection)
            {
                foreach (KeyValuePair<string, string[]> dependencyAssetNamesCollectionItem in dependencyAssetNamesCollection)
                {
                    List<string> dependencyAssetNames = new List<string>();
                    List<string> scateredDependencyAssetNames = new List<string>();
                    foreach (string dependencyAssetName in dependencyAssetNamesCollectionItem.Value)
                    {
                        AssetInfo? ProcessAssetInfo = m_ResourceManager.GetAssetInfo(dependencyAssetName);
                        if (assetInfo.HasValue)
                        {
                            dependencyAssetNames.Add(dependencyAssetName);
                        }
                        else
                        {
                            scateredDependencyAssetNames.Add(dependencyAssetName);
                        }
                    }
                    m_ResourceManager.m_AssetDependencyInfos.Add(dependencyAssetNamesCollectionItem.Key, new AssetDependencyInfo(dependencyAssetNamesCollectionItem.Key, dependencyAssetNames.ToArray()));
                }
            }

            private void ProcessResourceInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                m_ResourceManager.m_ResourceInfos.Add(resourceName, new ResourceInfo(resourceName, loadType));
            }
        }
    }
}