using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

class FBXPostprocessor : AssetPostprocessor
{
	static Material[] m_SkinMat = new Material[4];

	static string[] m_SkinMatName ={
		"b_body_1",
		"b_leg_1",
		"g_body_1",
		"g_leg_1"
	};
	
	public static Material[] SkinMat {
		get {
			if( m_SkinMat[0] == null )
			{
				string srcdir = GenerateMaterials.MaterialsSrcDir +"Skin/"; 
				m_SkinMat[0] = (Material)AssetDatabase.LoadAssetAtPath(srcdir+"boy_body.mat",typeof(Material));
				m_SkinMat[1] = (Material)AssetDatabase.LoadAssetAtPath(srcdir+"boy_leg.mat",typeof(Material));
				m_SkinMat[2] = (Material)AssetDatabase.LoadAssetAtPath(srcdir+"girl_body.mat",typeof(Material));
				m_SkinMat[3] = (Material)AssetDatabase.LoadAssetAtPath(srcdir+"girl_leg.mat",typeof(Material));
			}
			return m_SkinMat;
		}
	}	
	
    // This method is called just before importing an FBX.
    void OnPreprocessModel()
    {
		Debug.Log( "OnPreprocessModel assetPath:" + assetPath );
        ModelImporter mi = (ModelImporter)assetImporter;
        mi.globalScale = 1;

        if (!assetPath.Contains("/Characters/")) return;
        mi.animationCompression = ModelImporterAnimationCompression.Off;
		
		
        // we don't need Materials for Bones and  Animation.
		if (assetPath.Contains("/Models")) return;
        mi.importMaterials = false;
    }

    // This method is called immediately after importing an FBX.
    void OnPostprocessModel(GameObject go)
    {
		if (!assetPath.Contains("/Characters/")) return;
		
		Debug.Log("OnPostprocessModel GameObject:" + go.name);
		ProcessModel( go );
		ProcessAnimation( go );
		ProcessBones( go );
    }
	//deal with model
	void ProcessModel( GameObject go )
	{
		//nothing to do ;
		if (!assetPath.Contains("/Models") && !assetPath.Contains("/FBX_face")) return;
		if (!assetPath.Contains("/Decorate") )
		{
			RemoveAnimation( go );
		}
		
		SetShaderForCloth( go );
		//RemoveBones( go );
		SetDefaultSkinMat(go);
	}
	void SetDefaultSkinMat( GameObject go )
	{
		Material[] skinMats = SkinMat;
		Renderer[] rs = go.GetComponentsInChildren<Renderer>(true);
		List<Material> lms = new List<Material>();
		foreach( Renderer r in rs )
		{
			Material[] ms = r.sharedMaterials;
			for( int i = 0 ; i < ms.Length ; i++ )
			{
				for( int n =0; n < m_SkinMatName.Length ; n++ )
				{
					if( ms[i].name == m_SkinMatName[n] )
					{
						//AssetDatabase.DeleteAsset( AssetDatabase.GetAssetPath(r.sharedMaterials[i]) );
						if( !lms.Contains( ms[i] ) )
						{
							lms.Add( ms[i] );
						}
						ms[i] = skinMats[n];
						break;
					}
				}
			}
			r.sharedMaterials = ms;
		}
		
		foreach( Material m in lms )
		{
			AssetDatabase.DeleteAsset( AssetDatabase.GetAssetPath(m ) );
		}
		lms.Clear();
	}
	void ProcessAnimation( GameObject go )
	{
		if (!assetPath.Contains("/Animations/")) return;
		
		Debug.Log( "ProcessAnimation GameObject:" + go.name );
		RemoveMesh( go );
		RemoveBones( go );
		ModelImporter mi = (ModelImporter)assetImporter;
		mi.animationType = ModelImporterAnimationType.Generic;
		foreach(ModelImporterClipAnimation i in mi.clipAnimations)
		{
			i.lockRootHeightY = false;
			i.lockRootPositionXZ = false;
			i.lockRootRotation = false;
		}
		//mi.clipAnimations
		
/*
		Animation an = go.GetComponent<Animation>();
		if( an )
		{
			AnimationClip[] clips = AnimationUtility.GetAnimationClips( an );
			if( clips != null  )
			{
				//remove all
				List<AnimationClip> anlist = new List<AnimationClip>();
				foreach( AnimationClip cl in clips )
				{
					an.RemoveClip( cl );
									
					AnimationClip newAn = RemoveScaleCurve(cl);
					newAn = MergeKey( newAn );
					anlist.Add( newAn );
				}
				foreach( AnimationClip clnew in anlist )
				{
					an.AddClip( clnew,go.name );
				}
			}
			
		}
*/
	}
	
/*	AnimationClip RemoveScaleCurve( AnimationClip cl )
	{
		AnimationClip newAn  = new AnimationClip();
		AnimationClipCurveData[] cds = AnimationUtility.GetAllCurves( cl );
		if( cds != null )
		{
			foreach( AnimationClipCurveData cd in cds )
			{
				if( !cd.propertyName.Contains("localScale") )
				{
					newAn.SetCurve( cd.path,cd.type,cd.propertyName,cd.curve);
				}
			}
		}
		return newAn;
	}
	AnimationClip MergeKey( AnimationClip cl )
	{
		AnimationClip newAn  = new AnimationClip();
		AnimationClipCurveData[] cds = AnimationUtility.GetAllCurves( cl );
		Keyframe lastAddKf ;
		Keyframe prevkf ;
		Keyframe nowkf ;
		if( cds != null )
		{
			foreach( AnimationClipCurveData cd in cds )
			{
				AnimationCurve ac = cd.curve;
				if( ac.length >= 3)
				{
					AnimationCurve newAC = new AnimationCurve();
					lastAddKf = ac[0];
					prevkf = ac[1];
					nowkf = ac[2];
					newAC.AddKey( lastAddKf );
					for(int i = 2 ; i<ac.length ; i++ )
					{
						nowkf = ac[i];
						bool bDropPrevKey = false;
						if( Mathf.Approximately( nowkf.value,prevkf.value ) )
						{
							if( Mathf.Approximately( prevkf.value , lastAddKf.value ) )
							{	
								bDropPrevKey = true;
							}
						}
						if( !bDropPrevKey )
						{
							lastAddKf = prevkf;
							newAC.AddKey( lastAddKf );
						}
						prevkf = nowkf;
					}
					
					newAC.AddKey(nowkf);
				
					newAn.SetCurve(cd.path,cd.type,cd.propertyName,newAC);
				}
				else
				{
					newAn.SetCurve(cd.path,cd.type,cd.propertyName,ac);
				}
				
			}
		}
		return newAn;
	}*/
	void ProcessBones( GameObject go )
	{
		if (!assetPath.Contains("/Bones/")) return;
        // Assume an animation FBX has in character/Bones direction,
        // to determine if an fbx is a Model, like cloth.
		
		// For animation FBX's all unnecessary Objects are removed.
        // This is not required but improves clarity when browsing assets.
		//RemoveBones( go );
		
		Debug.Log("ProcessBones");
		
		RemoveMesh( go );
		RemoveAnimation( go );

	}
	
	void RemoveBones( GameObject go )
	{
		// Remove the bones.
        foreach (Transform o in go.transform)
		{
			if(o.gameObject.GetComponent<SkinnedMeshRenderer>() == null)
				Object.DestroyImmediate(o.gameObject);
		}
	}
	
	// Remove SkinnedMeshRenderers and their meshes.
	void RemoveMesh( GameObject go )
	{
		Transform t = null;
		for( int i = go.transform.childCount-1 ; i >= 0 ; i-- )
		{
			t = go.transform.GetChild( i );
			bool bRemove = false;
	        foreach ( SkinnedMeshRenderer smr in t.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>() )
	        {
	            Object.DestroyImmediate(smr.sharedMesh, true);
				if( smr.transform != t )
					Object.DestroyImmediate( smr.gameObject );
				bRemove = true;
	        }
			if( bRemove )
				GameObject.DestroyImmediate( t.gameObject );
		}
	}
	
	void RemoveAnimation( GameObject go )
	{
		if ( !go.name.StartsWith("wing_")
			&& !go.name.StartsWith("hip_")
			&& !go.name.StartsWith("lefthand_")
			&& !go.name.StartsWith("righthand_")
			&& !go.name.StartsWith("shoulders_")
			&& !go.name.StartsWith("leftear_")
			&& !go.name.StartsWith("rightear_"))
		{
			foreach (Animation ani in go.GetComponentsInChildren<Animation>())
			{
				AnimationClip clip = ani.clip;
				Object.DestroyImmediate( clip ,true );
				Object.DestroyImmediate( ani );
			}
		}
	}
	
	void OnPreprocessTexture()
	{
        if (!assetPath.Contains("/Characters/")) return;

        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.mipmapEnabled = false;
        string path = textureImporter.assetPath;
        if (path.Contains("Assets/Add On Resource/Characters/Models/"))
        {
            if (CheckIOSTexture())
            {
                textureImporter.textureFormat = TextureImporterFormat.PVRTC_RGBA4;
            }
            else
            {
                textureImporter.textureFormat = TextureImporterFormat.ETC_RGB4;
                path = path.Replace("Assets/Add On Resource/Characters/Models/", "");
                if (path.Contains("all_"))
                {
                    if (path[path.Length - 6] == '_' && path[path.Length - 5] == 'a')
                    {
                        textureImporter.maxTextureSize = 256;
                    }
                    else
                    {
                        textureImporter.maxTextureSize = 512;
                    }
                }
                else if (path[path.Length - 6] == '_' && path[path.Length - 5] == 'a')
                {
                    textureImporter.maxTextureSize = 256;
                }
                else if (path.Contains("gloves_") || path.Contains("headwear_") || path.Contains("facial_")
                    || path.Contains("shoulders_") || path.Contains("lefthand_") || path.Contains("righthand_")
                    || path.Contains("wrist_") || path.Contains("leftear_") || path.Contains("rightear_"))
                {
                    textureImporter.maxTextureSize = 128;
                }
                else if (path.Contains("face_1000") || path.Contains("face_1500"))
                {
                    textureImporter.maxTextureSize = 512;
                }
                else
                {
                    textureImporter.maxTextureSize = 256;
                }
            }
        }
	}

    private bool CheckIOSTexture()
    {
        int index = assetPath.LastIndexOf('.');
        string path = assetPath.Substring(0, index);

        string iosTag = "_ios";
        int iosIndex = path.LastIndexOf("_ios");
        if (iosIndex != -1 && (iosIndex + iosTag.Length == path.Length))
        {
            return true;
        }
        return false;
    }

	
	void SetShaderForCloth( GameObject go )
	{
		foreach( SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>() )
		{
			Material m = smr.sharedMaterial;
			if (m.name.Contains("hair_") || m.name.Contains("coat_") || m.name.Contains("pant_"))
			{
				m.shader = Shader.Find("XuanQu/Charactor/Cloth-AlphaTest-Cutoff");
				Texture2D t= (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(m.mainTexture).Replace(m.mainTexture.name,m.mainTexture.name+"_a"),typeof(Texture2D));
				if( t != null )
				{
					m.SetTexture("_MainTexAphle",t);
				}
			}
			else if( m.name.Contains("_body_") || m.name.Contains("face_") 
				|| m.name.Contains("_leg_") || m.name.Contains("_foot_") 
				|| m.name.Contains("gloves_3000")
				|| m.name.Contains("gloves_3500"))
			{
				m.shader = Shader.Find("XuanQu/Charactor/Skin");
				m.color = new Color(1,1,1,1);
			}
			else
			{
				m.shader = Shader.Find("XuanQu/Charactor/Cloth");
				
				Texture2D t= (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(m.mainTexture).Replace(m.mainTexture.name,m.mainTexture.name+"_a"),typeof(Texture2D));
				if( t != null )
				{
					m.shader = Shader.Find("XuanQu/Charactor/Cloth-AlphaTest");
					m.SetTexture("_MainTexAphle",t);
				}


// sample for artist
//				t = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(m.mainTexture).Replace(m.mainTexture.name,m.mainTexture.name+"_b"),typeof(Texture2D));
//				if( t != null )
//				{
//					m.shader = Shader.Find("XuanQu/Charactor/Cloth-AlphaTest-Cuttoff");
//					m.SetTexture("_MainTexAphle",t);
//				}

			}
			
			//m.shader = new Shader()
		}
	}
	
}