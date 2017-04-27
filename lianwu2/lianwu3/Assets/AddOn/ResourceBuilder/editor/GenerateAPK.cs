using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class GenerateAPK {
	
	// Add menu item to the menu.
	[MenuItem ("GameObject/Generate APK")]
	static void GenerateAPKFunction () 
	{
		string[] Args = System.Environment.GetCommandLineArgs();
		Debug.Log(Args.Length.ToString());
		string iconname = "";
		string key = "";
		string keyaliasName = "";
		string keyaliasPass = "";
		string keystorePass = "";
		for( int i = 0 ; i < Args.Length ; i++ )
		{
			if( Args[i].Contains("-PARTERICON") && i+1<Args.Length )
			{
				iconname = Args[i+1];
			}
			if( Args[i].Contains("-KEYFILE") && i+1<Args.Length )
			{
				key = Args[i+1];
			}
			if( Args[i].Contains("-KEYALIASNAME") && i+1<Args.Length )
			{
				keyaliasName= Args[i+1];
			}	
			if( Args[i].Contains("-KEYALIASPASS") && i+1<Args.Length )
			{
				keyaliasPass = Args[i+1];
			}	
			if( Args[i].Contains("-KEYSTOREPASS") && i+1<Args.Length )
			{
				keystorePass = Args[i+1];
			}
			Debug.Log(Args[i]);
		}
		
		
		for( int i = 0 ; i < Args.Length ; i++ )
		{

			Debug.Log(Args[i]);
		}	
		string ICON_path = "Assets\\Art_new\\UI\\icon\\" + iconname + ".png";
		Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath(ICON_path,typeof(Texture2D));
		Texture2D[] ts = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Android);
		for(int i = 0;i<ts.Length;i++)
		{
			ts[i] = t;
		}
		
		
		//set quality
		string[] names = QualitySettings.names;
		for( int i = 0;i<names.Length;i++ )
		{
			if( names[i] == "Good" )
			{
				QualitySettings.SetQualityLevel(i);
			}
		}
		QualitySettings.pixelLightCount = 1;
		QualitySettings.blendWeights = BlendWeights.TwoBones;
		QualitySettings.vSyncCount = 0;
		QualitySettings.antiAliasing = 0;
		
		
		PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android,ts);
		PlayerSettings.Android.keyaliasName = keyaliasName;//"lianwu";
		PlayerSettings.Android.keyaliasPass = keyaliasPass;//"lianwu20130624";
		PlayerSettings.Android.keystorePass = keystorePass;//"DDLE_LW_20130624";
		PlayerSettings.Android.keystoreName = key;
		PlayerSettings.Android.useAPKExpansionFiles = false;
		PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.Auto;
		string[] scenes = 
		{ 
			"Assets/Scenes/logo.unity"
			,"Assets/Scenes/GameStart.unity"
			,"Assets/Scenes/EmptyScene.unity"
		};
		GenerateAndroidAsset._GenerateAndroidAsset();
		EditorUserBuildSettings.androidBuildSubtarget = AndroidBuildSubtarget.ETC;
		BuildPipeline.BuildPlayer(scenes,"AutoBuilder.APK",BuildTarget.Android,BuildOptions.None);
	}
	
	// Validate the menu item.
	// The item will be disabled if this function returns false.
	[MenuItem ("GameObject/Generate APK", true)]
	static bool ValidateGenerateAPKFunction () {
		return false;
	}
		
}