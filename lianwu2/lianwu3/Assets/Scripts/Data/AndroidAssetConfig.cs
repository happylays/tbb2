using System.Collections.Generic;
using LoveDance.Client.Common;

namespace LoveDance.Client.Data.AndroidAssetConfig
{
	public class AndroidAssetConfig
	{
		private static Dictionary<string, byte> mDicAsset = new Dictionary<string, byte>();

		public static void Load(byte[] bytes)
		{
			XQFileStream fs = new XQFileStream();
			fs.Open(bytes);

			if(fs.IsOpen())
			{
				int count = fs.ReadInt();
				string assetName = null;
				for (int i = 0; i < count; ++i)
				{
					assetName = fs.ReadString();
					mDicAsset[assetName] = 0;
				}
			}

			fs.Close();
		}

		public static bool HasAsset(string assetName)
		{
			return mDicAsset.ContainsKey(assetName);
		}
	}
}

