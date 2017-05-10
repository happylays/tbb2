using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;
using LoveDance.Client.Common;
using LoveDance.Client.Common.Messengers;

namespace LoveDance.Client.Loader
{
	public class AniState
	{
		public string Name { get; set; }
		public float Speed { get; set; }
		public string Motion { get; set; }
		public List<TranState> Trans = new List<TranState>();
	}

	public class TranState
	{
		public string SrcState { get; set; }
		public string DestState { get; set; }
		public float TranOffset { get; set; }
		public float TranDuration { get; set; }
		public float ExitTime { get; set; }
		public bool IsBoy { get; set; }
	}


	public class AnimationLoader
	{
		AnimationLoader(string strFileName)
		{
			m_strAniName = strFileName;
			s_AllAniLoader.Add(strFileName, this);
		}

		public static string HelloBoy
		{
			get
			{
				return "BOY_HELLO";
			}
		}

		public static string HelloGirl
		{
			get
			{
				return "GIRL_HELLO";
			}
		}

		public static string StandWorldBoy
		{
			get
			{
				return "BOY_WORLD_STAND";
			}
		}

		public static string StandWorldGirl
		{
			get
			{
				return "GIRL_WORLD_STAND";
			}
		}

		public static string DressBoy
		{
			get
			{
				return "BOY_RELOAD";
			}
		}

		public static string DressGirl
		{
			get
			{
				return "GIRL_RELOAD";
			}
		}

		public static string DressBehind
		{
			get
			{
				return "HIPRELOAD";
			}
		}

		public static string StandRoomBoy_A
		{
			get
			{
				return "BOY_STAND_A";
			}
		}

		public static string StandRoomBoy_B
		{
			get
			{
				return "BOY_STAND_B";
			}
		}

		public static string StandRoomBoy_C
		{
			get
			{
				return "BOY_STAND_C";
			}
		}

		public static string StandRoomBoy_D
		{
			get
			{
				return "BOY_STAND_D";
			}
		}

		public static string StandRoomGirl_A
		{
			get
			{
				return "GIRL_STAND_A";
			}
		}

		public static string StandRoomGirl_B
		{
			get
			{
				return "GIRL_STAND_B";
			}
		}

		public static string StandRoomGirl_C
		{
			get
			{
				return "GIRL_STAND_C";
			}
		}

		public static string StandRoomGirl_D
		{
			get
			{
				return "GIRL_STAND_D";
			}
		}

		public static string BoyPrepareDance
		{
			get
			{
				return "A000209";
			}
		}

		public static string GirlPrepareDance
		{
			get
			{
				return "A000212";
			}
		}

		public static string StartDance
		{
			get
			{
				return "A000215";
			}
		}

		public static string MissBoy
		{
			get
			{
				return "BOY_MISS";
			}
		}

		public static string MissGirl
		{
			get
			{
				return "GIRL_MISS";
			}
		}

		public static string LoseBoy
		{
			get
			{
				return "BOY_LOSER";
			}
		}

		public static string LoseGirl
		{
			get
			{
				return "GIRL_LOSER";
			}
		}
		public static string WinBoy
		{
			get
			{
				return "BOY_WIN";
			}
		}

		public static string WinGirl
		{
			get
			{
				return "GIRL_WIN";
			}
		}

		public static string WalkBoy
		{
			get
			{
				return "BOY_WALK";
			}
		}

		public static string WalkGirl
		{
			get
			{
				return "GIRL_WALK";
			}
		}

		public static string BS_WalkBoy
		{
			get
			{
				return "BS_BOY_MOVE";
			}
		}

		public static string BS_WalkGirl
		{
			get
			{
				return "BS_GIRL_MOVE";
			}
		}

		public static string VIP_STAND_BOY_01
		{
			get
			{
				return "VIP_STAND_BOY_01";
			}
		}

		public static string VIP_STAND_BOY_02
		{
			get
			{
				return "VIP_STAND_BOY_02";
			}
		}

		public static string VIP_STAND_BOY_03
		{
			get
			{
				return "VIP_STAND_BOY_03";
			}
		}

		public static string VIP_STAND_GIRL_01
		{
			get
			{
				return "VIP_STAND_GIRL_01";
			}
		}

		public static string VIP_STAND_GIRL_02
		{
			get
			{
				return "VIP_STAND_GIRL_02";
			}
		}

		public static string VIP_STAND_GIRL_03
		{
			get
			{
				return "VIP_STAND_GIRL_03";
			}
		}

		public static string HB_StartDance_Boy
		{
			get
			{
				return "EB_D_STAND";
			}
		}

		public static string HB_StartDance_Girl
		{
			get
			{
				return "EG_D_STAND";
			}
		}

		public static string HB_LoseBoy
		{
			get
			{
				return "EB_D_LOSER";
			}
		}

		public static string HB_LoseGirl
		{
			get
			{
				return "EG_D_LOSER";
			}
		}
		public static string HB_WinBoy
		{
			get
			{
				return "EB_PHOTO_";
			}
		}

		public static string HB_WinGirl
		{
			get
			{
				return "EG_PHOTO_";
			}
		}

		public static string HB_STAND_BOY_01
		{
			get
			{
				return "EB_HEART_STAND_01";
			}
		}

		public static string HB_STAND_BOY_02
		{
			get
			{
				return "EB_HEART_STAND_02";
			}
		}

		public static string HB_STAND_GIRL_01
		{
			get
			{
				return "EG_HEART_STAND_01";
			}
		}

		public static string HB_STAND_GIRL_02
		{
			get
			{
				return "EG_HEART_STAND_02";
			}
		}

		public static string HB_SELECT_BOY_01
		{
			get
			{
				return "EB_HEART_YES_01";
			}
		}

		public static string HB_SELECT_BOY_02
		{
			get
			{
				return "EB_HEART_YES_02";
			}
		}

		public static string HB_SELECT_GIRL_01
		{
			get
			{
				return "EG_HEART_YES_01";
			}
		}

		public static string HB_SELECT_GIRL_02
		{
			get
			{
				return "EG_HEART_YES_02";
			}
		}
		
		public static string BIG_MAMA_NPC_DANCE
		{
			get
			{
				return "DAMA_ALL";
			}
		}

		public static string BS_ROOM_BOY_STAND
		{
			get { return "BS_ROOM_BOY_STAND"; }
		}

		public static string BS_ROOM_GIRL_STAND
		{
			get { return "BS_ROOM_GIRL_STAND"; }
		}

		public static string StudioMultiBoy { get { return ""; } }
		public static string StudioMultiGirl { get { return ""; } }

		public static string HandbookPreviewPoseBoy { get { return "TUJIAN_BOY_01"; } }
		public static string HandbookPreviewPoseGirl { get { return "TUJIAN_GIRL_01"; } }

		// StarShowAni
		public static string StarShow_Male_1 { get { return "MXX_BOY_FIRST"; } }
		public static string StarShow_Male_2 { get { return "MXX_BOY_SECOND"; } }
		public static string StarShow_Male_3 { get { return "MXX_BOY_THIRD"; } }
		public static string StarShow_Female_1 { get { return "MXX_GIRL_FIRST"; } }
		public static string StarShow_Female_2 { get { return "MXX_GIRL_SECOND"; } }
		public static string StarShow_Female_3 { get { return "MXX_GIRL_THIRD"; } }

		public string ResName
		{
			get
			{
				return m_strAniName;
			}
			set
			{
				m_strAniName = value;
			}
		}

		public AnimationClip AniClip
		{
			get
			{
				return m_AniClip;
			}
		}

		public static List<string> DanceAniUnique
		{
			get
			{
				return s_DanceAniUnique;
			}
		}

		public static List<string> DanceAniSequence
		{
			get
			{
				return s_DanceAniSequence;
			}
		}

		public static List<string> DanceAniSequenceBoy
		{
			get
			{
				return s_DanceAniSequenceBoy;
			}
		}

		public static bool WorldAniExist
		{
			get
			{
				return m_WorldAniExist;
			}
		}

		public static bool RoomAniExist
		{
			get
			{
				return m_RoomAniExist;
			}
		}

		public static bool StageAniExist
		{
			get
			{
				return m_StageAniExist;
			}
		}

		public static bool GuideAniExist
		{
			get
			{
				return m_GuideAniExist;
			}
		}

		public static bool WeddingAniExist
		{
			get
			{
				return m_WeddingAniExist;
			}
		}

		public static bool StudioAniExist
		{
			get
			{
				return m_StudioAniExist;
			}
		}

		public static int HB_Win_Index
		{
			get
			{
				return m_HB_Win_Index;
			}
		}

		public static bool RoleCreateAniExist
		{
			get
			{
				return m_RoleCreateAniExist;
			}
		}

		public static bool CeremonyAniExist
		{
			get
			{
				return m_CeremonyAniExist;
			}
		}

		public static bool HandbookAniExist
		{
			get
			{
				return m_HandbookAniExist;
			}
		}
		
		public static bool BigMamaAniExist
		{
			get
			{
				return m_BigMamaAniExist;
			}
		}

		public static bool StarShowAniExist
		{
			get
			{
				return m_StarShowAniExist;
			}
		}

		public static Dictionary<string, AniState> s_AniStates = new Dictionary<string, AniState>();

		static Dictionary<string, AnimationLoader> s_AllAniClips = new Dictionary<string, AnimationLoader>();
		static Dictionary<string, AnimationLoader> s_AllAniLoader = new Dictionary<string, AnimationLoader>();
		static List<string> s_DanceAniSequence = new List<string>();	//Default is girl animation
		static List<string> s_DanceAniSequenceBoy = new List<string>();
		static List<string> s_DanceAniUnique = new List<string>();
		private static List<string> s_StarShowAgentAniList = null;

		string m_strAniName = "";

		AnimationClip m_AniClip = null;
		AssetBundle m_AniAsset = null;
		WWW m_AniWWW = null;
		int m_AssetRefer = 0;

		IEnumerator m_LoadItor = null;

		static bool m_WorldAniExist = false;
		static bool m_RoomAniExist = false;
		static bool m_StageAniExist = false;
		static bool m_GuideAniExist = false;
		static bool m_WeddingAniExist = false;
		static bool m_StudioAniExist = false;
		static bool m_RoleCreateAniExist = false;
		static bool m_CeremonyAniExist = false;
		static bool m_HandbookAniExist = false;
		static bool m_BigMamaAniExist = false;
		static bool m_StarShowAniExist = false;
		
		static int m_HB_Win_Index = 0;

        //动画加载路径
		static string mAssetWWWDir = "";
		static string mAssetDir = "";
		static string mInAssetWWWDir = "";
		static string mInAssetDir = "";
        static string m_netAssetDir = "";//网络路径
		
		/// <summary>
        /// 初始化路径
		/// </summary>
		/// <param name="assetDir"> GlobalValue.s_AniDir</param>
		/// <param name="assetWWWDir">GlobalValue.s_AniWWWDir</param>
		/// <param name="inAssetDir"></param>
		/// <param name="inAssetWWWDir">GlobalValue.s_InAniWWWDir</param>
		public static void InitAniLoader(string assetDir, string assetWWWDir, string inAssetDir, string inAssetWWWDir,string netAssetDir)
		{
			mAssetDir = assetDir;
			mAssetWWWDir = assetWWWDir;
			mInAssetDir = inAssetDir;
			mInAssetWWWDir = inAssetWWWDir;
            m_netAssetDir = netAssetDir;
		}

        private IEnumerator DownLoadAnimation()
        {
            string strName = m_strAniName + ".assetbundle";
            if (WWWDownLoaderConfig.CheckResNeedUpdate(strName))
            {
				DownLoadPack downloadPack = WWWDownLoader.InsertDownLoad(strName, m_netAssetDir + strName, DownLoadAssetType.AssetBundle, null, null, DownLoadOrderType.AfterRunning);
				if (downloadPack != null)
				{
					while (!downloadPack.AssetReady)
					{
						yield return null;
					}
					if (downloadPack.Bundle != null)
					{
						downloadPack.Bundle.Unload(true);
					}
				}
                WWWDownLoader.RemoveDownLoad(strName, null);
            }
            Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
        }

		public IEnumerator LoadAnimation()
		{
            string strName = m_strAniName + ".assetbundle";
			
			if (s_AllAniLoader.ContainsKey(m_strAniName))
			{
				s_AllAniLoader.Remove(m_strAniName);
				s_AllAniClips.Add(m_strAniName, this);
			}

			if (WWWDownLoaderConfig.CheckResNeedUpdate(strName))
			{
				DownLoadPack downloadPack = WWWDownLoader.InsertDownLoad(strName, m_netAssetDir + strName, DownLoadAssetType.AssetBundle, null, null, DownLoadOrderType.AfterRunning);
				if (downloadPack != null)
				{
					while (!downloadPack.AssetReady)
					{
						yield return null;
					}
					m_AniAsset = downloadPack.Bundle;
					if (m_AniAsset != null)
					{
						m_AniClip = (AnimationClip)m_AniAsset.mainAsset;

						if (CommonValue.PhoneOS == Phone_OS.Ios)
						{
							m_AniAsset.Unload(false);
						}
					}
				}
				WWWDownLoader.RemoveDownLoad(strName, null);
			}
			else
			{
				string assetWWWPath = mAssetWWWDir + m_strAniName + ".assetbundle";
				string assetPath = mAssetDir + m_strAniName + ".assetbundle";
				if (!File.Exists(assetPath))
				{
					assetWWWPath = mInAssetWWWDir + m_strAniName + ".assetbundle";
				}

				using (m_AniWWW = new WWW(assetWWWPath))
				{
					while (m_AniWWW != null && !m_AniWWW.isDone)
					{
						yield return null;
					}

					if (m_AniWWW != null)
					{
						if (m_AniWWW.error != null)
						{
							Debug.LogError(m_AniWWW.error);
							Debug.LogError("Animation Load Error! AssetName : " + m_strAniName);
						}
						else
						{
							m_AniAsset = m_AniWWW.assetBundle;
							if (m_AniAsset != null)
							{
								m_AniClip = (AnimationClip)m_AniAsset.mainAsset;
								if (CommonValue.PhoneOS == Phone_OS.Ios)
								{
									m_AniAsset.Unload(false);
								}
							}
						}

						m_AniWWW.Dispose();
						m_AniWWW = null;
					}
				}
			}
            Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
		}

        public IEnumerator DownLoadAnimationSync()
        {
            if (m_LoadItor == null)
            {
                m_LoadItor = DownLoadAnimation();
            }

            while (m_LoadItor.MoveNext())
            {
                yield return null;
            }
        }

		public IEnumerator LoadAnimationSync()
        {
			if (m_LoadItor == null)
			{
				m_LoadItor = LoadAnimation();
			}

            while(m_LoadItor.MoveNext())
            {
                yield return null;
            }
		}


        public static IEnumerator DownLoadStageSceneAnimation(string musicName, SongMode mode)
        {
            List<AnimationLoader> loaderList = GetStageSceneAnimation(mode);

            IEnumerator itor = null;
            foreach (AnimationLoader aniLoader in loaderList)
            {
                if (aniLoader != null)
                {
                    itor = aniLoader.DownLoadAnimationSync();
                    while (itor.MoveNext())
                    {
                        yield return null;
                    }
                    ReleaseAnimationOnDownLoad(aniLoader.ResName);
                }
            }

            itor = LoadDanceAniConfig(musicName, mode);
            while (itor.MoveNext())
            {
                yield return null;
            }

            List<AnimationLoader> listAni = (List<AnimationLoader>)itor.Current;
            for (int i = 0; i < listAni.Count; i++)
            {
                if (listAni[i] != null)
                {
                    itor = listAni[i].DownLoadAnimationSync();
                    while (itor.MoveNext())
                    {
                        yield return null;
                    }
                    ReleaseAnimationOnDownLoad(listAni[i].ResName);
                }
            }
        }


        public static IEnumerator LoadStageSceneAnimation(string musicName, SongMode mode)
        {
            m_StageAniExist = true;

            List<AnimationLoader> loaderList = GetStageSceneAnimation(mode);

            IEnumerator itor = null;
            foreach (AnimationLoader aniLoader in loaderList)
            {
                if (aniLoader != null)
                {
                    itor = aniLoader.LoadAnimationSync();
                    while (itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }

            itor = LoadDanceAniConfig(musicName, mode);
            while (itor.MoveNext())
            {
                yield return null;
            }

            List<AnimationLoader> listAni = (List<AnimationLoader>)itor.Current;
            for (int i = 0; i < listAni.Count; i++)
            {
                if (listAni[i] != null)
                {
                    itor = listAni[i].LoadAnimationSync();
                    while (itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }
        }


        /// <summary>
        /// 获取所有动作Loader
        /// </summary>
        private static List<AnimationLoader> GetStageSceneAnimation(SongMode mode)
        {
            List<AnimationLoader> loaderList = new List<AnimationLoader>();
            loaderList.Add(GetAnimationLoader(AnimationLoader.BoyPrepareDance));
            loaderList.Add(GetAnimationLoader(AnimationLoader.GirlPrepareDance));
            loaderList.Add(GetAnimationLoader(AnimationLoader.StartDance));
            loaderList.Add(GetAnimationLoader(AnimationLoader.HB_StartDance_Boy));
            loaderList.Add(GetAnimationLoader(AnimationLoader.HB_StartDance_Girl));
            loaderList.Add(GetAnimationLoader(AnimationLoader.MissBoy));
            loaderList.Add(GetAnimationLoader(AnimationLoader.MissGirl));

            loaderList.Add(GetAnimationLoader(AnimationLoader.LoseBoy));
            loaderList.Add(GetAnimationLoader(AnimationLoader.LoseGirl));
            loaderList.Add(GetAnimationLoader(AnimationLoader.WinBoy));
            loaderList.Add(GetAnimationLoader(AnimationLoader.WinGirl));
           
            return loaderList;
        }


        static IEnumerator LoadDanceAniConfig(string musicName, SongMode mode)
        {
            s_DanceAniUnique.Clear();
            s_DanceAniSequence.Clear();
            s_DanceAniSequenceBoy.Clear();
            s_AniStates.Clear();

            List<AnimationLoader> listAni = new List<AnimationLoader>();
            string strResPath = m_netAssetDir + "Controller/" + musicName + "/Dance.txt";
            string strName = musicName + "_Dance.txt";
            if (WWWDownLoaderConfig.CheckResNeedUpdate(strName))
            {
                DownLoadPack downloadPack = WWWDownLoader.InsertDownLoad(strName, strResPath, DownLoadAssetType.Text, null, null, DownLoadOrderType.AfterRunning);
                if (downloadPack != null)
                {
                    while (!downloadPack.AssetReady)
                    {
                        yield return null;
                    }

                    using (StreamReader sr = new StreamReader(new MemoryStream(downloadPack.DataBytes), CommonFunc.GetCharsetEncoding()))
                    {
                        listAni = ParseDanceTxt(sr);
                        sr.Close();
                    }
                }
                WWWDownLoader.RemoveDownLoad(strName, null);
            }
            else
            {
                string strFilePath = mAssetDir + "Controller/" + musicName + "/Dance.txt";
                string strInFilePath = mInAssetDir + "Controller/" + musicName + "/Dance.txt";

                string assetWWWPath = CommonFunc.GetCorrectWWWDir(strFilePath, strInFilePath);

                WWW www = null;
                using (www = new WWW(assetWWWPath))
                {
                    while (!www.isDone)
                    {
                        yield return null;
                    }

                    if (www.error != null)
                    {
                        Debug.LogError(www.error);
                        Debug.LogError("AniLoad Error! AssetName : " + assetWWWPath);
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(new MemoryStream(www.bytes), CommonFunc.GetCharsetEncoding()))
                        {
                            listAni = ParseDanceTxt(sr);
                            sr.Close();
                        }
                    }

                    www.Dispose();
                    www = null;
                }
            }
            Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
            yield return listAni;
        }


        static List<AnimationLoader> ParseDanceTxt(StreamReader sr)
        {
            string k = "";
            string v = "";

            AniState aniState = null;
            TranState tranState = null;
            string strStartState = "";

            string strLine = "";
            while ((strLine = sr.ReadLine()) != null)
            {
                strLine = strLine.Trim();
                if (strLine.StartsWith("DefaultState"))
                {
                    StringHelp.DepartString(strLine, ref k, ref v, ":");
                    //TODO
                }

                if (strLine.StartsWith("["))
                {
                    if (aniState != null)
                    {
                        if (s_AniStates.ContainsKey(aniState.Name))
                        {
                            s_AniStates.Remove(aniState.Name);
                        }

                        s_AniStates.Add(aniState.Name, aniState);
                    }

                    aniState = new AniState();
                    aniState.Name = strLine.Substring(1, strLine.Length - 2).Trim();

                    if (aniState.Name.ToUpper() == "DANCE START")
                    {
                        strStartState = aniState.Name;
                    }
                }
                else if (strLine.StartsWith("Speed"))
                {
                    StringHelp.DepartString(strLine, ref k, ref v, ":");
                    aniState.Speed = Convert.ToSingle(v);
                }
                else if (strLine.StartsWith("Motion"))
                {
                    StringHelp.DepartString(strLine, ref k, ref v, ":");
                    aniState.Motion = v;
                }
                else if (strLine.StartsWith("Transition:"))
                {
                    tranState = new TranState();
                    aniState.Trans.Add(tranState);
                }
                else if (strLine.StartsWith("SrcState"))
                {
                    StringHelp.DepartString(strLine, ref k, ref v, ":");
                    tranState.SrcState = v;
                }
                else if (strLine.StartsWith("DestState"))
                {
                    StringHelp.DepartString(strLine, ref k, ref v, ":");
                    tranState.DestState = v;
                }
                else if (strLine.StartsWith("TransitionOffset"))
                {
                    StringHelp.DepartString(strLine, ref k, ref v, ":");
                    tranState.TranOffset = Convert.ToSingle(v);
                }
                else if (strLine.StartsWith("TransitionDuration"))
                {
                    StringHelp.DepartString(strLine, ref k, ref v, ":");
                    tranState.TranDuration = Convert.ToSingle(v);
                }
                else if (strLine.StartsWith("ExitTime"))
                {
                    StringHelp.DepartString(strLine, ref k, ref v, ":");
                    tranState.ExitTime = Convert.ToSingle(v);
                }
                else if (strLine.StartsWith("IsBoy"))
                {
                    StringHelp.DepartString(strLine, ref k, ref v, ":");
                    tranState.IsBoy = Convert.ToInt32(v) == 1 ? true : false;
                }
            }

            sr.Close();

            if (s_AniStates.ContainsKey(aniState.Name))
            {
                s_AniStates.Remove(aniState.Name);
            }

            s_AniStates.Add(aniState.Name, aniState);

            //Girl
            string currState = strStartState;
            List<AnimationLoader> loaderList = BuildAniList(currState, false);
            //Boy
            currState = strStartState;
            List<AnimationLoader> boyLoaderList = BuildAniList(currState, true);
            loaderList.AddRange(boyLoaderList);

            return loaderList;
        }

		public void Release()
		{
			if (m_AniWWW != null)
			{
				m_AniWWW.Dispose();
				m_AniWWW = null;
			}

			if (m_AniClip != null)
			{
				if (CommonValue.PhoneOS == Phone_OS.Ios)
				{
					Resources.UnloadAsset(m_AniClip);
				}
				m_AniClip = null;
			}

			if (m_AniAsset != null)
			{
				m_AniAsset.Unload(true);
				m_AniAsset = null;
			}
		}

		public void ReleaseSingle()
		{
			if (m_AniWWW != null)
			{
				m_AniWWW.Dispose();
				m_AniWWW = null;
			}

			if (m_AniClip != null)
			{
				if (CommonValue.PhoneOS == Phone_OS.Ios)
				{
					Resources.UnloadAsset(m_AniClip);
				}
				m_AniClip = null;
			}

			if (m_AniAsset != null)
			{
				m_AniAsset.Unload(true);
				m_AniAsset = null;
			}
		}

		public static IEnumerator LoadSingleAnimation(string aniName)
		{
			AnimationLoader aniLoader = GetAnimationLoader(aniName);

			if (aniLoader != null)
			{
                IEnumerator itor = aniLoader.LoadAnimationSync();
                while(itor.MoveNext())
                {
                    yield return null;
                }
			}
		}

		public static void ReleaseSingleAnimation(string clipName)
		{
			if (s_AllAniClips.ContainsKey(clipName))
			{
				AnimationLoader aniLoader = s_AllAniClips[clipName];
				if (aniLoader != null)
				{
					aniLoader.m_AssetRefer--;

					if (aniLoader.m_AssetRefer <= 0)
					{
						s_AllAniClips.Remove(clipName);
						aniLoader.ReleaseSingle();
					}
				}
			}
		}

		public static AnimationLoader GetAnimationLoader(string clipName)
		{
			AnimationLoader aniLoader = null;
			if (s_AllAniLoader.ContainsKey(clipName))
			{
				aniLoader = s_AllAniLoader[clipName];
				aniLoader.m_AssetRefer++;
			}
			else
			{
				if (s_AllAniClips.ContainsKey(clipName))
				{
					aniLoader = s_AllAniClips[clipName];
					aniLoader.m_AssetRefer++;
				}
				else
				{
					aniLoader = new AnimationLoader(clipName);
					aniLoader.m_AssetRefer = 1;
				}
			}

			return aniLoader;
		}

		public static AnimationClip GetAnimationClip(string clipName)
		{
			AnimationClip aniClip = null;

			if (s_AllAniClips.ContainsKey(clipName))
			{
				AnimationLoader aniLoader = s_AllAniClips[clipName];
				if (aniLoader != null)
				{
					aniClip = aniLoader.AniClip;
				}
			}

			return aniClip;
		}

        public static void ReleaseAnimationOnDownLoad(string clipName)
        {
            if (s_AllAniLoader.ContainsKey(clipName))
            {
                AnimationLoader aniLoader = s_AllAniLoader[clipName];
                s_AllAniLoader.Remove(clipName);

                if (aniLoader != null)
                {
                    aniLoader.Release();
                }
            }
        }

		public static void ReleaseAnimation(string clipName)
        {
			if (s_AllAniClips.ContainsKey(clipName))
			{
                AnimationLoader aniLoader = s_AllAniClips[clipName];
				s_AllAniClips.Remove(clipName);

				if (aniLoader != null)
				{
					aniLoader.Release();
				}
			}
		}

		public static void ClearWorldSceneAnimation()
		{
			m_WorldAniExist = false;

			ReleaseAnimation(AnimationLoader.StandWorldBoy);
			ReleaseAnimation(AnimationLoader.StandWorldGirl);
			ReleaseAnimation(AnimationLoader.DressBoy);
			ReleaseAnimation(AnimationLoader.DressGirl);
			ReleaseAnimation(AnimationLoader.DressBehind);
		}

		public static IEnumerator LoadWorldSceneAnimation()
		{
			m_WorldAniExist = true;

			List<AnimationLoader> loaderList = new List<AnimationLoader>();
			loaderList.Add(GetAnimationLoader(AnimationLoader.StandWorldBoy));
			loaderList.Add(GetAnimationLoader(AnimationLoader.StandWorldGirl));
			loaderList.Add(GetAnimationLoader(AnimationLoader.DressBoy));
			loaderList.Add(GetAnimationLoader(AnimationLoader.DressGirl));
			loaderList.Add(GetAnimationLoader(AnimationLoader.DressBehind));

            IEnumerator itor = null;
            foreach (AnimationLoader aniLoader in loaderList)
            {
                if (aniLoader != null)
                {
                    itor = aniLoader.LoadAnimationSync();
                    while(itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }
		}

		public static void ClearRoomSceneAnimation()
		{
			m_RoomAniExist = false;

			ReleaseAnimation(AnimationLoader.HelloBoy);
			ReleaseAnimation(AnimationLoader.HelloGirl);
			ReleaseAnimation(AnimationLoader.StandRoomBoy_A);
			ReleaseAnimation(AnimationLoader.StandRoomBoy_B);
			ReleaseAnimation(AnimationLoader.StandRoomBoy_C);
			ReleaseAnimation(AnimationLoader.StandRoomBoy_D);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_A);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_B);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_C);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_D);
			ReleaseAnimation(AnimationLoader.VIP_STAND_BOY_01);
			ReleaseAnimation(AnimationLoader.VIP_STAND_BOY_02);
			ReleaseAnimation(AnimationLoader.VIP_STAND_BOY_03);
			ReleaseAnimation(AnimationLoader.VIP_STAND_GIRL_01);
			ReleaseAnimation(AnimationLoader.VIP_STAND_GIRL_02);
			ReleaseAnimation(AnimationLoader.VIP_STAND_GIRL_03);
			ReleaseAnimation(AnimationLoader.BS_ROOM_BOY_STAND);
			ReleaseAnimation(AnimationLoader.BS_ROOM_GIRL_STAND);
		}

		public static IEnumerator LoadStarShowSceneAnimation(List<string> agentAni)
		{
			m_StarShowAniExist = true;
			s_StarShowAgentAniList = agentAni;

			List<AnimationLoader> loaderList = new List<AnimationLoader>();
			loaderList.Add(AnimationLoader.GetAnimationLoader(StarShow_Male_1));
			loaderList.Add(AnimationLoader.GetAnimationLoader(StarShow_Male_2));
			loaderList.Add(AnimationLoader.GetAnimationLoader(StarShow_Male_3));
			loaderList.Add(AnimationLoader.GetAnimationLoader(StarShow_Female_1));
			loaderList.Add(AnimationLoader.GetAnimationLoader(StarShow_Female_2));
			loaderList.Add(AnimationLoader.GetAnimationLoader(StarShow_Female_3));

			if (agentAni != null)
			{
				for (int i = 0; i < agentAni.Count; i++)
				{
					if (!string.IsNullOrEmpty(agentAni[i]))
					{
						loaderList.Add(AnimationLoader.GetAnimationLoader(agentAni[i]));
					}
				}
			}

			IEnumerator itor = null;
			foreach (AnimationLoader aniLoader in loaderList)
			{
				if (aniLoader != null)
				{
					itor = aniLoader.LoadAnimationSync();
					while (itor.MoveNext())
					{
						yield return null;
					}
				}
			}
		}

		public static void ClearStarShowSceneAnimation()
		{
			m_StarShowAniExist = false;
			if (s_StarShowAgentAniList != null)
			{
				for (int i = 0; i < s_StarShowAgentAniList.Count; i++)
				{
					if (!string.IsNullOrEmpty(s_StarShowAgentAniList[i]))
					{
						ReleaseAnimation(s_StarShowAgentAniList[i]);
					}
				}
				s_StarShowAgentAniList = null;
			}

			ReleaseAnimation(AnimationLoader.StarShow_Male_1);
			ReleaseAnimation(AnimationLoader.StarShow_Male_2);
			ReleaseAnimation(AnimationLoader.StarShow_Male_3);
			ReleaseAnimation(AnimationLoader.StarShow_Female_1);
			ReleaseAnimation(AnimationLoader.StarShow_Female_2);
			ReleaseAnimation(AnimationLoader.StarShow_Female_3);
		}

		public static IEnumerator LoadRoomSceneAnimation()
		{
			m_RoomAniExist = true;

			List<AnimationLoader> loaderList = new List<AnimationLoader>();
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.HelloBoy));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.HelloGirl));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomBoy_A));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomBoy_B));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomBoy_C));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomBoy_D));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomGirl_A));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomGirl_B));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomGirl_C));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomGirl_D));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.VIP_STAND_BOY_01));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.VIP_STAND_BOY_02));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.VIP_STAND_BOY_03));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.VIP_STAND_GIRL_01));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.VIP_STAND_GIRL_02));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.VIP_STAND_GIRL_03));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.BS_ROOM_BOY_STAND));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.BS_ROOM_GIRL_STAND));

            IEnumerator itor = null;
            foreach (AnimationLoader aniLoader in loaderList)
            {
                if (aniLoader != null)
                {
                    itor = aniLoader.LoadAnimationSync();
                    while(itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }
		}

		public static void ClearStageSceneAnimation()
		{
			m_StageAniExist = false;

			ReleaseAnimation(AnimationLoader.BoyPrepareDance);
			ReleaseAnimation(AnimationLoader.GirlPrepareDance);
			ReleaseAnimation(AnimationLoader.StartDance);
			ReleaseAnimation(AnimationLoader.MissBoy);
			ReleaseAnimation(AnimationLoader.MissGirl);
			ReleaseAnimation(AnimationLoader.LoseBoy);
			ReleaseAnimation(AnimationLoader.LoseGirl);
			ReleaseAnimation(AnimationLoader.WinBoy);
			ReleaseAnimation(AnimationLoader.WinGirl);

			ReleaseAnimation(AnimationLoader.HB_StartDance_Boy);
			ReleaseAnimation(AnimationLoader.HB_StartDance_Girl);
			ReleaseAnimation(AnimationLoader.HB_LoseBoy);
			ReleaseAnimation(AnimationLoader.HB_LoseGirl);
			ReleaseAnimation(AnimationLoader.HB_WinBoy + m_HB_Win_Index.ToString("D2"));
			ReleaseAnimation(AnimationLoader.HB_WinGirl + m_HB_Win_Index.ToString("D2"));

			ReleaseAnimation(AnimationLoader.HB_STAND_BOY_01);
			ReleaseAnimation(AnimationLoader.HB_STAND_BOY_02);
			ReleaseAnimation(AnimationLoader.HB_STAND_GIRL_01);
			ReleaseAnimation(AnimationLoader.HB_STAND_GIRL_02);
			ReleaseAnimation(AnimationLoader.HB_SELECT_BOY_01);
			ReleaseAnimation(AnimationLoader.HB_SELECT_BOY_02);
			ReleaseAnimation(AnimationLoader.HB_SELECT_GIRL_01);
			ReleaseAnimation(AnimationLoader.HB_SELECT_GIRL_02);

			ClearDanceAnimation();
		}

		public static void ClearDanceAnimation()
		{
			foreach (string stateName in s_DanceAniUnique)
			{
				AniState aniState = null;
				if (s_AniStates.ContainsKey(stateName))
				{
					aniState = s_AniStates[stateName];
				}

				if (aniState != null)
				{
					ReleaseAnimation(aniState.Motion);
				}
			}

			s_DanceAniUnique.Clear();
			s_DanceAniSequence.Clear();
			s_DanceAniSequenceBoy.Clear();
		}


		public static void ClearGuideSceneAnimation()
		{
			m_GuideAniExist = false;

			ReleaseAnimation(AnimationLoader.HelloBoy);
			ReleaseAnimation(AnimationLoader.HelloGirl);
			ReleaseAnimation(AnimationLoader.StartDance);
			ReleaseAnimation(AnimationLoader.MissBoy);
			ReleaseAnimation(AnimationLoader.MissGirl);

			ClearDanceAnimation();
		}


		public static IEnumerator LoadWeddingOutSceneAnimation()
		{
			m_WeddingAniExist = true;

			List<AnimationLoader> loaderList = new List<AnimationLoader>();

			loaderList.Add(GetAnimationLoader(AnimationLoader.StandRoomBoy_A));
			loaderList.Add(GetAnimationLoader(AnimationLoader.StandRoomGirl_A));
			loaderList.Add(GetAnimationLoader(AnimationLoader.WalkBoy));
			loaderList.Add(GetAnimationLoader(AnimationLoader.WalkGirl));
			loaderList.Add(GetAnimationLoader(AnimationLoader.BS_WalkBoy));
			loaderList.Add(GetAnimationLoader(AnimationLoader.BS_WalkGirl));

            IEnumerator itor = null;
            foreach (AnimationLoader aniLoader in loaderList)
            {
                if (aniLoader != null)
                {
                    itor = aniLoader.LoadAnimationSync();
                    while(itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }
		}

		public static void ClearWeddingOutSceneAnimation()
		{
			m_WeddingAniExist = false;

			ReleaseAnimation(AnimationLoader.StandRoomBoy_A);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_A);
			ReleaseAnimation(AnimationLoader.WalkBoy);
			ReleaseAnimation(AnimationLoader.WalkGirl);
			ReleaseAnimation(AnimationLoader.BS_WalkBoy);
			ReleaseAnimation(AnimationLoader.BS_WalkGirl);
		}

		static List<string> m_GroupAnimation = new List<string>();
		static List<string> m_SingleAnimation = new List<string>();

		public static IEnumerator LoadStudioSceneAnimation()
		{
			m_StudioAniExist = true;

			List<AnimationLoader> loaderList = new List<AnimationLoader>();

			loaderList.Add(GetAnimationLoader(AnimationLoader.HelloBoy));
			loaderList.Add(GetAnimationLoader(AnimationLoader.HelloGirl));
			loaderList.Add(GetAnimationLoader(AnimationLoader.StandRoomBoy_A));
			loaderList.Add(GetAnimationLoader(AnimationLoader.StandRoomGirl_A));


            IEnumerator itor = null;
            foreach (AnimationLoader aniLoader in loaderList)
            {
                if (aniLoader != null)
                {
                    itor = aniLoader.LoadAnimationSync();
                    while(itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }
		}

		public static IEnumerator LoadStudioSceneSingleAnimation(List<string> clips, bool isBoy)
		{
			List<AnimationLoader> loaderList = new List<AnimationLoader>();

			for (int i = 0; i < clips.Count; ++i)
			{
				string clipName = clips[i];
				if (!string.IsNullOrEmpty(clipName))
				{
					loaderList.Add(GetAnimationLoader(clipName));
					m_SingleAnimation.Add(clipName);
				}
			}

            IEnumerator itor = null;
            foreach (AnimationLoader aniLoader in loaderList)
            {
                if (aniLoader != null)
                {
                    itor = aniLoader.LoadAnimationSync();
                    while(itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }
		}

		public static IEnumerator LoadStudioSceneGroupAnimation(List<string> clips)
		{
			List<AnimationLoader> loaderList = new List<AnimationLoader>();
			for (int i = 0; i < clips.Count; ++i)
			{
				if (clips[i] == "")
					continue;
				loaderList.Add(GetAnimationLoader(clips[i]));
				m_GroupAnimation.Add(clips[i]);
			}

            IEnumerator itor = null;
            foreach (AnimationLoader aniLoader in loaderList)
            {
                if (aniLoader != null)
                {
                    itor = aniLoader.LoadAnimationSync();
                    while(itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }
		}

		public static void ClearStudioSingleAnimation()
		{
			for (int i = 0; i < m_SingleAnimation.Count; ++i)
			{
				ReleaseAnimation(m_SingleAnimation[i]);
			}
			m_SingleAnimation.Clear();
		}


		public static void ClearStudioGroupAnimation()
		{
			for (int i = 0; i < m_GroupAnimation.Count; ++i)
			{
				string clipName = m_GroupAnimation[i];
				ReleaseAnimation(clipName);
			}
			m_GroupAnimation.Clear();
		}

		public static void ClearStudioSceneAnimation()
		{
			m_StudioAniExist = false;

			ReleaseAnimation(AnimationLoader.HelloBoy);
			ReleaseAnimation(AnimationLoader.HelloGirl);
			ReleaseAnimation(AnimationLoader.StandRoomBoy_A);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_A);

			ClearStudioSingleAnimation();
			ClearStudioGroupAnimation();
		}

		static List<AnimationLoader> BuildAniList(string currState, bool isBoy)
		{
			List<AnimationLoader> loaderList = new List<AnimationLoader>();

			while (true)
			{
				if (s_AniStates.ContainsKey(currState))
				{
					AniState state = s_AniStates[currState];

					if (isBoy)
					{
						if (!s_DanceAniSequenceBoy.Contains(state.Name))
						{
							loaderList.Add(GetAnimationLoader(state.Motion));
							if (!s_DanceAniUnique.Contains(state.Name))
							{
								s_DanceAniUnique.Add(state.Name);
							}
						}
						s_DanceAniSequenceBoy.Add(state.Name);
					}
					else
					{
						if (!s_DanceAniSequence.Contains(state.Name))
						{
							loaderList.Add(GetAnimationLoader(state.Motion));
							if (!s_DanceAniUnique.Contains(state.Name))
							{
								s_DanceAniUnique.Add(state.Name);
							}
						}
						s_DanceAniSequence.Add(state.Name);
					}

					if (!currState.ToUpper().Contains("DANCE END"))
					{
						List<TranState> tranList = state.Trans;
						if (tranList.Count == 1)
						{
							currState = tranList[0].DestState;
						}
						else if (tranList.Count > 1)
						{
							foreach (TranState ts in tranList)
							{
								if (ts.IsBoy == isBoy)
								{
									currState = ts.DestState;
								}
							}
						}
					}
					else
					{
						break;
					}
				}
				else
				{
					Debug.LogError("Animation control need \"DANCE START\" motion");
					break;
				}
			}

			return loaderList;
		}

		/// <summary>
        /// 释放创建人物动画
		/// </summary>
		public static void ClearRoleCreateAnimation()
		{
			m_RoleCreateAniExist = false;

			ReleaseAnimation(AnimationLoader.VIP_STAND_BOY_01);
			ReleaseAnimation(AnimationLoader.VIP_STAND_GIRL_01);
			ReleaseAnimation(AnimationLoader.StandRoomBoy_A);
			ReleaseAnimation(AnimationLoader.StandRoomBoy_B);
			ReleaseAnimation(AnimationLoader.StandRoomBoy_C);
			ReleaseAnimation(AnimationLoader.StandRoomBoy_D);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_A);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_B);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_C);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_D);
		}

		/// <summary>
        /// 加载创建人物动画
		/// </summary>
		/// <returns></returns>
		public static IEnumerator LoadRoleCreateAnimation()
		{
			List<AnimationLoader> loaderList = new List<AnimationLoader>();
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.VIP_STAND_BOY_01));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.VIP_STAND_GIRL_01));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomBoy_A));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomBoy_B));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomBoy_C));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomBoy_D));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomGirl_A));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomGirl_B));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomGirl_C));
			loaderList.Add(AnimationLoader.GetAnimationLoader(AnimationLoader.StandRoomGirl_D));
            
            IEnumerator itor = null;
            foreach (AnimationLoader aniLoader in loaderList)
            {
                if (aniLoader != null)
                {
                    itor = aniLoader.LoadAnimationSync();
                    while(itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }

			m_RoleCreateAniExist = true;
		}

		public static IEnumerator LoadCeremonyOutSceneAnimation()
		{
			m_CeremonyAniExist = true;

			List<AnimationLoader> loaderList = new List<AnimationLoader>();

			loaderList.Add(GetAnimationLoader(AnimationLoader.StandRoomBoy_A));
			loaderList.Add(GetAnimationLoader(AnimationLoader.StandRoomGirl_A));
			loaderList.Add(GetAnimationLoader(AnimationLoader.WalkBoy));
			loaderList.Add(GetAnimationLoader(AnimationLoader.WalkGirl));
			loaderList.Add(GetAnimationLoader(AnimationLoader.BS_WalkBoy));
			loaderList.Add(GetAnimationLoader(AnimationLoader.BS_WalkGirl));

            IEnumerator itor = null;
			foreach (AnimationLoader aniLoader in loaderList)
			{
				if (aniLoader != null)
				{
                    itor = aniLoader.LoadAnimationSync();
                    while(itor.MoveNext())
                    {
                        yield return null;
                    }
				}
			}
		}

		public static void ClearCeremonyOutSceneAnimation()
		{
			m_CeremonyAniExist = false;

			ReleaseAnimation(AnimationLoader.StandRoomBoy_A);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_A);
			ReleaseAnimation(AnimationLoader.WalkBoy);
			ReleaseAnimation(AnimationLoader.WalkGirl);
			ReleaseAnimation(AnimationLoader.BS_WalkBoy);
			ReleaseAnimation(AnimationLoader.BS_WalkGirl);
		}

		public static IEnumerator LoadHandbookPoseAnimation(bool isBoy)
		{
			List<AnimationLoader> loaderList = new List<AnimationLoader>();

            if (isBoy)
            {
                loaderList.Add(GetAnimationLoader(AnimationLoader.HandbookPreviewPoseBoy));
            }
            else
            {
                loaderList.Add(GetAnimationLoader(AnimationLoader.HandbookPreviewPoseGirl));
            }

            IEnumerator itor = null;
            foreach (AnimationLoader aniLoader in loaderList)
            {
                if (aniLoader != null)
                {
                    itor = aniLoader.LoadAnimationSync();
                    while(itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }

			m_HandbookAniExist = true;
		}

        public static void ClearHandbookPoseAnimation()
		{
			m_HandbookAniExist = false;
            ReleaseAnimation(AnimationLoader.HandbookPreviewPoseBoy);
            ReleaseAnimation(AnimationLoader.HandbookPreviewPoseGirl);
		}
		
		public static IEnumerator LoadBigMamaSceneAnimation()
		{
			m_BigMamaAniExist = true;

			List<AnimationLoader> loaderList = new List<AnimationLoader>();

			loaderList.Add(GetAnimationLoader(AnimationLoader.StandRoomBoy_A));
			loaderList.Add(GetAnimationLoader(AnimationLoader.StandRoomGirl_A));
			loaderList.Add(GetAnimationLoader(AnimationLoader.WalkBoy));
			loaderList.Add(GetAnimationLoader(AnimationLoader.WalkGirl));
			loaderList.Add(GetAnimationLoader(AnimationLoader.BIG_MAMA_NPC_DANCE));
			loaderList.Add(GetAnimationLoader(AnimationLoader.BS_WalkBoy));
			loaderList.Add(GetAnimationLoader(AnimationLoader.BS_WalkGirl));

			IEnumerator itor = null;
			foreach (AnimationLoader aniLoader in loaderList)
			{
				if (aniLoader != null)
				{
                    itor = aniLoader.LoadAnimationSync();
                    while(itor.MoveNext())
                    {
                        yield return null;
                    }
				}
			}
		}

		public static void ClearBigMamaSceneAnimation()
		{
			m_BigMamaAniExist = false;

			ReleaseAnimation(AnimationLoader.StandRoomBoy_A);
			ReleaseAnimation(AnimationLoader.StandRoomGirl_A);
			ReleaseAnimation(AnimationLoader.WalkBoy);
			ReleaseAnimation(AnimationLoader.WalkGirl);
			ReleaseAnimation(AnimationLoader.BIG_MAMA_NPC_DANCE);
			ReleaseAnimation(AnimationLoader.BS_WalkBoy);
			ReleaseAnimation(AnimationLoader.BS_WalkGirl);
		}
	}
}