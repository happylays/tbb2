using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ToolExtend : Editor 
{
    /**NGUI Label 相关工具集.**/
    [MenuItem("Tool Extend/NGUI Wizard/Label")]
	static void ChangeLabel()
	{
        EditorWindow.GetWindow<ChangeLabelFontWizard>(false, "Label(NGUI)", true);
	}

    /**NGUI Sprite 相关工具集.**/
    [MenuItem("Tool Extend/NGUI Wizard/Sprite")]
	static void ChangeSprite()
	{
        EditorWindow.GetWindow<ChangeSpriteWizard>(false, "Sprite(NGUI)", true);
	}

	/**NGUI Sprite 相关工具集.**/
	[MenuItem("Tool Extend/GenerateUIUtility")]
	static void BuildMarkedUI()
	{
		EditorWindow.GetWindow<BuildMarkedUI>(true, "UI编译工具", true);
	}

}