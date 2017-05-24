using UnityEditor;
using UnityEngine;

/// <summary>
/// 单独编译all.shd文件
/// </summary>
public class GenerateAllShader
{
	[MenuItem("Resource Generator/GenerateResource/All Shader")]
	static void Generate_Atlas()
	{
		GenerateResource.BuildAllShader("all");
		Debug.Log("Finish Generate Shader");
	}
}