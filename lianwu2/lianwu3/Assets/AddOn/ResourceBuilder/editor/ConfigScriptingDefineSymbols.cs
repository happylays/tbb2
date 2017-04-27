using UnityEngine;
using System.Collections;
using UnityEditor;

public class ConfigScriptingDefineSymbols : MonoBehaviour {

	[MenuItem("Resource Generator/GenerateResource/ConfigSmallPack")]
	static void ConfigSmallPack()
	{
		string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
		if (string.IsNullOrEmpty(symbols))
		{
			symbols = "PACKAGE_BASIC";
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbols);
		}
		else if (!symbols.Contains("PACKAGE_BASIC"))
		{
			symbols += ";PACKAGE_BASIC";
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbols);
		}
	}
}
