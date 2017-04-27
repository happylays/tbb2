using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class GenerateUI
{
	//[MenuItem("Resource Generator/GenerateResource/UI")]
	static void GenerateAllUIScene()
	{
		if ( !Directory.Exists( UIWndAssetbundlePath ) )
		{
			Directory.CreateDirectory( UIWndAssetbundlePath );
		}

		if ( !Directory.Exists( UITextureAssetbundlePath ) )
		{
			Directory.CreateDirectory( UITextureAssetbundlePath );
		}

		if ( !Directory.Exists( UISoundAssetbundlePath ) )
		{
			Directory.CreateDirectory( UISoundAssetbundlePath );
		}

		List<string> uiWndSrcDirList = new List<string>();
        List<string> uiTxtSrcDirList = new List<string>();
        uiWndSrcDirList.Add(UIWndBasicSrcDir);
		uiWndSrcDirList.Add(UIWndSrcDir);
        uiTxtSrcDirList.Add(SpecialTexBasicSrcDir); 
#if !PACKAGE_BASIC
        uiTxtSrcDirList.Add(SpecialTexSrcDir);
#endif
        GenerateResource.BuildAllShader("all");
        ProcUIScene_All(uiTxtSrcDirList, uiWndSrcDirList);
		GenerateResource.PopAllShader();	
	}

	//[MenuItem( "Resource Generator/GenerateResource/Select UI" )]
	//static void GenerateSelectUIScene()
	//{
	//    if ( !Directory.Exists( UIWndAssetbundlePath ) )
	//    {
	//        Directory.CreateDirectory( UIWndAssetbundlePath );
	//    }

	//    if ( !Directory.Exists( UITextureAssetbundlePath ) )
	//    {
	//        Directory.CreateDirectory( UITextureAssetbundlePath );
	//    }

	//    if ( !Directory.Exists( UISoundAssetbundlePath ) )
	//    {
	//        Directory.CreateDirectory( UISoundAssetbundlePath );
	//    }

	//    Object selectUI = Selection.activeObject;
	//    if ( selectUI != null )
	//    {
	//        string assetPath = AssetDatabase.GetAssetPath( selectUI );
	//        if ( assetPath.EndsWith( ".unity" ) )
	//        {
	//            GenerateResource.BuildAllShader( "all" );
	//            ProcUIScene_Single( SpecialTexSrcDir, assetPath );
	//            GenerateResource.PopAllShader();
	//        }
	//    }
	//}

#if !PACKAGE_BASIC
    static string SpecialTexSrcDir = "Assets/Art_new/UI/texture/special_texture/";
#endif
	static string UIWndSrcDir = "Assets/Scenes/UI_New/";	
    static string UIWndBasicSrcDir = "Assets/Scenes/UI_Basic/";
    static string SpecialTexBasicSrcDir = "Assets/Art_new/UI/texture/special_texture_basic/";

    public static string GetUIWndBasicSrcDir()
    {
        return UIWndBasicSrcDir;
    }

	public static string GetUIWndSrcDir()
	{
		return UIWndSrcDir;
	}

	public static string UIWndAssetbundlePath
	{
		get { return GenerateResource.ResAssetbundleDir + "UI/Wnd/"; }
	}

	public static string UITextureAssetbundlePath
	{
		get { return GenerateResource.ResAssetbundleDir + "UI/Texture/"; }
	}

	public static string UISoundAssetbundlePath
	{
		get { return GenerateResource.ResAssetbundleDir + "UI/Sound/"; }
	}
	
	static void ProcUIScene_All(List<string> specialDirList, List<string> uisceneDirList)
	{
		List<string> specialList = new List<string>();
		List<string> uisceneList = new List<string>();

        foreach (string specialDir in specialDirList)
        {
            if (Directory.Exists(specialDir))
            {
                BuildSpecialTexList(specialDir, specialList);
            }
            else
            {
                Debug.Log("path is not exist: " + specialDir);
            }
        }

        foreach (string uisceneDir in uisceneDirList)
        {
            if (Directory.Exists(uisceneDir))
            {
                BuildUISceneList(uisceneDir, uisceneList);
            }
            else
            {
                Debug.Log("path is not exist: " + uisceneDir);
            }
        }
		
		ProcUIScene( specialList, uisceneList );
	}

	static void ProcUIScene_Single(string specialDir, string uiscenePath)
	{
		List<string> specialList = new List<string>();

		if ( Directory.Exists( specialDir ) )
		{
			BuildSpecialTexList( specialDir, specialList );
		}
		else
		{
			Debug.Log( "path is not exist: " + specialDir );
		}

		ProcUIScene( specialList, uiscenePath );
	}

	static void ProcUIScene(List<string> specialList, List<string> uisceneList)
	{
		UIConfig uiConfig = new UIConfig();
		Dictionary<string, Object> textureMap = new Dictionary<string, Object>();
		Dictionary<string, Object> soundMap = new Dictionary<string, Object>();
		string TextureNomallPath = "";
#if PACKAGE_BASIC
		//添加小包过滤操作
		Dictionary<string, string> deleteTextureMap = new Dictionary<string, string>();
		Dictionary<string, string> deleteSoundMap = new Dictionary<string, string>();
		List<string> notDelTextureList = new List<string>();
		List<string> notDelSoundList = new List<string>();
#endif

		string[] textureArr = specialList.ToArray();
		foreach ( string texturePath in textureArr )
		{
			if( !textureMap.ContainsKey( texturePath ) )
			{
				textureMap.Add( texturePath, AssetDatabase.LoadAssetAtPath( texturePath, typeof( Texture2D ) ) );
			}
		}

		foreach ( string uiscenePath in uisceneList )
		{
			string sectionName = uiscenePath.Substring( uiscenePath.LastIndexOf( '/' ) + 1 );
			sectionName = sectionName.Replace( ".unity", "" );

			string[] sceneDepArr = AssetDatabase.GetDependencies( new string[1]{ uiscenePath } );
			foreach ( string sceneDepPath in sceneDepArr )
			{
				if ( IsImageFile( sceneDepPath ) )
				{
					if ( !textureMap.ContainsKey( sceneDepPath ) )
					{
						textureMap.Add( sceneDepPath, AssetDatabase.LoadAssetAtPath( sceneDepPath, typeof( Texture2D ) ) );
					}
#if PACKAGE_BASIC
					//添加小包过滤操作
					if (uiscenePath.Contains(UIWndSrcDir) && !deleteTextureMap.ContainsKey(sceneDepPath))
					{
						if (specialList.Contains(sceneDepPath))
						{//Check it again.
							Debug.LogError("Special texture dictionary has same texture,TexturePath=" + sceneDepPath + ".UIScenePath=" + uiscenePath);
						}
						else
						{
							deleteTextureMap.Add(sceneDepPath, null);
						}
					}
					if (uiscenePath.Contains(UIWndBasicSrcDir) && !notDelTextureList.Contains(sceneDepPath))
					{
						notDelTextureList.Add(sceneDepPath);
					}
#endif
					if( textureMap[sceneDepPath].name != "Nomall" ) 
					{
						uiConfig.AddConfig( sectionName, UISubSection.UI_Texture, textureMap[sceneDepPath].name );
					}
				}
				else if ( IsSoundFile( sceneDepPath ) )
				{
					if ( !soundMap.ContainsKey( sceneDepPath ) )
					{
						soundMap.Add( sceneDepPath, AssetDatabase.LoadAssetAtPath( sceneDepPath, typeof( AudioClip ) ) );
					}
#if PACKAGE_BASIC
					//添加小包过滤操作
					if (uiscenePath.Contains(UIWndSrcDir) && !deleteSoundMap.ContainsKey(sceneDepPath))
					{
						deleteSoundMap.Add(sceneDepPath, null);
					}
					if (uiscenePath.Contains(UIWndBasicSrcDir) && !notDelSoundList.Contains(sceneDepPath))
					{
						notDelSoundList.Add(sceneDepPath);
					}
#endif
					uiConfig.AddConfig( sectionName, UISubSection.UI_Sound, soundMap[sceneDepPath].name );
				}
			}
		}

#if PACKAGE_BASIC
		//过滤小包中用到的资源
		foreach (string notPath in notDelTextureList)
		{
			if (deleteTextureMap.ContainsKey(notPath))
			{
				deleteTextureMap.Remove(notPath);
			}
		}
		foreach (string notPath in notDelSoundList)
		{
			if (deleteSoundMap.ContainsKey(notPath))
			{
				deleteSoundMap.Remove(notPath);
			}
		}
#endif
		
		BuildPipeline.PushAssetDependencies();

		foreach ( KeyValuePair<string, Object> texturekvp in textureMap )
		{
			string path = UITextureAssetbundlePath + texturekvp.Value.name + ".uit";
			
			if( texturekvp.Value.name == "Nomall")
			{
				TextureNomallPath = path;
			}
#if PACKAGE_BASIC
			//添加小包过滤操作
			if (deleteTextureMap.ContainsKey(texturekvp.Key))
			{
				deleteTextureMap[texturekvp.Key] = path;
			}
#endif
			BuildAssetBundle.Build(texturekvp.Value, null, path, true);
		}
			
		foreach ( KeyValuePair<string, Object> soundkvp in soundMap )
		{
			string path = UISoundAssetbundlePath + soundkvp.Value.name + ".auc";
#if PACKAGE_BASIC
			//添加小包过滤操作
			if (deleteSoundMap.ContainsKey(soundkvp.Key))
			{
				deleteSoundMap[soundkvp.Key] = path;
			}
#endif
			BuildAssetBundle.Build(soundkvp.Value, null, path, true);
		}
		
		string storePath = UIWndAssetbundlePath + "UI.unity";
		BuildPipeline.BuildStreamedSceneAssetBundle( uisceneList.ToArray(), storePath, EditorUserBuildSettings.activeBuildTarget );
		BuildPipeline.PopAssetDependencies();

		uiConfig.SaveConfig( UIWndAssetbundlePath + "UIConfig.txt" );
		
		if( !string.IsNullOrEmpty(TextureNomallPath) )
		{
			File.Delete(TextureNomallPath);
		}
#if PACKAGE_BASIC
		//添加小包过滤操作
		foreach (string path in deleteTextureMap.Values)
		{
			if (string.IsNullOrEmpty(path))
			{
				Debug.LogError("Build basic package error, can not delete unuse texture");
			}
			else
			{
				File.Delete(path);
			}
		}
		foreach (string path in deleteSoundMap.Values)
		{
			if (string.IsNullOrEmpty(path))
			{
				Debug.LogError("Build basic package error, can not delete unuse sound");
			}
			else
			{
				File.Delete(path);
			}
		}
#endif
	}

	static void ProcUIScene(List<string> specialList, string uiscenePath)
	{
		Dictionary<string, Object> textureMap = new Dictionary<string, Object>();
		Dictionary<string, Object> soundMap = new Dictionary<string, Object>();
		string TextureNomallPath = "";

		string[] textureArr = specialList.ToArray();
		foreach ( string texturePath in textureArr )
		{
			if ( !textureMap.ContainsKey( texturePath ) )
			{
				textureMap.Add( texturePath, AssetDatabase.LoadAssetAtPath( texturePath, typeof( Texture2D ) ) );
			}
		}

		string uisceneName = uiscenePath.Substring( uiscenePath.LastIndexOf( '/' ) + 1 );
		string sectionName = uisceneName.Replace( ".unity", "" );

		string configPath = UIWndAssetbundlePath + "UIConfig.txt";
		UIConfig uiConfig = new UIConfig();
		uiConfig.LoadConfig( configPath, configPath );
		uiConfig.ClearSection( sectionName );

		string[] sceneDepArr = AssetDatabase.GetDependencies( new string[1] { uiscenePath } );
		foreach ( string sceneDepPath in sceneDepArr )
		{
			if ( IsImageFile( sceneDepPath ) )
			{
				if ( !textureMap.ContainsKey( sceneDepPath ) )
				{
					textureMap.Add( sceneDepPath, AssetDatabase.LoadAssetAtPath( sceneDepPath, typeof( Texture2D ) ) );
				}
				if( textureMap[sceneDepPath].name != "Nomall" ) 
				{
					uiConfig.AddConfig( sectionName, UISubSection.UI_Texture, textureMap[sceneDepPath].name );
				}
			}
			else if ( IsSoundFile( sceneDepPath ) )
			{
				if ( !soundMap.ContainsKey( sceneDepPath ) )
				{
					soundMap.Add( sceneDepPath, AssetDatabase.LoadAssetAtPath( sceneDepPath, typeof( AudioClip ) ) );
				}

				uiConfig.AddConfig( sectionName, UISubSection.UI_Sound, soundMap[sceneDepPath].name );
			}
		}

		BuildPipeline.PushAssetDependencies();

		foreach ( KeyValuePair<string, Object> texturekvp in textureMap )
		{
			string path = UITextureAssetbundlePath + texturekvp.Value.name + ".uit";
			if( texturekvp.Value.name == "Nomall")
			{
				TextureNomallPath = path;
			}

			BuildAssetBundle.Build(texturekvp.Value, null, path, true);
		}

		foreach ( KeyValuePair<string, Object> soundkvp in soundMap )
		{
			string path = UISoundAssetbundlePath + soundkvp.Value.name + ".auc";
			BuildAssetBundle.Build(soundkvp.Value, null, path, true);
		}

		string storePath = UIWndAssetbundlePath + uisceneName;
		BuildPipeline.PushAssetDependencies();
		BuildPipeline.BuildStreamedSceneAssetBundle( new string[] { uiscenePath }, storePath, EditorUserBuildSettings.activeBuildTarget );
		BuildPipeline.PopAssetDependencies();

		BuildPipeline.PopAssetDependencies();

		uiConfig.SaveConfig( configPath );

		if( !string.IsNullOrEmpty(TextureNomallPath) )
		{
			File.Delete(TextureNomallPath);
		}
	}

	public static void BuildSpecialTexList(string dirPath, List<string> fileList)
	{
		string[] fileArr = Directory.GetFiles( dirPath );
		foreach ( string filePath in fileArr )
		{
			if( IsImageFile( filePath ) )
			{
				fileList.Add( filePath );
			}
		}

		string[] dirArr = Directory.GetDirectories( dirPath );
		foreach ( string dir in dirArr )
		{
			if( !dir.Contains( "/." ) )
			{
				BuildSpecialTexList( dir, fileList );
			}
		}
	}

	public static void BuildUISceneList(string dirPath, List<string> fileList)
	{
		string[] fileArr = Directory.GetFiles( dirPath, "*.unity" );
		foreach ( string filePath in fileArr )
		{
			fileList.Add( filePath );
		}

		string[] dirArr = Directory.GetDirectories( dirPath );
		foreach ( string dir in dirArr )
		{
			if( !dir.Contains( "/." ) )
			{
				BuildUISceneList( dir, fileList );
			}
		}
	}

	public static bool IsImageFile(string filePath)
	{
		string extension = filePath.Substring( filePath.Length - 4 ).ToLower();
		if( extension == ".png" || extension == ".bmp" || extension == ".tga" || extension == ".psd" || extension == ".dds" || extension == ".jpg")
		{
			return true;
		}

		return false;
	}

	static bool IsSoundFile(string filePath)
	{
		string extension = filePath.Substring( filePath.Length - 4 ).ToLower();
		if( extension == ".ogg" || extension == ".wav" || extension == ".mp3" )
		{
			return true;
		}

		return false;
	}
}
