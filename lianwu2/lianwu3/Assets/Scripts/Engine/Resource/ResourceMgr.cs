using System.Collections;
using LoveDance.Client.Common;
//using LoveDance.Client.Data.Setting;
using LoveDance.Client.Loader;
//using LoveDance.Client.Data;

namespace LoveDance.Client.Logic.Ress
{
    public class ClientResourcesMgr
    {
        static bool s_IsInitUIRes = false;
        static bool s_IsInitRemainRes = false;
        static bool s_IsInitCommUIRes = false;
        static bool s_IsInitRequiredRes = false;

        static bool s_IsInitingUIRes = false;
        static bool s_IsInitingRemainRes = false;
        static bool s_IsInitingCommUIRes = false;
        static bool s_IsInitingRequiredRes = false;

        public static bool IsInitRemainRes
        {
            get
            {
                return s_IsInitRemainRes;
            }
        }

        public static bool AlreadyInitRequiredRes
        {
            get
            {
                return s_IsInitRequiredRes && !s_IsInitingRequiredRes;
            }
        }

        /// <summary>
        /// 初始化loader路径
        /// </summary>
        public static void InitGameLoader()
        {
            AnimationLoader.InitAniLoader(
                CommonValue.AniDir,
                CommonValue.AniWWWDir,
                CommonValue.InAniDir,
                CommonValue.InAniWWWDir,
                CommonValue.NetAniDir);

            BoneLoader.InitPlayerBone(
                CommonValue.BoneDir,
                CommonValue.BoneWWWDir,
                CommonValue.InBoneDir,
                CommonValue.InBoneWWWDir,
                CommonValue.NetBoneDir);

            ModelLoader.InitModelLoader(
                CommonValue.MaterialDir,
                CommonValue.MaterialWWWDir,
                CommonValue.InMaterialDir,
                CommonValue.InMaterialWWWDir,
                CommonValue.NetMaterialDir);

            SkinnLoader.InitSkinLoader(
                CommonValue.MaterialDir,
                CommonValue.MaterialWWWDir,
                CommonValue.InMaterialDir,
                CommonValue.InMaterialWWWDir,
                CommonValue.NetMaterialDir);

            UIWndLoader.InitUIWndLoader(
                CommonValue.UIDir,
                CommonValue.UIWWWDir,
                CommonValue.InUIDir,
                CommonValue.InUIWWWDir,
                CommonValue.NetUIDir);

            UIAtlasLoader.InitUIAtlasLoader(
                CommonValue.UIAtlasDir,
                CommonValue.UIAtlasWWWDir,
                CommonValue.InUIAtlasDir,
                CommonValue.InUIAtlasWWWDir,
                CommonValue.NetUIAtlasDir);

            ExtraLoader.InitExtraLoader(
                CommonValue.UITextureDir,
                CommonValue.UITextureWWWDir,
                CommonValue.InUITextureDir,
                CommonValue.InUITextureWWWDir,
                CommonValue.NetUITextureDir);

            SceneLoader.InitSceneLoader(
                CommonValue.StageWWWDir,
                CommonValue.StageDir,
                CommonValue.InStageWWWDir,
                CommonValue.InStageDir,
                CommonValue.NetStageDir
                );

        }

        /// <summary>
        /// 加载客户端资源
        /// </summary>
        public static IEnumerator LoadClientResource()
        {
            IEnumerator itor = LoadRequiredResource();
            while (itor.MoveNext())
            {
                yield return null;
            }

            itor = LoadUIResource();
            while (itor.MoveNext())
            {
                yield return null;
            }

            //itor = LoadCommonUI();
            //while (itor.MoveNext())
            //{
            //    yield return null;
            //}

            itor = LoadRemainResource();
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        //TODO
        public static void ReleaseClientResource()
        {

        }

        /// <summary>
        /// 加载UI资源
        /// </summary>
        public static IEnumerator LoadUIResource()
        {
            if (!s_IsInitUIRes)
            {
                s_IsInitUIRes = true;
                s_IsInitingUIRes = true;

                IEnumerator itor = ShaderLoader.LoadAllShader(
                    CommonValue.ShaderDir,
                    CommonValue.ShaderWWWDir,
                    CommonValue.InShaderDir,
                    CommonValue.InShaderWWWDir,
                    CommonValue.NetShaderDir);

                while (itor.MoveNext())
                {
                    yield return null;
                }

                //IEnumerator itor;
                itor = UIWndLoader.PrepareUI();
                while (itor.MoveNext())
                {
                    yield return null;
                }

                s_IsInitingUIRes = false;
            }
            else
            {
                while (s_IsInitingUIRes)
                {
                    yield return null;
                }
            }
        }

        /// <summary>
        /// 加载关键性资源
        /// </summary>
        public static IEnumerator LoadRequiredResource()
        {
            if (!s_IsInitRequiredRes)
            {
                s_IsInitingRequiredRes = true;
                s_IsInitRequiredRes = true;

                //while (!WWWDownLoaderConfig.IsVersionConfigInfoReady)
                //{
                //    yield return null;
                //}

                IEnumerator itor = LoadStaticData();
                while (itor.MoveNext())
                {
                    yield return null;
                }

                itor = UIWndLoader.LoadUIConfig(CommonValue.UIDir, CommonValue.InUIDir, CommonValue.NetUIDir);
                while (itor.MoveNext())
                {
                    yield return null;
                }

                s_IsInitingRequiredRes = false;
            }
            else
            {
                while (s_IsInitingRequiredRes)
                {
                    yield return null;
                }
            }
        }

        /// <summary>
        /// 加载公共UI资源
        /// </summary>
        public static IEnumerator LoadCommonUI()
        {
            if (!s_IsInitCommUIRes)
            {
                s_IsInitingCommUIRes = true;
                s_IsInitCommUIRes = true;

                IEnumerator itor = UIWndLoader.LoadMainWndAsync();
                while (itor.MoveNext())
                {
                    yield return null;
                }

                s_IsInitingCommUIRes = false;
            }
            else
            {
                while (s_IsInitingCommUIRes)
                {
                    yield return null;
                }
            }
        }

        /// <summary>
        /// 加载非关键性资源
        /// </summary>
        static IEnumerator LoadRemainResource()
        {
            if (!s_IsInitRemainRes)
            {
                s_IsInitingRemainRes = true;
                s_IsInitRemainRes = true;

                IEnumerator itor = SkinnLoader.LoaddefaultSkins();
                while (itor.MoveNext())
                {
                    yield return null;
                }

                itor = BoneLoader.LoadPlayerBone();
                while (itor.MoveNext())
                {
                    yield return null;
                }


                s_IsInitingRemainRes = false;
            }
            else
            {
                while (s_IsInitingRemainRes)
                {
                    yield return null;
                }
            }
        }
        
        /// <summary>
        /// 加载静态数据
        /// </summary>
        static IEnumerator LoadStaticData()
        {
            IEnumerator itor = null;
            itor = (new CMusicDataLoader()).Load(CommonDef.SD_MUSIC_INFO);
            while (itor.MoveNext())
            {
                yield return null;
            }

            yield return null;            
        }
    }
}