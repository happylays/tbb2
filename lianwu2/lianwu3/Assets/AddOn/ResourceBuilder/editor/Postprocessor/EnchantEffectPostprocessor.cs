using UnityEngine;
using System.Collections;
using UnityEditor;

public class EnchantEffectPostprocessor : AssetPostprocessor
{
	private string mEnchantPath = "Assets/Add On Resource/EnchantEffect/";

	void OnPreprocessTexture()
	{
		if (assetPath.Contains(mEnchantPath))
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

			textureImporter.wrapMode = TextureWrapMode.Repeat;
			textureImporter.filterMode = FilterMode.Bilinear;
			textureImporter.anisoLevel = 1;
		}
	}

	void OnPostprocessTexture(Texture2D texture)
	{
		if (assetPath.Contains(mEnchantPath))
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

	void OnPreprocessModel()
	{
		if (assetPath.Contains(mEnchantPath))
		{
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
