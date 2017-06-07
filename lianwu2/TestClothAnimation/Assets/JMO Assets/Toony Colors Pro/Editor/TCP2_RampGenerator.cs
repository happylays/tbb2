// Toony Colors Pro+Mobile 2
// (c) 2014-2016 Jean Moreno

using UnityEngine;
using UnityEditor;

// Utility to generate ramp textures

public class TCP2_RampGenerator : EditorWindow
{
	[MenuItem(TCP2_Menu.MENU_PATH + "Ramp Generator", false, 500)]
	static void OpenTool()
	{
		GetWindowTCP2();
	}

	static private TCP2_RampGenerator GetWindowTCP2()
	{
		TCP2_RampGenerator window = EditorWindow.GetWindow<TCP2_RampGenerator>(true, "TCP2 : Ramp Generator", true);
		window.minSize = new Vector2(352f, 132f);
		window.maxSize = new Vector2(352f, 132f);
		return window;
	}

	//--------------------------------------------------------------------------------------------------
	// INTERFACE

#if UNITY_EDITOR_WIN
	private const string OUTPUT_FOLDER = "\\Textures\\Custom Ramps\\";
#else
	private const string OUTPUT_FOLDER = "/Textures/Custom Ramps/";
#endif

	[SerializeField]
	private Gradient mGradient;
	private int textureWidth = 256;

	//--------------------------------------------------------------------------------------------------

	void OnEnable() { Init(); }

	void Init()
	{
		mGradient = new Gradient();
		mGradient.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.black, 0.49f), new GradientColorKey(Color.white, 0.51f) };
		mGradient.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) };
	}

	void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		TCP2_GUI.HeaderBig("TCP 2 - RAMP GENERATOR");
		TCP2_GUI.HelpButton("Ramp Generator");
		EditorGUILayout.EndHorizontal();
		TCP2_GUI.Separator();

		SerializedObject so = new SerializedObject(this);
		SerializedProperty sp = so.FindProperty("mGradient");
		EditorGUILayout.PropertyField(sp, GUIContent.none);

		textureWidth = EditorGUILayout.IntField("TEXTURE SIZE:", textureWidth);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("64", EditorStyles.miniButtonLeft)) textureWidth = 64;
		if (GUILayout.Button("128", EditorStyles.miniButtonMid)) textureWidth = 128;
		if (GUILayout.Button("256", EditorStyles.miniButtonMid)) textureWidth = 256;
		if (GUILayout.Button("512", EditorStyles.miniButtonMid)) textureWidth = 512;
		if (GUILayout.Button("1024", EditorStyles.miniButtonRight)) textureWidth = 1024;
		EditorGUILayout.EndHorizontal();

		if (GUI.changed)
		{
			so.ApplyModifiedProperties();
			mGradient.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) };
		}

		GUILayout.Space(8f);
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("GENERATE", GUILayout.Width(120f), GUILayout.Height(34f)))
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Generated Ramp", "TCP2_CustomRamp", "png", "Save Generated Ramp");
			GenerateTexture(path);
		}
		EditorGUILayout.EndHorizontal();
	}

	//--------------------------------------------------------------------------------------------------

	private void GenerateTexture(string path)
	{
		if(string.IsNullOrEmpty(path))
			return;

		Texture2D ramp = new Texture2D(textureWidth, 4, TextureFormat.RGB24, true, false);
		for (int x = 0; x < textureWidth; x++)
		{
			float delta = Mathf.Clamp01(x / (float)textureWidth);
			Color col = mGradient.Evaluate(delta);
			ramp.SetPixel(x, 0, col);
			ramp.SetPixel(x, 1, col);
			ramp.SetPixel(x, 2, col);
			ramp.SetPixel(x, 3, col);
		}
		ramp.Apply(true);
		byte[] png = ramp.EncodeToPNG();
		Object.DestroyImmediate(ramp);

		string systemPath = Application.dataPath + "/" + path.Substring(7);
		System.IO.File.WriteAllBytes(systemPath, png);

		AssetDatabase.ImportAsset(path);
		TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
		ti.wrapMode = TextureWrapMode.Clamp;
#if UNITY_5_5_OR_NEWER
		ti.textureCompression = TextureImporterCompression.Uncompressed;
		ti.alphaSource = TextureImporterAlphaSource.None;
#else
		ti.textureFormat = TextureImporterFormat.RGB24;
#endif
		ti.SaveAndReimport();
	}
}
