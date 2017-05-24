using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class GenerateStage {

#if !PACKAGE_BASIC
    static string StageSrcDir = "Assets/Art_new/Scenes/Stages/";

    public static string GetStageSrcDir()
    {
        return StageSrcDir;
    }
#endif

    static string StageBasicSrcDir = "Assets/Art_new/Scenes/Stages_basic/";

#if PACKAGE_DYNAMICDOWNLOAD
    static string StageBasicPrimiviteSrcDir = "Assets/Art_new/Scenes/Stages_basic/Stages_basic_primitive/";
    static string StagePrimiviteSrcDir = "Assets/Art_new/Scenes/Stages/Stages_primitive/";
#endif

    static string StageRootDir = "Assets/Art_new/Scenes/";

	[MenuItem("Resource Generator/GenerateResource/Stage")]
	static void Generate_Stage()
	{
		GenerateResource.BuildAllShader("all");

        List<string> dirList = new List<string>();
        dirList.Add(StageBasicSrcDir);
#if !PACKAGE_BASIC
        dirList.Add(StageSrcDir);
#endif
        ProcessAll(dirList);
        GenerateResource.PopAllShader();
	}

    [MenuItem("Resource Generator/GenerateResource/Select Stage")]
	static void Generate_SelectedStage()
    {
        string tagpath = EditorUtility.OpenFilePanel("", Application.dataPath + "/Art_new/Scenes/", "unity");
        if (string.IsNullOrEmpty(tagpath) && !tagpath.Contains(StageRootDir))
            return;
        tagpath = "Assets" + tagpath.Replace(Application.dataPath, "");

        ProceStage_Dir(tagpath);
    }

    public static string GetStageBasicSrcDir()
    {
        return StageBasicSrcDir;
    }

	public static string StageAssetbundlePath
	{
		get{ return GenerateResource.ResAssetbundleDir + "Scenes/"; } 
	}

    public static string StageExtendAssetbundlePath
    {
        get { return AssetBundlePath.ResExtendBundleDir + "Scenes/"; }
    }

    static void ProcessAll(List<string> dirPathList)
    {
        List<string> fileList = new List<string>();

        foreach (string dir in dirPathList)
        {
            if (Directory.Exists(dir))
            {
                BuildStageSceneList(dir, fileList);
            }
            else
            {
                Debug.Log("path is not exist: " + dir);
            }
        }

        foreach (string filePath in fileList)
        {
            ProceStage_Dir(filePath);
        }
    }

    static void BuildStageSceneList(string dirPath, List<string> fileList)
    {
        string[] fileArr = Directory.GetFiles(dirPath, "*.unity");
        foreach (string filePath in fileArr)
        {
            fileList.Add(filePath);
        }

        string[] dirArr = Directory.GetDirectories(dirPath);
        foreach (string dir in dirArr)
        {
            if (!dir.Contains("/."))
            {
                BuildStageSceneList(dir, fileList);
            }
        }
    }

    static void ProceStage_Dir(string filePath)
    {
        string parentPath = GetStagePath(filePath);
        if (!Directory.Exists(parentPath))
        {
            Directory.CreateDirectory(parentPath);
        }

        filePath = filePath.Replace('\\', '/');
        string stageName = filePath.Substring(filePath.LastIndexOf('/') + 1);
		stageName = stageName.Replace(".unity", ".scene");

		if (!BuildAssetBundle.IsLegalAsset(stageName))
		{
			Debug.LogError("Generate stage warning, asset name is not all lower," + filePath);
			EditorUtility.DisplayDialog("Error", "Generate stage warning, asset name is not all lower,Please try again!" + filePath, "OK");
			return;
		}

        BuildPipeline.PushAssetDependencies();
		BuildPipeline.BuildStreamedSceneAssetBundle(new string[] { filePath }, parentPath + stageName, EditorUserBuildSettings.activeBuildTarget);
        BuildPipeline.PopAssetDependencies();
    }

    static string GetStagePath(string stagePath)
    {
        string parentPath = StageAssetbundlePath;

#if PACKAGE_DYNAMICDOWNLOAD
        if (stagePath.Replace('\\', '/').Contains(StageBasicPrimiviteSrcDir)
        ||  stagePath.Replace('\\', '/').Contains(StagePrimiviteSrcDir))
        {
            parentPath = StageExtendAssetbundlePath;
        }
#endif

        return parentPath;
    }
}
