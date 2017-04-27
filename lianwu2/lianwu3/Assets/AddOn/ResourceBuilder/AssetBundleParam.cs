
public class AssetBundlePath
{
	/// <summary>
	/// 通用路径
	/// </summary>
	public static string ResBundleDir
	{
		get
		{
			return "Res/";
		}
	}

    public static string ResExtendBundleDir
    {
        get { return "Res_Extend/"; }
    }

	/// <summary>
	/// UI相关路径
	/// </summary>
	public static string DLLPrefabAssetDir
	{
		get
		{
			return "Assets/Prefab/Dll/";
		}
	}
	public static string UIAssetRootPath
	{
		get 
		{ 
			return ResBundleDir + "UI/"; 
		}
	}

    public static string UIExtendAssetRootPath
    {
        get
        {
            return ResExtendBundleDir + "UI/";
        }
    }

    public static string PrefabAssetRootPath
    {
        get
        {
            return ResBundleDir + "Prefab/";
        }
    }
	public static string UIWndAssetDir
	{
		get
		{
			return "Assets/Scenes/UI_New/";
		}
	}
	public static string UIWndBasicAssetDir
	{
		get
		{
			return "Assets/Scenes/UI_Basic/";
		}
	}
	public static string SpecialTexBasicAssetDir
	{
		get
		{
			return "Assets/Art_new/UI/texture/special_texture_basic/";
		}
	}
	public static string PrefabBasicAssetDir
	{
		get
		{
			return "Assets/Prefab_dynamic/Prefab_ui_basic/";
		}
	}
	public static string SpecialTexAssetDir
	{
		get
		{
			return "Assets/Art_new/UI/texture/special_texture/";
		}
	}
	public static string PrefabAssetDir
	{
		get
		{
			return "Assets/Prefab_dynamic/Prefab_ui/";
		}
	}

    public static string DynamicPrefabBasicAssetDir
    {
        get
        {
            return "Assets/Prefab_dynamic/Prefab_basic/";
        }
    }

    public static string DynamicPrefabAssetDir
    {
        get
        {
            return "Assets/Prefab_dynamic/Prefab/";
        }
    }

	public static string ArtUIDir
	{
		get
		{
			return "Assets/Art_new/UI/";
		}
	}

	/// <summary>
	/// Shader
	/// </summary>
	public static string ShaderSrcDir
	{
		get
		{
			return "Assets/shaders/";
		}
	}

	public static string ShaderAssetbundlePath
	{
		get { return ResBundleDir; }
	}

	/// <summary>
	/// Materials
	/// </summary>
	public static string MaterialsAssetPath
	{
		get { return "Assets/Add On Resource/Characters/Models/"; }
	}

	public static string MaterialsbundlePath
	{
		get { return ResBundleDir + "Materials/"; }
	}

	public static string BadgesAssetPath
	{
		get { return "Assets/Add On Resource/Characters/Badge/"; }
	}

	public static string BadgesBundlePath
	{
		get { return ResBundleDir + "Effect/"; }
	}

	public static string EnchantEffectAssetPath
	{
		get { return "Assets/Add On Resource/EnchantEffect/";}
	}

	public static string EnchantEffectBundlePath
	{
		get { return BadgesBundlePath + "EnchantEffect/"; }
	}

	public static string EnchantLightMaterialAssetPath
	{
		get { return EnchantEffectAssetPath + "LightMaterial/"; }
	}

	public static string EnchantLightMaterialBundlePath
	{
		get { return EnchantEffectBundlePath + "LightMaterial/"; }
	}

	/// <summary>
	/// Shader
	/// </summary>
	public static string ShaderExtentdSrcDir
	{
		get
		{
			return "Assets/shaders_extend/";
		}
	}

	public static string ShaderExtendAssetbundlePath
	{
		get { return ResBundleDir + "ShaderExtend/"; }
    }

	public static string UIPrefabAssetDir
	{
		get
		{
			return "Assets/UIPrefab/";
		}
	}

	public static string UIPrefabBasicAssetDir
	{
		get
		{
			return "Assets/UIPrefab/UI_Basic/";
		}
	}

	public static string UIPrefabNewAssetDir
	{
		get
		{
			return "Assets/UIPrefab/UI_New/";
		}
	}

    #region Dynamic_Primitive
#if PACKAGE_DYNAMICDOWNLOAD
    public static string SpecialTexBasicPrimitiveAssetDir
    {
        get { return "Assets/Art_new/UI/texture/special_texture_basic/special_texture_basic_primitive"; }
    }

    public static string SpecialTexPrimitiveAssetDir
    {
        get { return "Assets/Art_new/UI/texture/special_texture/special_texture_primitive"; }
    }  
#endif
    #endregion
}