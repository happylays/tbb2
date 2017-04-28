using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Common;

namespace LoveDance.Client.Loader
{
	public class RoleBodyLoader
	{
        static GameObject m_roleBodyLoadCoroutineObj = null;
        private DownLoadCoroutine m_roleBodyloaderCoroutine = null;

		List<GameObject> m_SMRs = null;
		Dictionary<ParticleSystem, float> m_clothParticleEffect = null;
		Dictionary<ParticleSystem, float> m_extraParticleEffect = null;
		SkinnLoader m_SkinLoader = null;
		ModelLoader m_ModelLoader = null;
		private ItemCloth_Type m_ColthType = ItemCloth_Type.ItemCloth_Type_Begin;
		private string m_ResuceName;
		private string m_ChildPath = "";	//模型子目录
        private bool m_isNetLoad = false;//是否需要动态下载;

        private Callback<RoleBodyLoader,bool> m_callBackDownLoadFinish = null;

		private ItemCloth_Type[] m_ExcludeTypes = null;

		public ItemCloth_Type[] ExcludeTypes
		{
			get
			{
				return m_ExcludeTypes;
			}
		}

		public string ResuceName
		{
			get
			{
				return m_ResuceName;
			}
		}

		public ItemCloth_Type ClothType
		{
			get
			{
				return this.m_ColthType;
			}
			set
			{
				m_ColthType = value;
			}
		}

		RoleBodyLoader(string strFileName, ItemCloth_Type Type, ItemCloth_Type[] excludeTypes, string childPath) //: base(strFileName)
		{
			m_ResuceName = strFileName;
			m_ColthType = Type;
			m_ExcludeTypes = excludeTypes;
			m_ChildPath = childPath;

            m_isNetLoad = WWWDownLoaderConfig.CheckResNeedUpdate(m_ResuceName);
		}

        private DownLoadCoroutine GetDownLoadCoroutine()
        {
            if (m_roleBodyLoadCoroutineObj == null)
            {
                GameObject gtGroupObj = GameObject.Find("LoaderCoroutineGroup");
                if (gtGroupObj == null)
                {
                    gtGroupObj = new GameObject("LoaderCoroutineGroup");
                    GameObject.DontDestroyOnLoad(gtGroupObj);
                }
                m_roleBodyLoadCoroutineObj = new GameObject("RoleBodyLoadCoroutine");//Coroutine节点;
                m_roleBodyLoadCoroutineObj.transform.parent = gtGroupObj.transform;
                GameObject.DontDestroyOnLoad(m_roleBodyLoadCoroutineObj);
            }

            if (m_roleBodyloaderCoroutine == null)
            {
                m_roleBodyloaderCoroutine = m_roleBodyLoadCoroutineObj.AddComponent<DownLoadCoroutine>();
                return m_roleBodyloaderCoroutine;
            }
            return null;
        }

        private void ReleaseDownLoadCoroutine()
        {
            if (m_roleBodyloaderCoroutine != null)
            {
                m_roleBodyloaderCoroutine.StopAllCoroutines();
                Component.Destroy(m_roleBodyloaderCoroutine);

                m_roleBodyloaderCoroutine = null;
            }
        }

		public static RoleBodyLoader GetRoleBodyLoader(string strFileName, ItemCloth_Type Type, ItemCloth_Type[] excludeTypes, string childPath)
		{
			RoleBodyLoader Loader = new RoleBodyLoader(strFileName, Type, excludeTypes, childPath);
			return Loader;
		}

        /// <summary>
        /// 是不是需要网络下载;
        /// </summary>
        public bool IsNetLoad
        {
            get
            {
                return m_isNetLoad;
            }
        }

        public IEnumerator LoadBodySync()
        {
            IEnumerator itor = LoadBody();
            while(itor.MoveNext())
            {
                yield return null;
            }
        }

        public void LoadBodyAsync(Callback<RoleBodyLoader,bool> callBack)
        {
            DownLoadCoroutine gtDLC = GetDownLoadCoroutine();
            if(gtDLC != null)
            {
                m_callBackDownLoadFinish = callBack;
                gtDLC.StartCoroutine(LoadBody());
            }
        }

        private IEnumerator LoadBody()
		{
			bool isSkinLoadSuc = false;
			bool isModelLoadSuc = false;
            IEnumerator itor = null;
            if (m_SkinLoader == null)
            {
                m_SkinLoader = SkinnLoader.GetkinnedMeshRendererLoader(m_ResuceName, m_ChildPath);
            }
            if (m_SkinLoader != null)
            {
                itor = m_SkinLoader.LoadMeshRenderer();
                while (itor.MoveNext())
                {
                    yield return null;
                }
				isSkinLoadSuc = m_SkinLoader.IsLoadSuc;
            }
            if (m_ModelLoader == null)
            {
                m_ModelLoader = ModelLoader.GetModelLoader(m_ResuceName, m_ChildPath);  
            }
            if (m_ModelLoader != null)
            {
                itor = m_ModelLoader.LoadModel();
                while (itor.MoveNext())
                {
                    yield return null;
                }
				isModelLoadSuc = m_ModelLoader.IsLoadSuc;
			}

            if (m_callBackDownLoadFinish != null)
            {
				m_callBackDownLoadFinish(this, isSkinLoadSuc || isModelLoadSuc);
            }
        }
                
		public List<GameObject> GetMainGameObject
		{
			get
			{
				if (m_SMRs == null && m_SkinLoader != null)
				{
					System.Object[] arr = m_SkinLoader.GetMainGameObject;

					m_SMRs = (List<GameObject>)arr[0];
					m_clothParticleEffect = (Dictionary<ParticleSystem, float>)arr[1];
				}
				return m_SMRs;
			}
		}

		public Dictionary<ParticleSystem, float> GetClothParticleEffect
		{
			get
			{
				return m_clothParticleEffect;
			}
		}

		public List<object> GetBoneNames
		{
			get
			{
				if (m_SkinLoader != null && m_SkinLoader != null)
				{
					return m_SkinLoader.BoneNames;
				}
				return null;
			}
		}

		public GameObject GetExtraGameObject
		{
			get
			{
				System.Object[] resArr = m_ModelLoader.TotalModel;
				GameObject go = (GameObject)resArr[0];
				m_extraParticleEffect = (Dictionary<ParticleSystem, float>)resArr[1];

				return go;
			}
		}

		public Dictionary<ParticleSystem, float> GetExtraParticleEffect
		{
			get
			{
				return m_extraParticleEffect;
			}
		}

		public void Release()
		{
            ReleaseDownLoadCoroutine();

			m_clothParticleEffect = null;
			m_extraParticleEffect = null;
            m_callBackDownLoadFinish = null;

			if (m_SMRs != null)
			{
				foreach (GameObject smr in m_SMRs)
				{
					GameObject.Destroy(smr);
				}

				m_SMRs.Clear();
			}

			if (m_SkinLoader != null)
			{
				m_SkinLoader.Release();
				m_SkinLoader = null;
			}

			if (m_ModelLoader != null)
			{
				m_ModelLoader.Release();
				m_ModelLoader = null;
			}
		}
	}
}