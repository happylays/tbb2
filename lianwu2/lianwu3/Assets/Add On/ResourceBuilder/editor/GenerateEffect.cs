using UnityEngine;
using System.IO;
using UnityEditor;

public class GenerateEffect  {
	
	[MenuItem("Resource Generator/GenerateResource/Effect")]
	static void GenerateSpecialEffect()
	{
		GenerateResource.BuildAllShader("all");
		if ( !Directory.Exists( EffectAssetbundlePath ) )
		{
			Directory.CreateDirectory( EffectAssetbundlePath );
		}

		string Specialeffects_dir = "Assets/Add On Resource/Characters/Badge";
		ProcSpecialEffectDir(Specialeffects_dir) ;

		Specialeffects_dir = "Assets/Add On Resource/Scenes";
		ProcSpecialEffectDir(Specialeffects_dir);

		Specialeffects_dir = "Assets/Add On Resource/UI";
		ProcSpecialEffectDir(Specialeffects_dir);

		GenerateResource.PopAllShader();

		GenerateEnchantEffect.GenerateAllEnchantEffect();
	}

	public static string EffectAssetbundlePath
	{
		get { return GenerateResource.ResAssetbundleDir + "Effect/";} 
	}
	
	static void ProcSpecialEffectDir(string pathName)
	{
		string[] existingMaterials = Directory.GetFiles(pathName,"*.prefab");
		foreach ( string str in existingMaterials )
		{
			//proc Prefab
			ProcSpecialEffectAssetBundle(str) ;
		}
		string[] existingDirtory = Directory.GetDirectories(pathName);
		foreach ( string str in existingDirtory )
		{
			if( !str.Contains("/.") )
			{
				ProcSpecialEffectDir(str);			
			}
		}		
	}
	
	static void ProcSpecialEffectAssetBundle(string strPrefabPath)
	{
		Object saveObject = (Object)AssetDatabase.LoadMainAssetAtPath(strPrefabPath);
		if(saveObject != null)
		{
			if (!BuildAssetBundle.IsLegalAsset(saveObject.name))
			{
				Debug.LogError("Generate special effect error, asset name is not all lower," + strPrefabPath);
				EditorUtility.DisplayDialog("Error", "Generate special effect error, asset name is not all lower,Please try again!" + strPrefabPath, "OK");
				return;
			}

			string path = EffectAssetbundlePath + saveObject.name + ".pre";
			BuildPipeline.PushAssetDependencies();

			BuildAssetBundle.Build(saveObject, null, path, true);

			BuildPipeline.PopAssetDependencies();
		}
	}

	[MenuItem("Resource Generator/GenerateResource/Select Effects")]
	static void GenerateSelectEffects()
	{
		
		if (!Directory.Exists(EffectAssetbundlePath))
		{
			Directory.CreateDirectory(EffectAssetbundlePath);
		}

		string Specialeffects_dir = "Assets/Add On Resource/Characters/Badge/";
		string tagpath = EditorUtility.OpenFolderPanel("", Application.dataPath + "/Add On Resource/Characters/Badge/", "");
		if (string.IsNullOrEmpty(tagpath) && !tagpath.Contains(Specialeffects_dir))
			return;

		tagpath = "Assets" + tagpath.Replace(Application.dataPath, "");

		GenerateResource.BuildAllShader("all");
		ProcSpecialEffectDir(tagpath);

		GenerateResource.PopAllShader();

		//GenerateEnchantEffect.GenerateAllEnchantEffect();
	}
}
