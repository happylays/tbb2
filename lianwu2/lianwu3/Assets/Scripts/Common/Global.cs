using UnityEngine;
//using LoveDance.Client.Network.Friend;
using LoveDance.Client.Logic;
//using LoveDance.Client.Logic.Tips;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
//using LoveDance.Client.Data.Tips;
using System.Collections.Generic;
using System.Text;
//using LoveDance.Client.Data.Item;
//using LoveDance.Client.Data;
using System;
//using LoveDance.Client.Data.Setting;
//using LoveDance.Client.Network.Item;
//using LoveDance.Client.Data.AndroidAssetConfig;

public class GlobalFunc
{
	public static void SetFont(GameObject go)
	{
		UILabel[] allLabel = go.GetComponentsInChildren<UILabel>(true);
		int allLabelLength = allLabel.Length;
		for (int i = 0; i < allLabelLength; ++i)
		{
			UILabel label = allLabel[i];
			if (label != null)
			{
				if (label.font == null || label.font.name == "pr_ui-font")
				{
					label.font = GlobalValue.s_UIFont;
				}
			}
			else
			{
				Debug.LogError("GlobalFunc SetFont failed.UILabel can not be null.");
			}
		}
	}

	/**设置相机的clearFlags**/
	public static void SetCameraClearFlags(GameObject go, CameraClearFlags srcFlags, CameraClearFlags destFlags)
	{
		UICamera[] allUIcam = go.GetComponentsInChildren<UICamera>(true);
		int uiCamLength = allUIcam.Length;
		for (int i = 0; i < uiCamLength; ++i)
		{
			UICamera uicam = allUIcam[i];
			if (uicam != null)
			{
				Camera cam = uicam.gameObject.GetComponent<Camera>();
				if (cam != null && cam.clearFlags == srcFlags)
				{
					cam.clearFlags = destFlags;
				}
			}
		}
	}

    public static Vector3 SpriteScale(UIAtlas uiAtlas, string spriteName)
    {
        Vector3 spriteScale = Vector3.one;

        if (uiAtlas != null && spriteName.Length > 0)
        {
            UIAtlas.Sprite atlasSprite = uiAtlas.GetSprite(spriteName);
            if (atlasSprite != null)
            {
                Rect rect = atlasSprite.outer;
                spriteScale = new Vector3(rect.width, rect.height, 1f);
            }
            else
            {
                Debug.LogError("Sprite: " + spriteName + " not exist in atlas: " + uiAtlas.name);
            }
        }

        return spriteScale;
    }


	public static bool ExistInFile(string dir, string assetName)
	{
		bool exist = false;
#if UNITY_ANDROID
		exist = AndroidAssetConfig.HasAsset(assetName);
#else
		exist = System.IO.File.Exists(dir + assetName);
#endif
		return exist;
	}
	
	/// <summary>
	/// 计算系统字文字内容实际宽度
	/// </summary>
	/// <param name="str">文字内容</param>
	/// <param name="fontWidth">字体宽度</param>
	/// <returns>实际宽度</returns>
	public static float GetStringWidth(string str, float fontWidth)
	{
		return fontWidth * GetStringPrintedSize(str).x;
	}

	/// <summary>
	/// 计算系统字文字内容打印大小
	/// </summary>
	/// <param name="str">文字内容</param>
	/// <returns>打印大小</returns>
	public static Vector2 GetStringPrintedSize(string str)
	{
		if(string.IsNullOrEmpty(str))
		{
			return Vector2.zero;
		}

		return GlobalValue.s_UIFont.CalculatePrintedSize(str, false, UIFont.SymbolStyle.None);
	}

	public static void SetGameLayer(GameObject go, GameLayer layer)
	{
		Transform[] childs = go.GetComponentsInChildren<Transform>();
		if (childs != null)
		{
			for (int i = 0; i < childs.Length; i++)
			{
				if (childs[i] != null)
				{
					childs[i].gameObject.layer = (int)layer;
				}
			}
		}
	}

}


public class GlobalValue
{
    public static UIFont s_UIFont = null;

#if UNITY_ANDROID && !UNITY_EDITOR
	public static IntPtr s_DBitmapClazz;
#endif
}

public class ThirdPlatformFlag
{
    // For all platform 
    public static bool s_bSwitchPlayer = false;
	public static string s_PUID = "";
}
