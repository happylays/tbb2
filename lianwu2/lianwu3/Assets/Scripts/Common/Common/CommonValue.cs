using UnityEngine;

namespace LoveDance.Client.Common
{
	/// <summary>
	/// 用于存储初始化后不会变化的常量、变量;
	/// </summary>
	public class CommonValue
	{
		static string s_StorageRootDir = null;		//路径
		static CallbackReturn<string> s_StorageRootDirCB = null;	//路径回调

		static string s_StorageDir = null;
		static CallbackReturn<string> s_StorageDirCB = null;

		static Phone_OS s_PhoneOS = Phone_OS.None;	//游戏运行平台
		static CallbackReturn<Phone_OS> s_PhoneOSCB = null;


        static NetworkType s_NetWorkType = NetworkType.None;//当前网络环境;
        static CallbackReturn<string> s_StorageNetWorkTypeCB = null;

		static string s_AppDir = null;
		static string s_CaptureRelativeDir = null;	//截图相对路径
		static string s_CaptureAbsoluteDir = null;	//截图绝对路径

		static Package_Type sPackageType = Package_Type.Invalid;	// 获取大小包版本
		static CallbackReturn<bool> s_IsNewPackageCB = null;		// 用于获取是否大包

		static string s_PackageSize = null;							//扩展包大小
		static CallbackReturn<string> s_PackageSizeCB = null;

		static Version_Type s_ChargeVersion = Version_Type.Free;	//付费版本
		static CallbackReturn<Version_Type> s_ChargeVersionCB = null;

		static AudioType s_AudioType = AudioType.OGGVORBIS;			//音频文件格式
		static CallbackReturn<AudioType> s_AudioTypeCB = null;

		static string s_AudioExtensionType = null;
		static CallbackReturn<string> s_AudioExtensionTypeCB = null;

		static Version_IOS_Type s_IOSVersion = Version_IOS_Type.Official;	//IOS版本
		static CallbackReturn<Version_IOS_Type> s_IOSVersionCB = null;

		static Callback s_ClearFontCB = null;	//清除字体
		static CallbackReturn<Material, GameObject> s_GetAtlasMatCB = null;	//获取Atlas（非引用类型）材质球

		private static string s_ServerSettingDownloadIP = null;						// 图片下载服务器IP
		private static CallbackReturn<string> s_ServerSettingDownloadIPCB = null;	// 图片下载服务器IP写入回调

		private static int s_ServerSettingDownloadPort = 0;							// 图片下载服务器Port
		private static CallbackReturn<int> s_ServerSettingDownloadPortCB = null;	// 图片下载服务器Port写入回调

		private static string s_ThirdDataServerId = null;							// 平台ServerID
		private static CallbackReturn<string> s_ThirdDataServerIdCB = null;			// 平台ServerID写入回调

		public static string StorageRootDir
		{
			get
			{
				if (s_StorageRootDir == null)
				{
					if (s_StorageRootDirCB == null)
					{
						Debug.LogError("CommonSetting StorageRootDirCB can not be null.");
					}
					else
					{
						s_StorageRootDir = s_StorageRootDirCB();
					}
				}
				return s_StorageRootDir;
			}
		}
		
        public static NetworkType NetWorkType
        {
            set
            {
                s_NetWorkType = value;
            }
            get
            {
                if (s_NetWorkType == NetworkType.None)
                {
                    if (s_StorageNetWorkTypeCB == null)
                    {
                        Debug.LogError("CommonSetting s_StorageNetWorkTypeCB can not be null.");
                    }
                    else
                    {
                        s_NetWorkType = (NetworkType)System.Convert.ToInt32(s_StorageNetWorkTypeCB());
                    }
                }
                return s_NetWorkType;
            }
        }

		public static string StorageDir
		{
			get
			{
				if (s_StorageDir == null)
				{
					if (s_StorageDirCB == null)
					{
						Debug.LogError("CommonSetting StorageDirCB can not be null.");
					}
					else
					{
						s_StorageDir = s_StorageDirCB();
					}
				}

				return s_StorageDir;
			}
		}

		public static Phone_OS PhoneOS
		{
			get
			{
				if(s_PhoneOS == Phone_OS.None)
				{
					if(s_PhoneOSCB == null)
					{
						////Debug.LogError("CommonSetting PhoneOSCB can not be null.");
					}
					else
					{
						s_PhoneOS = s_PhoneOSCB();
					}
				}

				return s_PhoneOS;
			}
		}

        public static Platform_Define Platform_Define
        {
            get
            {
                Platform_Define define = Platform_Define.OTHER;
#if UNITY_IPHONE
                define = Platform_Define.IPHONE;
#elif UNITY_ANDROID
                define = Platform_Define.ANDROID;
#endif

                return define;
            }
        }

		public static string AppDir
		{
			get
			{
				if (s_AppDir == null)
				{
					string strDir = "";

					if (Application.platform == RuntimePlatform.WindowsWebPlayer
						|| Application.platform == RuntimePlatform.OSXWebPlayer
						|| Application.platform == RuntimePlatform.OSXPlayer
						|| Application.platform == RuntimePlatform.IPhonePlayer)
					{
						strDir = Application.persistentDataPath + "/";
					}
					else if (Application.platform == RuntimePlatform.Android)
					{
						//strDir = StorageDir + "/xuanqu/lwts/";
                        strDir = Application.persistentDataPath + "/";
					}
					else
					{
						strDir = Application.dataPath + "/../";
					}

					s_AppDir = strDir;
				}

				return s_AppDir;
			}
		}

		public static string CaptureRelativeDir
		{
			get
			{
				if (s_CaptureRelativeDir == null)
				{
					string strDir = "screenshot/";

					if(Application.platform == RuntimePlatform.Android)
					{
						strDir = "lwscreenshot/";
					}
					else if(Application.platform == RuntimePlatform.IPhonePlayer)
					{
						strDir = "LWTS_Share/";
					}

					s_CaptureRelativeDir = strDir;
				}

				return s_CaptureRelativeDir;
			}
		}

		public static string CaptureAbsoluteDir
		{
			get
			{
				if (s_CaptureAbsoluteDir == null)
				{
					string strDir = "";

					if (Application.platform == RuntimePlatform.Android)
					{
						strDir = StorageRootDir + "/" + CaptureRelativeDir;
					}
					else
					{
						strDir = AppDir + CaptureRelativeDir;
					}

					s_CaptureAbsoluteDir = strDir;
				}

				return s_CaptureAbsoluteDir;
			}
		}

		public static CallbackReturn<string> StorageRootDirCB
		{
			set
			{
				s_StorageRootDirCB = value;
			}
		}

		public static CallbackReturn<string> StorageDirCB
		{
			set
			{
				s_StorageDirCB = value;
			}
		}

		public static CallbackReturn<Phone_OS> PhoneOSCB
		{
			set
			{
				s_PhoneOSCB = value;
			}
		}
        public static CallbackReturn<string> StorageNetWorkTypeCB
        {
            set
            {
                s_StorageNetWorkTypeCB = value;
            }
        }

		public static Callback ClearFontCB
		{
			get
			{
				return s_ClearFontCB;
			}
			set
			{
				s_ClearFontCB = value;
			}
		}

		public static CallbackReturn<Material, GameObject> GetAtlasMatCB
		{
			get
			{
				return s_GetAtlasMatCB;
			}
			set
			{
				s_GetAtlasMatCB = value;
			}
		}

		public static string ResDir
		{
			get
			{
				string dir = "";

				if (Application.platform == RuntimePlatform.Android)
				{
					dir = AppDir + "res/";
				}
				else if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					dir = AppDir + "Res/";
				}
				else
				{
					dir = AppDir + "Res/";
				}

				return dir;
			}
		}

		public static string InResDir
		{
			get
			{
				string dir = "";

				if (Application.platform == RuntimePlatform.Android)
				{
					dir = Application.dataPath + "!/assets/"+ "res_unity/";
				}
				else if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					dir = Application.dataPath + "/../" + "Res/";
				}
				else
				{
					dir = AppDir + "Res/";
				}

				return dir;
			}
		}

        public static string NetResDir
        {
            get
            {
                return "";
            }
        }

		public static string ConfDir
		{
			get
			{
				string dir = "";

				if (Application.platform == RuntimePlatform.Android)
				{
					dir = AppDir + "config/";
				}
				else if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					dir = AppDir + "Config/";
				}
				else
				{
					dir = CommonValue.AppDir + "Config/";
				}

				return dir;
			}
		}

		public static CallbackReturn<bool> IsNewPackage
		{
			set
			{
				s_IsNewPackageCB = value;
			}
		}

		public static Package_Type s_PackageType
		{
			get
			{
				if (sPackageType == Package_Type.Invalid)
				{
					if (s_IsNewPackageCB != null)
					{
						sPackageType = s_IsNewPackageCB() ? Package_Type.New : Package_Type.All;
					}
				}
				return sPackageType;
			}
			set
			{
				sPackageType = value;
			}
		}

		public static string PackageSize
		{
			get
			{
				if (s_PackageSize == null)
				{
					if (s_PackageSizeCB == null)
					{
						Debug.LogError("PackageSize function can not be null.");
					}
					else
					{
						s_PackageSize = s_PackageSizeCB();
					}
				}

				return s_PackageSize;
			}
		}

		public static CallbackReturn<string> PackageSizeCB
		{
			set
			{
				s_PackageSizeCB = value;
			}
		}

		public static Version_Type ChargeVersion
		{
			get
			{
				if (s_ChargeVersionCB == null)
				{
					Debug.LogError("ChargeVersion function can not be null.");
				}
				else
				{
					s_ChargeVersion = s_ChargeVersionCB();
				}

				return s_ChargeVersion;
			}
		}

		public static CallbackReturn<Version_Type> ChargeVersionCB
		{
			set
			{
				s_ChargeVersionCB = value;
			}
		}

		public static AudioType AudioType
		{
			get
			{
				if (s_AudioTypeCB == null)
				{
					Debug.LogError("ChargeVersion function can not be null.");
				}
				else
				{
					s_AudioType = s_AudioTypeCB();
				}

				return s_AudioType;
			}
		}

		public static CallbackReturn<AudioType> AudioTypeCB
		{
			set
			{
				s_AudioTypeCB = value;
			}
		}

		public static string AudioExtensionType
		{
			get
			{
				if (s_AudioExtensionTypeCB == null)
				{
					Debug.LogError("AudioExtensionType function can not be null.");
				}
				else
				{
					s_AudioExtensionType = s_AudioExtensionTypeCB();
				}

				return s_AudioExtensionType;
			}
		}

		public static CallbackReturn<string> AudioExtensionTypeCB
		{
			set
			{
				s_AudioExtensionTypeCB = value;
			}
		}

		public static Version_IOS_Type IOSVersion
		{
			get
			{
				if (s_IOSVersionCB == null)
				{
					Debug.LogError("IOSVersion function can not be null.");
				}
				else
				{
					s_IOSVersion = s_IOSVersionCB();
				}

				return s_IOSVersion;
			}
		}

		public static CallbackReturn<Version_IOS_Type> IOSVersionCB
		{
			set
			{
				s_IOSVersionCB = value;
			}
		}

		public static string ServerSettingDownloadIP
		{
			get
			{
				if (s_ServerSettingDownloadIP == null)
				{
					if (s_ServerSettingDownloadIPCB == null)
					{
						Debug.LogError("ServerSettingDownloadIP function can not be null.");
					}
					else
					{
						s_ServerSettingDownloadIP = s_ServerSettingDownloadIPCB();
					}
				}

				return s_ServerSettingDownloadIP;
			}
		}

		public static CallbackReturn<string> ServerSettingDownloadIPCB
		{
			set
			{
				s_ServerSettingDownloadIPCB = value;
			}
		}

		public static int ServerSettingDownloadPort
		{
			get
			{
				if (s_ServerSettingDownloadPort == 0)
				{
					if (s_ServerSettingDownloadPortCB == null)
					{
						Debug.LogError("ServerSettingDownloadPort function can not be null.");
					}
					else
					{
						s_ServerSettingDownloadPort = s_ServerSettingDownloadPortCB();
					}
				}

				return s_ServerSettingDownloadPort;
			}
		}

		public static CallbackReturn<int> ServerSettingDownloadPortCB
		{
			set
			{
				s_ServerSettingDownloadPortCB = value;
			}
		}


		public static string ThirdDataServerId
		{
			get
			{
				if (s_ThirdDataServerId == null)
				{
					if (s_ThirdDataServerIdCB == null)
					{
						Debug.LogError("ThirdDataServerId function can not be null.");
					}
					else
					{
						s_ThirdDataServerId = s_ThirdDataServerIdCB();
					}
				}

				return s_ThirdDataServerId;
			}
		}

		public static CallbackReturn<string> ThirdDataServerIdCB
		{
			set
			{
				s_ThirdDataServerIdCB = value;
			}
		}


		public static string LogDir { get { return AppDir + "Logs/"; } }

        public static string StaDataDir { get { return ResDir + "StaticData/"; } }
        public static string InStaDataDir { get { return InResDir + "StaticData/"; } }
        public static string NetStaDataDir { get { return NetResDir + "StaticData/"; } }

		public static string ClientVerDir { get { return ResDir + "ClientVer/"; } }
		public static string InClientVerDir { get { return InResDir + "ClientVer/"; } }
		public static string ServerVerDir { get { return ResDir + "ServerVer/"; } }
		public static string InServerVerDir { get { return InResDir + "ServerVer/"; } }
		public static string NetVerDir { get { return NetResDir + "ServerVer/"; } }
		public static string ExtraVerDir { get { return ResDir + "ExtraVer/"; } }

        public static string MusicDir { get { return ResDir + "Music/"; } }
        public static string InMusicDir { get { return InResDir + "Music/"; } }
        public static string NetMusicDir { get { return NetResDir + "Music/"; } }

        public static string ShaderDir { get { return ResDir; } }
        public static string InShaderDir { get { return InResDir; } }
        public static string NetShaderDir { get { return NetResDir; } }

        public static string BoneDir { get { return ResDir; } }
        public static string InBoneDir { get { return InResDir; } }
        public static string NetBoneDir { get { return NetResDir; } }
		
        public static string AniDir { get { return ResDir + "Animations/"; } }
		public static string InAniDir { get { return InResDir + "Animations/"; } }
        public static string NetAniDir { get { return NetResDir + "Animations/"; } }

        public static string MaterialDir { get { return ResDir + "Materials/"; } }
        public static string InMaterialDir { get { return InResDir + "Materials/"; } }
        public static string NetMaterialDir { get { return NetResDir + "Materials/"; } }

        public static string EffectDir { get { return ResDir + "Effect/"; } }
        public static string InEffectDir { get { return InResDir + "Effect/"; } }
        public static string NetEffectDir { get { return NetResDir + "Effect/"; } }

		public static string StageDir { get { return ResDir + "Scenes/"; } }
        public static string InStageDir { get { return InResDir + "Scenes/"; } }
        public static string NetStageDir { get { return NetResDir + "Scenes/"; } }

        public static string UITextureDir { get { return ResDir + "UI/Texture/"; } }
        public static string InUITextureDir { get { return InResDir + "UI/Texture/"; } }
        public static string NetUITextureDir { get { return NetResDir + "UI/Texture/"; } }

        public static string UIIconDir { get { return ResDir + "Icon/"; } }
        public static string InUIIconDir { get { return InResDir + "Icon/"; } }
        public static string NetUIIconDir { get { return NetResDir + "Icon/"; } }

        public static string UIAtlasDir { get { return ResDir + "UI/Atlas/"; } }
        public static string InUIAtlasDir { get { return InResDir + "UI/Atlas/"; } }
        public static string NetUIAtlasDir { get { return NetResDir + "UI/Atlas/"; } }

        public static string ImageDir { get { return AppDir + "picture/"; } }

        public static string UIDir { get { return ResDir + "UI/"; } }
        public static string InUIDir { get { return InResDir + "UI/"; } }
        public static string NetUIDir { get { return NetResDir + "UI/"; } }

        public static string PrefabDir { get { return ResDir + "Prefab/"; } }
        public static string InPrefabDir { get { return InResDir + "Prefab/"; } }
        public static string NetPrefabDir { get { return NetResDir + "Prefab/"; } }

		public static string EnchantEffectDir { get { return "EnchantEffect/"; } }
		public static string EnchantLightDir { get { return "EnchantEffect/LightMaterial/"; } }
        public static string ShaderExtendDir { get { return ResDir + "ShaderExtend/"; } }
        public static string InShaderExtendDir { get { return InResDir + "ShaderExtend/"; } }
        public static string NetShaderExtendDir { get { return NetResDir + "ShaderExtend/"; } }
		public static string StaDataWWWDir { get { return "file://" + StaDataDir; } }

		#region WWWDIR
		public static string MusicWWWDir { get { return "file://" + MusicDir; } }		
		public static string ShaderWWWDir { get { return "file://" + ShaderDir; } }	
		public static string BoneWWWDir { get { return "file://" + BoneDir; } }	
		public static string AniWWWDir { get { return "file://" + AniDir; } }
		public static string MaterialWWWDir { get { return "file://" + MaterialDir; } }		
		public static string EffectWWWDir { get { return "file://" + EffectDir; } }	
		public static string StageWWWDir { get { return "file://" + StageDir; } }	
		public static string UITextureWWWDir { get { return "file://" + UITextureDir; } }
        public static string UIIconWWWDir { get { return "file://" + UIIconDir; } }	
		public static string UIAtlasWWWDir { get { return "file://" + UIAtlasDir; } }	
		public static string ImageWWWDir { get { return "file://" + ImageDir; } }
		public static string UIWWWDir { get { return "file://" + UIDir; } }
		public static string ShaderExtendWWWDir { get { return "file://" + ShaderExtendDir; } }
        public static string PrefabWWWDir { get { return "file://" + PrefabDir; } }
		#endregion

		#region InWWWDIR

		public static string InStaDataWWWDir { get { return GetInWWWDir(InStaDataDir); } }
		public static string InMusicWWWDir { get { return GetInWWWDir(InMusicDir); } }
		public static string InShaderWWWDir { get { return GetInWWWDir(InShaderDir); } }
		public static string InBoneWWWDir { get { return GetInWWWDir(InBoneDir); } }
		public static string InAniWWWDir { get { return GetInWWWDir(InAniDir); } }
		public static string InMaterialWWWDir { get { return GetInWWWDir(InMaterialDir); } }
		public static string InEffectWWWDir { get { return GetInWWWDir(InEffectDir); } }
		public static string InStageWWWDir { get { return GetInWWWDir(InStageDir); } }
		public static string InUITextureWWWDir { get { return GetInWWWDir(InUITextureDir); } }
		public static string InUIIconWWWDir { get { return GetInWWWDir(InUIIconDir); } }
		public static string InUIAtlasWWWDir { get { return GetInWWWDir(InUIAtlasDir); } }
		public static string InUIWWWDir { get { return GetInWWWDir(InUIDir); } }
		public static string InShaderExtendWWWDir { get { return GetInWWWDir(InShaderExtendDir); } }
		public static string InPrefabWWWDir { get { return GetInWWWDir(InPrefabDir); } }

		public static string GetInWWWDir(string dir)
		{
			string newDir = "";

			if (Application.platform == RuntimePlatform.Android)
			{
				newDir = "jar:file://" + dir;
			}
			else
			{
				newDir = "file://" + dir;
			}

			return newDir;
		}

		#endregion
	}
}
