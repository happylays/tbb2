using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class GenerateAtlas {
	
	[MenuItem("Resource Generator/GenerateResource/Atlas")]
	static void Generate_Atlas()
	{
		if ( !Directory.Exists( UIAtlasAssetbundlePath ) )
		{
			Directory.CreateDirectory( UIAtlasAssetbundlePath );
		}

		List<string> atlasDirList = new List<string>();
        atlasDirList.Add(AtlasBasicSrcDir);
#if !PACKAGE_BASIC
        atlasDirList.Add(AtlasSrcDir);
#endif
        GenerateResource.BuildAllShader("all");
        ProcAtlas_Dir(atlasDirList);
		GenerateResource.PopAllShader();
	}

#if !PACKAGE_BASIC
	static string AtlasSrcDir = "Assets/Art_new/UI/prefab/icon_prefab/";
#endif
    static string AtlasBasicSrcDir = "Assets/Art_new/UI/prefab/icon_prefab_basic/";

	public static string UIAtlasAssetbundlePath
	{
		get{ return GenerateResource.ResAssetbundleDir + "UI/Atlas/"; }
	}
	
	static void ProcAtlas_Dir(List<string> dirPathList)
	{
        List<string> fileList = new List<string>();

        foreach (string dirPath in dirPathList)
        {
            if (Directory.Exists(dirPath))
            {
                BuildAtlasList(dirPath, fileList);
            }
            else
            {
                Debug.Log("path is not exist: " + dirPath);
            }
        }

        ProcAtlas_Files(fileList);
	}

	static void ProcAtlas_Files(List<string> fileList)
	{
		Dictionary<string, Object> atlasMap = new Dictionary<string, Object>();
		string[] atlasArr = fileList.ToArray();
		foreach ( string atlasPath in atlasArr )
		{
			if( !atlasMap.ContainsKey( atlasPath ) )
			{
				atlasMap.Add( atlasPath, AssetDatabase.LoadAssetAtPath( atlasPath, typeof(UIAtlas) ) );
			}
		}
			
		foreach ( KeyValuePair<string, Object> atlkvp in atlasMap )
		{
			if (!BuildAssetBundle.IsLegalAsset(atlkvp.Value.name))
			{
				Debug.LogError("Generate atlas error, asset name is not all lower," + atlkvp.Key);
				EditorUtility.DisplayDialog("Error", "Generate atlas error, asset name is not all lower, Please try again!" + atlkvp.Key, "OK");
				return;
			}

			string path = UIAtlasAssetbundlePath + atlkvp.Value.name + ".atl";
			BuildPipeline.PushAssetDependencies();

			BuildAssetBundle.Build(atlkvp.Value, null, path, true);

			BuildPipeline.PopAssetDependencies();
		}
	}

	static void BuildAtlasList(string dirPath, List<string> fileList)
	{
		string[] fileArr = Directory.GetFiles( dirPath, "*.prefab" );
		foreach ( string filePath in fileArr )
		{
			fileList.Add( filePath );
		}

		string[] dirArr = Directory.GetDirectories( dirPath );
		foreach ( string dir in dirArr )
		{
			if( !dir.Contains( "/." ) )
			{
				BuildAtlasList( dir, fileList );
			}
		}
	}
	
}
