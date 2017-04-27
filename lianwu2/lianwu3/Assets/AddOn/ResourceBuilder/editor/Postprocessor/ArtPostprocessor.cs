using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// 用于控制Assets/Art_new/FBX/下的资源导入选项
/// </summary>
public class ArtPostprocessor : AssetPostprocessor
{
	private string mFBXPath = "Assets/Art_new/FBX/";

	void OnPreprocessTexture()
	{
		if (assetPath.Contains(mFBXPath))
		{
			TextureImporter textureImporter = (TextureImporter)assetImporter;

			textureImporter.textureType = TextureImporterType.Advanced;
			textureImporter.isReadable = false;
			textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
			textureImporter.generateCubemap = TextureImporterGenerateCubemap.None;

			textureImporter.lightmap = false;
			textureImporter.normalmap = false;
			textureImporter.grayscaleToAlpha = false;
			textureImporter.linearTexture = true;

			textureImporter.mipmapEnabled = false;

			//textureImporter.wrapMode = TextureWrapMode.Repeat;
			textureImporter.filterMode = FilterMode.Trilinear;
			textureImporter.anisoLevel = 4;
		}
	}

	void OnPreprocessModel()
	{
		if (assetPath.Contains(mFBXPath))
		{
			ModelImporter modelImporter = (ModelImporter)assetImporter;

			modelImporter.globalScale = 1;
			modelImporter.meshCompression = ModelImporterMeshCompression.Off;
			modelImporter.isReadable = false;
			modelImporter.optimizeMesh = true;
			modelImporter.addCollider = false;
			//modelImporter.swapUVChannels = false;
			//modelImporter.generateSecondaryUV = false;

			modelImporter.normalImportMode = ModelImporterTangentSpaceMode.Calculate;
			modelImporter.tangentImportMode = ModelImporterTangentSpaceMode.Calculate;
			modelImporter.normalSmoothingAngle = 60;
			modelImporter.splitTangentsAcrossSeams = true;

			//modelImporter.importMaterials = false;
		}
	}
}
