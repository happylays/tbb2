using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using LoveDance.Client.Common;

public class GenerateAnimation 
{
	//command
	[MenuItem("Resource Generator/GenerateResource/Animations")]
	static void GenerateAnimations()
	{
		if (!Directory.Exists(AniAssetbundlePath))
		{
			Directory.CreateDirectory(AniAssetbundlePath);
		}

		CopyAnimationFile(DanceSrcDir, DanceTempDir);

		ProceAnimations_Dir(DanceTempDir);

		//Delete Animation_Temp file
		if (Directory.Exists(DanceTempDir))
		{
			Directory.Delete(DanceTempDir, true);
		}
		AssetDatabase.Refresh();
	}

	[MenuItem("Assets/GenerateResource/Animation")]
	static void Generate_Animation()
	{

		if (Directory.Exists(DanceTempDir))
		{
			Directory.Delete(DanceTempDir, true);
		}
		Directory.CreateDirectory(DanceTempDir);
		

		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		if (selection != null && selection.Length > 0)
		{
			int fbxCount = 0;

			foreach (Object fbxObj in selection)
			{
				string fbxSrcPath = AssetDatabase.GetAssetPath(fbxObj);
				if (fbxSrcPath.EndsWith("FBX") && File.Exists(fbxSrcPath))
				{
					string fbxTempPath = DanceTempDir + fbxObj.name + ".FBX";
					//Copy to Animation_Temp
					File.Copy(fbxSrcPath, fbxTempPath, true);

					AssetDatabase.Refresh();

					//Generate assetBundle
					ProcAnimations_File(fbxTempPath);

					fbxCount++;
				}
			}

			EditorUtility.DisplayDialog("Tips", "[ " + fbxCount + " ] FBX files have been processed.", "OK");
		}

		//Delete Animation_Temp file
		if (Directory.Exists(DanceTempDir))
		{
			Directory.Delete(DanceTempDir, true);
		}
		AssetDatabase.Refresh();
	}

	[MenuItem("Assets/GenerateResource/Controller - No dependency tracking")]
	static void Generate_Controller_NoDependency()
	{
		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		if (selection != null && selection.Length > 0)
		{
			bool isFailed = false;
			int count = 0;

			foreach (Object obj in selection)
			{
				if (obj is AnimatorController)
				{
					string assetPath = AssetDatabase.GetAssetPath(obj);

					try
					{
						ParseString(assetPath);
						count++;
					}
					catch (System.Exception e)
					{
						Debug.LogException(e);
						isFailed = true;
					}
				}
			}

			if (isFailed)
			{
				EditorUtility.DisplayDialog("Tips", "Complete Failed!\nAll [ " + count + " ] controller files have be parsed", "OK");
			}
			else
			{
				EditorUtility.DisplayDialog("Tips", "Complete Success!\nAll [ " + count + " ] controller files have be parsed", "OK");
			}
		}
	}

	[MenuItem("Assets/GenerateResource/Controller - Track dependencies")]
	static void Generate_Controller_TrackDependencies()
	{
		bool isFailed = false;
		byte count = 0;

		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		if (selection!= null && selection.Length > 0)
		{
			string destPath = AniAssetbundlePath + "Controller/";
			try
			{
				if (Directory.Exists(destPath))
				{
					Directory.Delete(destPath, true);
				}
				Directory.CreateDirectory(destPath);

				if (Directory.Exists(UsedAniAssetbundlePath))
				{
					Directory.Delete(UsedAniAssetbundlePath, true);
				}
				Directory.CreateDirectory(UsedAniAssetbundlePath);

				if (Directory.Exists(UsedAniFBXPath))
				{
					Directory.Delete(UsedAniFBXPath, true);
				}
				Directory.CreateDirectory(UsedAniFBXPath);

				if (Directory.Exists(DanceTempDir))
				{
					Directory.Delete(DanceTempDir, true);
				}
				Directory.CreateDirectory(DanceTempDir);

				AssetDatabase.Refresh();
			}
			catch (System.Exception ex)
			{
				Debug.LogWarning(ex);
				isFailed = false;
				EditorUtility.DisplayDialog("Warning", "Delete Directory failed,Please do not open ../Controller or ../Used directory.", "OK");
				return;
			}

			Dictionary<string, string> allMotionDic = new Dictionary<string, string>();//Save animations of one operation,<Motion, StateName>
			string errorStrName = "";

			//Deal with controller
			foreach (Object obj in selection)
			{
				if (obj is AnimatorController)
				{
					string assetPath = AssetDatabase.GetAssetPath(obj);

					//AnimatorController ctrl = (AnimatorController)obj;

					Dictionary<string, StateInfo> stateDic = ParseString(assetPath);
					foreach (StateInfo sInfo in stateDic.Values)
					{
						if (!allMotionDic.ContainsKey(sInfo.Motion))
						{
							allMotionDic.Add(sInfo.Motion, sInfo.Name);
						}
					}
					count++;
				}
				else
				{
					errorStrName += obj.name + ",";
				}
			}

			//Deal with animation
			GenerateControllerAni(allMotionDic);//Save used animation to other directory

			//Delete Animation_Temp file
			if (Directory.Exists(DanceTempDir))
			{
				Directory.Delete(DanceTempDir, true);
			}

			if (isFailed)
			{
				EditorUtility.DisplayDialog("Warning", "Complete failed,Please try again!", "OK");
			}
			else
			{
				EditorUtility.DisplayDialog("Tips", "Complete success!,Total deal with[ " + count + " ]controller file", "OK");
			}

			if (!string.IsNullOrEmpty(errorStrName))
			{
				Debug.LogWarning("Some inValidate file are used," + errorStrName);
			}
		}
		else
		{
			EditorUtility.DisplayDialog("Warning", "No selected controller files.", "OK");
		}
		AssetDatabase.Refresh();
	}

	static string DanceSrcDir = "Assets/Add On Resource/Animation/";
	static string DanceTempDir = "Assets/Add On Resource/Animation_Temp/";
	public static string AniControlAssetbundlePath
	{
		get { return GenerateResource.ResAssetbundleDir + "Animations/Controller/"; }
	}
	public static string UsedAniAssetbundlePath
	{
		get { return GenerateResource.ResAssetbundleDir + "Animations/UsedAnimationBundle/"; }
	}
	public static string UsedAniFBXPath
	{
		get { return GenerateResource.ResAssetbundleDir + "Animations/UsedAnimationFBX/"; }
	}
	public static string AniAssetbundlePath
	{
		get { return GenerateResource.ResAssetbundleDir + "Animations/"; }
	}
	
	static void CopyAnimationFile(string srcPath, string destPath)
	{
		if (Directory.Exists(destPath)) Directory.Delete(destPath, true);
		AssetDatabase.Refresh();

		Directory.CreateDirectory(destPath);

		string[] fileArr = Directory.GetFiles(srcPath, "*.FBX", SearchOption.AllDirectories);
		for(int i = 0; i < fileArr.Length; ++i)
		{
			string filePath = fileArr[i];
			filePath = filePath.Replace("\\", "/");
			string dFilePath = destPath + filePath.Substring(filePath.LastIndexOf("/") + 1);
			File.Copy(filePath, dFilePath);
		}
		AssetDatabase.Refresh();
	}

	private static Dictionary<string, StateInfo> ParseString(string filePath)
	{
		Dictionary<string, string> stateCache = new Dictionary<string, string>();//State
		Dictionary<string, string> stateMachineCache = new Dictionary<string, string>();//StateMachine
		Dictionary<string, string> transitionCache = new Dictionary<string, string>();//Transitions

		Dictionary<string, StateInfo> stateDic = null;

		using (StreamReader sr = new StreamReader(filePath, CommonFunc.GetCharsetEncoding()))
		{
			string line = null;
			string pStr = null;

			while ((line = sr.ReadLine()) != null)
			{
				if (line.StartsWith("---"))
				{//StateMachine State Transition
					pStr += "#Id=" + line.Trim();
				}
				else
				{
					if (line.StartsWith("Transition") || line.StartsWith("State") || line.StartsWith("StateMachine"))
					{//top property (StateMachine State Transition filed)
						pStr += "|StateType=" + line.Trim().Replace(":", "").Trim();
					}
					else if (line.StartsWith("  m_"))
					{//2B
						string[] str = line.Split(':');
						if (str.Length >= 3 && !string.IsNullOrEmpty(str[2].Trim()))
						{
							pStr += "|" + str[0].Trim() + "=" + str[2].Replace("}", "").Trim();
						}
						else
						{
							pStr += "|" + str[0].Trim() + "=" + str[1].Trim();
						}
					}
					else if (line.StartsWith("  - {"))
					{ //2B-1B
						string[] str = line.Split(',');
						if (str.Length >= 3 && !string.IsNullOrEmpty(str[1].Trim()))
						{
							string[] subStr = str[1].Trim().Split(':');
							pStr += subStr[1].Trim();
						}
					}
					else if (line.StartsWith("  - m_"))
					{//2B-1B
						string[] str = line.Split(':');
						pStr += "," + str[0].Trim() + ":" + str[1].Trim();
					}
					else if (line.StartsWith("    m_"))
					{//4B
						string[] str = line.Split(':');
						pStr += ";" + str[0].Trim() + ":" + str[1].Trim();
					}
					else if (line.StartsWith("    data"))
					{
						pStr += ",";
					}
					else if (line.StartsWith("      first"))
					{ //4B
						pStr += "first:";
						string[] str = line.Split(':');
						if (!string.IsNullOrEmpty(str[2].Trim()))
						{
							pStr += ";" + str[2].Replace("}", "").Trim();
						}
					}
					else if (line.StartsWith("      second"))
					{ //6B
						pStr += "/second:";
					}
					else if (line.StartsWith("      - {"))
					{ //6B
						string[] str = line.Split(':');
						pStr += ";" + str[1].Replace("}", "").Trim();

					}
				}
			}

			string fileName = filePath.Substring(filePath.LastIndexOf('/') + 1, filePath.LastIndexOf('.') - filePath.LastIndexOf('/') - 1);

			//save data to cache
			if (string.IsNullOrEmpty(pStr))
			{
				EditorUtility.DisplayDialog("Error", "Read " + fileName + " file failed，Read nothing!", "OK");
				throw new System.Exception("Read " + fileName + " error");
			}

			string[] rStr = pStr.Split('#');
			foreach (string s in rStr)
			{
				if (string.IsNullOrEmpty(s.Trim())) continue;
				string id = s.Substring(s.IndexOf('&') + 1, 9);
				if (s.StartsWith("Id=--- !u!1107"))
				{ //StateMachine
					stateMachineCache.Add(id, s);
				}
				else if (s.StartsWith("Id=--- !u!1102"))
				{//State
					stateCache.Add(id, s);
				}
				else if (s.StartsWith("Id=--- !u!1101"))
				{ //Transitions
					transitionCache.Add(id, s);
				}
			}

			//save data to Txt
			stateDic= SaveData(fileName, stateMachineCache, stateCache, transitionCache);

			sr.Close();
		}
		return stateDic;
	}
	private static Dictionary<string, StateInfo> SaveData(string fileName, Dictionary<string, string> stateMachineCache, Dictionary<string, string> stateCache, Dictionary<string, string> transitioncache)
	{
		Dictionary<string, StateInfo> states = new Dictionary<string, StateInfo>();//State
		Dictionary<string, TransitionInfo> transitions = new Dictionary<string, TransitionInfo>();//Transitions
		Dictionary<string, StateInfo> usedStateDic = new Dictionary<string, StateInfo>();

		foreach (string stateKey in stateCache.Keys)
		{
			StateInfo info = new StateInfo();
			info.Id = stateKey;
			string[] arr = stateCache[stateKey].Split('|');
			foreach (string s in arr)
			{
				string[] str = s.Split('=');
				switch (str[0].Trim())
				{
					case "m_Name":
						info.Name = str[1].Trim();
						break;
					case "m_Speed":
						info.Speed = str[1].Trim();
						break;
					case "m_Motions":
						string motion = AssetDatabase.GUIDToAssetPath(str[1].Trim());
						motion = motion.Substring(motion.LastIndexOf('/') + 1).Replace(".FBX", "");
						info.Motion = motion;
						break;
				}
			}
			if (!string.IsNullOrEmpty(info.Id))
			{
				if (states.ContainsKey(info.Id))
				{
					states.Remove(info.Id);
				}
				states.Add(info.Id, info);
			}
		}

		foreach (string tranKey in transitioncache.Keys)
		{
			TransitionInfo info = new TransitionInfo();
			info.Id = tranKey;
			string[] arr = transitioncache[tranKey].Split('|');
			foreach (string s in arr)
			{
				string[] str = s.Split('=');
				switch (str[0].Trim())
				{
					case "m_SrcState":
						info.SrcStateId = str[1].Trim();
						break;
					case "m_DstState":
						info.DestStateId = str[1].Trim();
						break;
					case "m_TransitionDuration":
						info.TransitionDuration = str[1].Trim();
						break;
					case "m_TransitionOffset":
						info.TransitionOffset = str[1].Trim();
						break;
					case "m_Conditions":
						if (!string.IsNullOrEmpty(str[1].Trim()))
						{
							List<Dictionary<string, string>> conditions = new List<Dictionary<string, string>>();
							string[] cstr = str[1].Trim().Split(',');
							foreach (string cc in cstr)
							{
								if (string.IsNullOrEmpty(cc.Trim())) continue;
								Dictionary<string, string> temp = new Dictionary<string, string>();
								string[] ccstr = cc.Trim().Split(';');
								foreach (string cr in ccstr)
								{
									string[] rr = cr.Trim().Split(':');
									temp.Add(rr[0].Trim(), rr[1].Trim());
								}
								conditions.Add(temp);
							}
							info.Conditions = conditions;
						}
						break;
				}
			}
			transitions.Add(info.Id, info);
		}

		//StateMachine
		foreach (string key in stateMachineCache.Keys)
		{
			Dictionary<string, string> topProperties = new Dictionary<string, string>();

			string[] arr = stateMachineCache[key].Split('|');
			foreach (string s in arr)
			{
				string[] str = s.Split('=');
				topProperties.Add(str[0].Trim(), str[1].Trim());
			}
			string stateMachineName = topProperties["m_Name"];//StateMacheName

			if (!Directory.Exists(AniControlAssetbundlePath + fileName))
			{
				Directory.CreateDirectory(AniControlAssetbundlePath  + fileName);
			}
			using (StreamWriter writer = new StreamWriter(AniControlAssetbundlePath + fileName + "/" + stateMachineName + ".txt", false, CommonFunc.GetCharsetEncoding()))
			{
				string defaultState = topProperties["m_DefaultState"];//defualut start State
				if (states.ContainsKey(defaultState))
				{
					writer.WriteLine("DefaultState:" + states[defaultState].Name);
				}
				string orderedTransitions = topProperties["m_OrderedTransitions"];//relationship
				//   ,first:;0/second:,first:;110205311/second:;110170080;110190653,
				string[] dArr = orderedTransitions.Split(',');
				foreach (string ds in dArr)
				{
					string[] dSubArr = ds.Split('/');
					foreach (string dds in dSubArr)
					{
						string[] dThirdArr = dds.Split(':');
						if (dThirdArr[0] == "first")
						{
							if (!string.IsNullOrEmpty(dThirdArr[1].Trim()))
							{
								string[] dFouthArr = dThirdArr[1].Split(';');
								foreach (string ddds in dFouthArr)
								{
									if (string.IsNullOrEmpty(ddds)) continue;
									if (states.ContainsKey(ddds))
									{
										StateInfo state = states[ddds];
										writer.WriteLine("[" + state.Name + "]");
										writer.WriteLine("\tSpeed:" + state.Speed);
										writer.WriteLine("\tMotion:" + state.Motion);

										if(!usedStateDic.ContainsKey(state.Name)) usedStateDic.Add(state.Name, state);
									}
								}
							}
						}
						else if (dThirdArr[0] == "second")
						{
							if (!string.IsNullOrEmpty(dThirdArr[1].Trim()))
							{
								string[] dFouthArr = dThirdArr[1].Split(';');
								foreach (string ddds in dFouthArr)
								{
									if (string.IsNullOrEmpty(ddds)) continue;

									writer.WriteLine("\tTransition:");

									if (states.ContainsKey(ddds))
									{
										StateInfo state = states[ddds];
										writer.WriteLine("\t\t" + state.Name);
									}
									if (transitions.ContainsKey(ddds))
									{
										TransitionInfo tran = transitions[ddds];
										StateInfo srcState = states.ContainsKey(tran.SrcStateId) ? states[tran.SrcStateId] : null;
										StateInfo destState = states.ContainsKey(tran.DestStateId) ? states[tran.DestStateId] : null;
										if (srcState != null)
										{
											writer.WriteLine("\t\tSrcState:" + srcState.Name);
										}
										else
										{
											writer.WriteLine("\t\tSrcState:null");
										}
										if (destState != null)
										{
											writer.WriteLine("\t\tDestState:" + destState.Name);
										}
										else
										{
											writer.WriteLine("\t\tDestState:null");
										}
										writer.WriteLine("\t\tTransitionDuration:" + tran.TransitionDuration);
										writer.WriteLine("\t\tTransitionOffset:" + tran.TransitionOffset);

										if (tran.Conditions != null)
										{
											bool hasExistTime = false;
											foreach (Dictionary<string, string> condition in tran.Conditions)
											{
												if (condition != null)
												{
													if (condition.ContainsKey("m_ConditionEvent"))
													{
														if ("IsBoy".Equals(condition["m_ConditionEvent"]))
														{
															writer.WriteLine("\t\tIsBoy:" + condition["m_EventTreshold"]);
														}
													}

													if (!hasExistTime && condition.ContainsKey("m_ExitTime"))
													{
														writer.WriteLine("\t\tExitTime:" + condition["m_ExitTime"]);
														hasExistTime = true;
													}
												}
											}

											if (!hasExistTime)
											{
												//Compatible old version
												Dictionary<string, string> dicTemp = tran.Conditions.Count > 0 ? tran.Conditions[0] : null;
												if (dicTemp != null)
												{
													writer.WriteLine("\t\tExitTime:" + dicTemp["m_ExitTime"]);
												}
											}
										}
									}
								}
							}
						}
					}
				}
				writer.Close();
			}
		}
		return usedStateDic;
	}

	private static void GenerateControllerAni(Dictionary<string, string> dic)
	{
		foreach (string motionName in dic.Keys)
		{
			string fbxSrcPath = DanceSrcDir + motionName + ".FBX";
			string fbxTempPath = DanceTempDir + motionName + ".FBX";

			if (File.Exists(fbxSrcPath))
			{
				File.Copy(fbxSrcPath, fbxTempPath, true);
			}
		}
		AssetDatabase.Refresh();

		foreach (string motionName in dic.Keys)
		{
			string fbxSrcPath = DanceSrcDir + motionName + ".FBX";
			string fbxTempPath = DanceTempDir + motionName + ".FBX";
			string fbxDestPath = UsedAniFBXPath + motionName + ".FBX";
			string bundleSrcPath = AniAssetbundlePath + motionName.ToUpper() + ".assetbundle";
			string bundleDestPath = UsedAniAssetbundlePath + motionName.ToUpper() + ".assetbundle";

			if (File.Exists(fbxSrcPath))
			{
				ProcAnimations_File(fbxTempPath);

				//Copy
				if (File.Exists(bundleSrcPath) && File.Exists(fbxSrcPath))
				{
					//Copy to Res/Animations/UsedAnimationFBX
					File.Copy(fbxSrcPath, fbxDestPath, true);

					//Copy to Res/Animations/UsedAnimationBundle
					File.Copy(bundleSrcPath, bundleDestPath, true);
				}
				else
				{
					Debug.LogError("Can not find animation bundle file," + motionName.ToUpper());
				}
			}
			else
			{
				if (dic[motionName] != "New State")
				{
					Debug.LogWarning("Can not find FBX file," + fbxSrcPath);
				}
			}
		}
	}

	private static void ProceAnimations_Dir(string dirPath)
	{
		if (Directory.Exists(dirPath))
		{
			string[] fileArr = Directory.GetFiles(dirPath, "*.FBX");
			foreach (string filePath in fileArr)
			{
				ProcAnimations_File(filePath);
			}

			string[] dirArr = Directory.GetDirectories(dirPath);
			foreach (string dir in dirArr)
			{
				if (!dir.Contains("/."))
				{
					ProceAnimations_Dir(dir);
				}
			}
		}
		else
		{
			Debug.Log("path is not exist: " + dirPath);
		}
	}

	private static void ProcAnimations_File(string filePath)
	{
		Object[] objArr = AssetDatabase.LoadAllAssetRepresentationsAtPath(filePath);
		if (objArr == null || objArr.Length == 0)
		{
			return;
		}

		GameObject characterFBX = (GameObject)AssetDatabase.LoadMainAssetAtPath(filePath);
		string bundleName = characterFBX.name.ToUpper();
		GameObject.Destroy(characterFBX);

		//Delete old asset;
		string[] bundleArr = Directory.GetFiles(AniAssetbundlePath, bundleName + ".assetbundle");
		foreach (string bundlePath in bundleArr)
		{
			File.Delete(bundlePath);
		}

		string tempAnimationAssetPath = "Assets/TempAnimation.asset";
		foreach (Object obj in objArr)
		{
			AnimationClip aniClip = obj as AnimationClip;
			if (aniClip != null)
			{
				AnimationClip newClip = RemoveScaleCurve(aniClip);
				if (newClip != null)
				{
					AssetDatabase.CreateAsset(newClip, tempAnimationAssetPath);

					string path = AniAssetbundlePath + bundleName + ".assetbundle";
					Object clipAsset = AssetDatabase.LoadAssetAtPath(tempAnimationAssetPath, typeof(AnimationClip));

					BuildAssetBundle.Build(clipAsset, null, path);
					Debug.Log("Saved " + bundleName + " animation");

					AssetDatabase.DeleteAsset(tempAnimationAssetPath);
				}
			}
		}
	}

	private static AnimationClip RemoveScaleCurve(AnimationClip aniClip)
	{
		AnimationClip newClip = new AnimationClip();
		newClip.name = aniClip.name;

		AnimationClipCurveData[] clipDataArr = AnimationUtility.GetAllCurves(aniClip, true);
		if (clipDataArr != null)
		{
			int count = 0;
			foreach (AnimationClipCurveData clipData in clipDataArr)
			{
				if (clipData.propertyName.Contains("LocalScale"))
				{
					++count;
				}
				else
				{
					newClip.SetCurve(clipData.path, clipData.type, clipData.propertyName, clipData.curve);
				}
			}

			Debug.Log("RemoveScaleCurve count:" + count);
		}

		return newClip;
	}
}
