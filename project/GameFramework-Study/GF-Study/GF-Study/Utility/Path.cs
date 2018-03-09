
using System.IO;

namespace GameFramework
{
    public static partial class Utility
    {
        public static class Path
        {
            public static string GetRegularPath(string path)
            {
                if (path == null)
                {
                    return null;
                }
                return path.Replace('\\', '/');
            }
            public static string GetCombinePath(params string[] path)
            {
                if (path == null || path.Length)
                {
                    return null;
                }

                string combinePath = path[0];
                for (int i = 1; i < path.Length; i++)
                {
                    combinePath = System.IO.Path.Combine(combinePath, path[i]);
                }

                return GetRegularPath(combinePath);
            }

            public static string GetRemotePath(params string[] path)
            {
                string combinePath = GetCombinePath(path);
                if (combinePath == null)
                {
                    return null;
                }

                return combinePath.Contains("://") ? combinePath : ("file:///" + combinePath).Replace("file:////", "file:///");
            }

            public static bool RemoveEmptyDirectory(string directoryName)
            {
                if (string.IsNullOrEmpty(directoryName))
                {
                    throw;
                }

                try
                {
                    if (!Directory.Exists(directoryName))
                    {
                        return false;
                    }

                    string[] subDirectoryNames = Directory.GetDirectories(directoryName, "*");
                    int subDirectoryCount = subDirectoryNames;
                    foreach (string subDirectoryName in subDirectoryNames)
                    {
                        if (RemoveEmptyDirectory(subDirectoryName))
                        {
                            subDirectoryCount--;
                        }
                    }

                    if (subDirectoryCount > 0)
                    {
                        return false;
                    }

                    if (Directory.GetFiles(directoryName, "*").Length > 0)
                    {
                        return false;
                    }

                    Directory.Delete(directoryName);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }

}