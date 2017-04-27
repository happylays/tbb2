using System.Collections.Generic;

//资源打包相关
namespace LoveDance.Client.Common
{
	/// <summary>
	/// 资源类型
	/// </summary>
	public enum AssetBundleType : byte
	{
		Texture = 0,	//贴图
		Sound = 1,		//音频
		Pre = 2,		//预设
		Material = 3,		//材质
		Animation = 4,	//动画
		Shd = 5,		//着色器
		Model = 6,		//模型
		Scene = 7,		//场景文件
		Script = 8,		//脚本
		ScriptDLL = 9,	//DLL脚本
		Asset = 10,		//Asset文件，如NavMesh

		Max,
	}

	/// <summary>
	/// 该枚举只是为了兼容老版本资源而存在，只有用文件后缀名
	/// </summary>
	//public enum AssetBundleSuffix
	//{
	//    Uit = AssetBundleType.Texture,
	//    Auc = AssetBundleType.Sound,
	//    Pre = AssetBundleType.Prefab,
	//    Matt = AssetBundleType.Material,
	//    Ani = AssetBundleType.Animation,
	//    Shd = AssetBundleType.Shader,
	//    Fbx = AssetBundleType.FBX,
	//    Unity = AssetBundleType.Unity,
	//    Spt = AssetBundleType.Script,
	//    Sptd = AssetBundleType.ScriptDLL,
	//    Ast = AssetBundleType.Asset,

	//    Max = AssetBundleType.Max,
	//}

	public class DependencyAsset
	{
		public string AssetName = "";
		public string AssetPath = "";
		public string AssetSuffix = "";
		public AssetBundleType AssetType = AssetBundleType.Max;
		public int Depth = 0;
		public bool IsLeaf = false;	//true-叶子节点
		public HashSet<string> ParentNodeSet = new HashSet<string>();	//所有父节点
	}
}