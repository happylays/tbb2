using UnityEngine;
using System.IO;
using UnityEditor;

public class GenerateBone {
	
	[MenuItem("Resource Generator/GenerateResource/Bones")]
	static void GenerateBones()
	{
		ProcBones_Dir( BonesSrcDir );
	}
	
	static string BonesSrcDir = "Assets/Add On Resource/Characters/Bones/";
	public static string BonesAssetbundlePath
	{
		get { return GenerateResource.ResAssetbundleDir; }
	}
	
	static void ProcBones_Dir(string dirPath)
	{
		if( Directory.Exists( dirPath ) )
		{
			string[] fileArr = Directory.GetFiles( dirPath, "*.prefab" );
			foreach ( string filePath in fileArr )
			{
				ProcBones_File( filePath );
			}

			string[] dirArr = Directory.GetDirectories( dirPath );
			foreach ( string dir in dirArr )
			{
				if( !dir.Contains( "/." ) )
				{
					ProcBones_Dir( dir );
				}
			}
		}
		else
		{
			Debug.Log( "path is not exist: " + dirPath );
		}	
	}
	static void ProcBones_File(string filePath)
	{
		Object o = AssetDatabase.LoadMainAssetAtPath( filePath );
		if ( o is GameObject )
		{
			GameObject characterFBX = (GameObject)o;
			string bundleName = characterFBX.name;
	 		Debug.Log( "******* Creating Bones assetbundles for: " + bundleName + " *******" );

			//remove bones;
			GameObject rendererClone = (GameObject)PrefabUtility.InstantiatePrefab( characterFBX );
			Animator at = rendererClone.GetComponent<Animator>();
			if( at != null )
			{
				Object.DestroyImmediate( at );
			}

			Object rendererPrefab = GenerateResource.ReplacePrefab( rendererClone, rendererClone.name );
			string bundleParentPath = BonesAssetbundlePath;
		
			if (!Directory.Exists(bundleParentPath))
			{
				Directory.CreateDirectory(bundleParentPath);
			}

			if (!BuildAssetBundle.IsLegalAsset(bundleName))
			{
				Debug.LogError("Generate bone error, asset name is not all lower," + filePath);
				EditorUtility.DisplayDialog("Error", "Generate bone error, asset name is not all lower,Please try again!" + filePath, "OK");
				return;
			}

			BuildAssetBundle.Build(rendererPrefab, null, bundleParentPath + bundleName + ".res");
			Debug.Log("Saved Bones to '" + bundleParentPath + bundleName + ".res");
	
			AssetDatabase.DeleteAsset( AssetDatabase.GetAssetPath( rendererPrefab ) );
			GameObject.DestroyImmediate( rendererClone );
		}
	}

}
