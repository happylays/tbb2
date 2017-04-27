using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 用于处理UI资源;
/// </summary>
public class UIPostprocessor : AssetPostprocessor
{
	void OnPreprocessTexture()
	{
		if (assetPath.Contains("Assets/Art_new/UI/"))
		{//UI 贴图处理;
			if (assetPath.Contains("Assets/Art_new/UI/texture/ui_texture/") || assetPath.Contains("Assets/Art_new/UI/texture/icon_texture/"))
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

				textureImporter.wrapMode = TextureWrapMode.Clamp;
				textureImporter.filterMode = FilterMode.Trilinear;
				textureImporter.anisoLevel = 4;
			}
			else if (assetPath.Contains("Assets/Art_new/UI/texture/ui_texture_bgz/"))
			{//不规则贴图处理;
				TextureImporter textureImporter = (TextureImporter)assetImporter;
				if (textureImporter.textureType == TextureImporterType.Advanced)
				{
					textureImporter.isReadable = false;
				}
			}
		}
	}

	void OnPostprocessTexture(Texture2D texture)
	{
		if (assetPath.Contains("Assets/Art_new/UI/"))
		{//UI 贴图处理;
			if (assetPath.Contains("Assets/Art_new/UI/texture/ui_texture/") || assetPath.Contains("Assets/Art_new/UI/texture/icon_texture/"))
			{
				TextureImporter textureImporter = (TextureImporter)assetImporter;
				
				textureImporter.maxTextureSize = texture.width;
				textureImporter.textureFormat = TextureImporterFormat.ETC_RGB4;

				textureImporter.ClearPlatformTextureSettings("Android");
				textureImporter.ClearPlatformTextureSettings("iPhone");
				textureImporter.SetPlatformTextureSettings("Android", texture.width, TextureImporterFormat.ETC_RGB4, 50);
				textureImporter.SetPlatformTextureSettings("iPhone", texture.width, TextureImporterFormat.PVRTC_RGB4, 50);
			}
		}
	}

	void OnPreprocessModel()
	{
		if (assetPath.Contains("Assets/Art_new/UI/"))
		{//UI 模型;
			ModelImporter modelImporter = (ModelImporter)assetImporter;

			modelImporter.globalScale = 1;
			modelImporter.meshCompression = ModelImporterMeshCompression.Off;
			modelImporter.isReadable = false;
			modelImporter.optimizeMesh = true;
			modelImporter.addCollider = false;
			modelImporter.swapUVChannels = false;
			modelImporter.generateSecondaryUV = false;

			modelImporter.normalImportMode = ModelImporterTangentSpaceMode.Calculate;
			modelImporter.tangentImportMode = ModelImporterTangentSpaceMode.Calculate;
			modelImporter.normalSmoothingAngle = 60;
			modelImporter.splitTangentsAcrossSeams = true;

			modelImporter.importMaterials = false;
		}
	}
}
