// Toony Colors Pro+Mobile 2
// (c) 2014-2016 Jean Moreno

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// Graphical User Interface helper functions

public static class TCP2_GUI
{
	static private GUIStyle _EnabledLabel;
	static private GUIStyle EnabledLabel
	{
		get
		{
			if(_EnabledLabel == null)
			{
				_EnabledLabel = new GUIStyle(EditorStyles.label);
				_EnabledLabel.normal.background = EnabledBg;
			}
			return _EnabledLabel;
		}
	}

	static private GUIStyle _HelpIcon;
	static public GUIStyle HelpIcon
	{
		get
		{
			if(_HelpIcon == null)
			{
				_HelpIcon = new GUIStyle(EditorStyles.label);
				_HelpIcon.fixedWidth = 16;
				_HelpIcon.fixedHeight = 16;

				string rootPath = TCP2_Utils.FindReadmePath(true);
				Texture2D icon = AssetDatabase.LoadAssetAtPath("Assets" + rootPath + "/Editor/Icons/TCP2_HelpIcon.png", typeof(Texture2D)) as Texture2D;
				Texture2D icon_down = AssetDatabase.LoadAssetAtPath("Assets" + rootPath + "/Editor/Icons/TCP2_HelpIcon_Down.png", typeof(Texture2D)) as Texture2D;
				if(icon == null || icon_down == null) return null;

				_HelpIcon.normal.background = icon;
				_HelpIcon.active.background = icon_down;
			}

			return _HelpIcon;
		}
	}

	static private Texture2D _EnabledBg;
	static public Texture2D EnabledBg
	{
		get
		{
			if(_EnabledBg == null)
			{
				string rootPath = TCP2_Utils.FindReadmePath(true);
				_EnabledBg = AssetDatabase.LoadAssetAtPath("Assets" + rootPath + "/Editor/Icons/TCP2_EnabledBg.png", typeof(Texture2D)) as Texture2D;
			}
			return _EnabledBg;
		}
	}

	static private GUIStyle _CogIcon;
	static public GUIStyle CogIcon
	{
		get
		{
			if(_CogIcon == null)
			{
				_CogIcon = new GUIStyle(EditorStyles.label);
				_CogIcon.fixedWidth = 16;
				_CogIcon.fixedHeight = 16;
				
				string rootPath = TCP2_Utils.FindReadmePath(true);
				Texture2D icon = AssetDatabase.LoadAssetAtPath("Assets" + rootPath + "/Editor/Icons/TCP2_CogIcon.png", typeof(Texture2D)) as Texture2D;
				Texture2D icon_down = AssetDatabase.LoadAssetAtPath("Assets" + rootPath + "/Editor/Icons/TCP2_CogIcon_Down.png", typeof(Texture2D)) as Texture2D;
				if(icon == null || icon_down == null) return null;
				
				_CogIcon.normal.background = icon;
				_CogIcon.active.background = icon_down;
			}
			
			return _CogIcon;
		}
	}

	static private GUIStyle _HeaderLabel;
	static private GUIStyle _HeaderLabelPro;
	static private GUIStyle HeaderLabel
	{
		get
		{
			if(_HeaderLabel == null)
			{
				_HeaderLabel = new GUIStyle(EditorStyles.label);
				_HeaderLabel.fontStyle = FontStyle.Bold;
				_HeaderLabel.normal.textColor = new Color(0.35f,0.35f,0.35f);

				_HeaderLabelPro = new GUIStyle(_HeaderLabel);
				_HeaderLabelPro.normal.textColor = new Color(0.7f,0.7f,0.7f);
			}
			return EditorGUIUtility.isProSkin ? _HeaderLabelPro : _HeaderLabel;
		}
	}

	static private GUIStyle _SubHeaderLabel;
	static private GUIStyle _SubHeaderLabelPro;
	static private GUIStyle SubHeaderLabel
	{
		get
		{
			if (_SubHeaderLabel == null)
			{
				_SubHeaderLabel = new GUIStyle(EditorStyles.label);
				_SubHeaderLabel.fontStyle = FontStyle.Normal;
				_SubHeaderLabel.normal.textColor = new Color(0.35f, 0.35f, 0.35f);

				_SubHeaderLabelPro = new GUIStyle(_SubHeaderLabel);
				_SubHeaderLabelPro.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
			}
			return EditorGUIUtility.isProSkin ? _SubHeaderLabelPro : _SubHeaderLabel;
		}
	}

	static private GUIStyle _BigHeaderLabel;
	static private GUIStyle BigHeaderLabel
	{
		get
		{
			if(_BigHeaderLabel == null)
			{
				_BigHeaderLabel = new GUIStyle(EditorStyles.largeLabel);
				_BigHeaderLabel.fontStyle = FontStyle.Bold;
				_BigHeaderLabel.fixedHeight = 30;
			}
			return _BigHeaderLabel;
		}
	}

	static public GUIStyle _LineStyle;
	static public GUIStyle LineStyle
	{
		get
		{
			if(_LineStyle == null)
			{
				_LineStyle = new GUIStyle();
				_LineStyle.normal.background = EditorGUIUtility.whiteTexture;
				_LineStyle.stretchWidth = true;
			}
			
			return _LineStyle;
		}
	}

	//--------------------------------------------------------------------------------------------------
	// HELP

	static public void HelpButton(Rect rect, string helpTopic, string helpAnchor = null)
	{
		if(TCP2_GUI.Button(rect, HelpIcon, "?", "Help about:\n" + helpTopic))
		{
			OpenHelpFor(string.IsNullOrEmpty(helpAnchor) ? helpTopic : helpAnchor);
		}
	}
	static public void HelpButton(string helpTopic, string helpAnchor = null)
	{
		if(TCP2_GUI.Button(HelpIcon, "?", "Help about:\n" + helpTopic))
		{
			OpenHelpFor(string.IsNullOrEmpty(helpAnchor) ? helpTopic : helpAnchor);
		}
	}

	static public void OpenHelpFor(string helpTopic)
	{
		string rootDir = TCP2_Utils.FindReadmePath();
		if(rootDir == null)
		{
			EditorUtility.DisplayDialog("TCP2 Documentation", "Couldn't find TCP2 root folder! (the readme file is missing)\nYou can still access the documentation manually in the Documentation folder.", "Ok");
		}
		else
		{
			string helpAnchor = helpTopic.Replace("/", "_").Replace(@"\", "_").Replace(" ", "_").ToLowerInvariant() + ".htm";
			string topicLink = "file:///" + rootDir.Replace(@"\", "/") + "/Documentation/Documentation Data/Anchors/" + helpAnchor;
			Application.OpenURL(topicLink);
		}
	}

	static public void OpenHelp()
	{
		string rootDir = TCP2_Utils.FindReadmePath();
		if(rootDir == null)
		{
			EditorUtility.DisplayDialog("TCP2 Documentation", "Couldn't find TCP2 root folder! (the readme file is missing)\nYou can still access the documentation manually in the Documentation folder.", "Ok");
		}
		else
		{
			string helpLink = "file:///" + rootDir.Replace(@"\", "/") + "/Documentation/TCP2 Documentation.html";
			Application.OpenURL(helpLink);
		}
	}

	//--------------------------------------------------------------------------------------------------
	//GUI Functions
	
	static public void Separator()
	{
		GUILayout.Space(4);
		GUILine(new Color(.3f,.3f,.3f), 1);
		GUILine(new Color(.9f,.9f,.9f), 1);
		GUILayout.Space(4);
	}

	static public void Separator(Rect position)
	{
		Rect lineRect = position;
		lineRect.height = 1;
		GUILine(lineRect, new Color(.3f, .3f, .3f), 1);
		lineRect.y += 1;
		GUILine(lineRect, new Color(.9f, .9f, .9f), 1);
	}

	static public void SeparatorBig()
	{
		GUILayout.Space(10);
		GUILine(new Color(.3f,.3f,.3f), 2);
		GUILayout.Space(1);
		GUILine(new Color(.3f,.3f,.3f), 2);
		GUILine(new Color(.85f,.85f,.85f), 1);
		GUILayout.Space(2);
	}

	static public void GUILine(float height = 2f)
	{
		GUILine(Color.black, height);
	}
	static public void GUILine(Color color, float height = 2f)
	{
		Rect position = GUILayoutUtility.GetRect(0f, float.MaxValue, height, height, LineStyle);
		
		if(Event.current.type == EventType.Repaint)
		{
			Color orgColor = GUI.color;
			GUI.color = orgColor * color;
			LineStyle.Draw(position, false, false, false, false);
			GUI.color = orgColor;
		}
	}
	static public void GUILine( Rect position, Color color, float height = 2f )
	{
		if (Event.current.type == EventType.Repaint)
		{
			Color orgColor = GUI.color;
			GUI.color = orgColor * color;
			LineStyle.Draw(position, false, false, false, false);
			GUI.color = orgColor;
		}
	}

	//----------------------

	static public void Header(string header, string tooltip = null, bool expandWidth = false)
	{
		if(tooltip != null)
			EditorGUILayout.LabelField(new GUIContent(header, tooltip), HeaderLabel, GUILayout.ExpandWidth(expandWidth));
		else
			EditorGUILayout.LabelField(header, HeaderLabel, GUILayout.ExpandWidth(expandWidth));
	}

	static public void Header( Rect position, string header, string tooltip = null, bool expandWidth = false )
	{
		if (tooltip != null)
			EditorGUI.LabelField(position, new GUIContent(header, tooltip), HeaderLabel);
		else
			EditorGUI.LabelField(position, header, HeaderLabel);
	}

	static public void SubHeaderGray(string header, string tooltip = null, bool expandWidth = false)
	{
		if (tooltip != null)
			EditorGUILayout.LabelField(new GUIContent(header, tooltip), SubHeaderLabel, GUILayout.ExpandWidth(expandWidth));
		else
			EditorGUILayout.LabelField(header, SubHeaderLabel, GUILayout.ExpandWidth(expandWidth));
	}

	static public void HeaderAndHelp(string header, string helpTopic)
	{
		HeaderAndHelp(header, null, helpTopic);
	}
	static public void HeaderAndHelp(string header, string tooltip, string helpTopic)
	{
		GUILayout.BeginHorizontal();
		Rect r = GUILayoutUtility.GetRect(new GUIContent(header, tooltip), EditorStyles.label, GUILayout.ExpandWidth(true));
		Rect btnRect = r;
		btnRect.width = 16;
		//Button
		if(GUI.Button(btnRect, new GUIContent("", "Help about:\n" + helpTopic), HelpIcon))
			OpenHelpFor(helpTopic);
		//Label
		r.x += 16;
		r.width -= 16;
		GUI.Label(r, new GUIContent(header, tooltip), EditorStyles.boldLabel);
		GUILayout.EndHorizontal();
	}
	static public void HeaderAndHelp( Rect position, string header, string tooltip, string helpTopic )
	{
		if (!string.IsNullOrEmpty(helpTopic))
		{
			Rect btnRect = position;
			btnRect.width = 16;
			//Button
			if (GUI.Button(btnRect, new GUIContent("", "Help about:\n" + helpTopic), HelpIcon))
				OpenHelpFor(helpTopic);
		}

		//Label
		position.x += 16;
		position.width -= 16;
		GUI.Label(position, new GUIContent(header, tooltip), EditorStyles.boldLabel);
	}

	static public void HeaderBig(string header, string tooltip = null)
	{
		if(tooltip != null)
			EditorGUILayout.LabelField(new GUIContent(header, tooltip), BigHeaderLabel);
		else
			EditorGUILayout.LabelField(header, BigHeaderLabel);
	}

	static public void SubHeader(string header, string tooltip = null, float width = 146f)
	{
		SubHeader(header, tooltip, false, width);
	}
	static public void SubHeader(string header, string tooltip, bool highlight, float width)
	{
		if(tooltip != null)
			GUILayout.Label(new GUIContent(header, tooltip), highlight ? EnabledLabel : EditorStyles.label, GUILayout.Width(width));
		else
			GUILayout.Label(header, highlight ? EnabledLabel : EditorStyles.label, GUILayout.Width(width));
	}

	//----------------------
	
	static public bool Button(GUIStyle icon, string noIconText, string tooltip = null)
	{
		if(icon == null)
			return GUILayout.Button(new GUIContent(noIconText, tooltip), EditorStyles.miniButton);
		else
			return GUILayout.Button(new GUIContent("", tooltip), icon);
	}

	static public bool Button(Rect rect, GUIStyle icon, string noIconText, string tooltip = null)
	{
		if(icon == null)
			return GUI.Button(rect, new GUIContent(noIconText, tooltip), EditorStyles.miniButton);
		else
			return GUI.Button(rect, new GUIContent("", tooltip), icon);
	}

	static public int RadioChoice(int choice, bool horizontal, params string[] labels)
	{
		var guiContents = new GUIContent[labels.Length];
		for(int i = 0; i < guiContents.Length; i++)
		{
			guiContents[i] = new GUIContent(labels[i]);
		}
		return RadioChoice(choice, horizontal, guiContents);
	}
	static public int RadioChoice(int choice, bool horizontal, params GUIContent[] labels)
	{
		if(horizontal)
			EditorGUILayout.BeginHorizontal();

		for(int i = 0; i < labels.Length; i++)
		{
			GUIStyle style = EditorStyles.miniButton;
			if(labels.Length > 1)
			{
				if(i == 0)
					style = EditorStyles.miniButtonLeft;
				else if(i == labels.Length-1)
					style = EditorStyles.miniButtonRight;
				else
					style = EditorStyles.miniButtonMid;
			}
			
			if(GUILayout.Toggle(i == choice, labels[i], style))
			{
				choice = i;
			}
		}

		if(horizontal)
			EditorGUILayout.EndHorizontal();

		return choice;
	}

	static public int RadioChoiceHorizontal( Rect position, int choice, params GUIContent[] labels )
	{
		for (int i = 0; i < labels.Length; i++)
		{
			Rect rI = position;
			rI.width /= labels.Length;
			rI.x += i * rI.width;
			if (GUI.Toggle(rI, choice == i, labels[i], (i == 0) ? EditorStyles.miniButtonLeft : (i == labels.Length - 1) ? EditorStyles.miniButtonRight : EditorStyles.miniButtonMid))
			{
				choice = i;
			}
		}

		return choice;
	}
}

//===================================================================================================================================================================
// Material Property Drawers

public class TCP2HeaderHelpDecorator : MaterialPropertyDrawer
{
	protected readonly string header;
	protected readonly string help;

	public TCP2HeaderHelpDecorator( string header )
	{
		this.header = header;
		this.help = null;
	}
	public TCP2HeaderHelpDecorator( string header, string help )
	{
		this.header = header;
		this.help = help;
	}

	public override void OnGUI( Rect position, MaterialProperty prop, string label, MaterialEditor editor )
	{
		TCP2_GUI.HeaderAndHelp(position, this.header, null, this.help);
	}

	public override float GetPropertyHeight( MaterialProperty prop, string label, MaterialEditor editor )
	{
		return 18f;
	}
}

//---------------------------------------------------------------------------------------------------------------------

public class TCP2SeparatorDecorator : MaterialPropertyDrawer
{
	public override void OnGUI( Rect position, MaterialProperty prop, string label, MaterialEditor editor )
	{
		position.y += 4;
		position.height -= 4;
		TCP2_GUI.Separator(position);
	}

	public override float GetPropertyHeight( MaterialProperty prop, string label, MaterialEditor editor )
	{
		return 12f;
	}
}

//---------------------------------------------------------------------------------------------------------------------

public class TCP2OutlineNormalsGUIDrawer : MaterialPropertyDrawer
{
	readonly GUIContent[] labels = new GUIContent[]
	{
			new GUIContent("Regular", "Use regular vertex normals"),
			new GUIContent("Vertex Colors", "Use vertex colors as normals (with smoothed mesh)"),
			new GUIContent("Tangents", "Use tangents as normals (with smoothed mesh)"),
			new GUIContent("UV2", "Use second texture coordinates as normals (with smoothed mesh)"),
	};
	readonly string[] keywords = new string[] { "TCP2_NONE", "TCP2_COLORS_AS_NORMALS", "TCP2_TANGENT_AS_NORMALS", "TCP2_UV2_AS_NORMALS" };

	public override void OnGUI( Rect position, MaterialProperty prop, string label, MaterialEditor editor )
	{
		TCP2_GUI.Header("OUTLINE NORMALS", "Defines where to take the vertex normals from to draw the outline.\nChange this when using a smoothed mesh to fill the gaps shown in hard-edged meshes.");

		Rect r = EditorGUILayout.GetControlRect();
		r = EditorGUI.IndentedRect(r);
		int index = GetCurrentIndex(prop);
		EditorGUI.BeginChangeCheck();
		index = TCP2_GUI.RadioChoiceHorizontal(r, index, labels);
		if (EditorGUI.EndChangeCheck())
		{
			SetKeyword(prop, index);
		}
	}

	public override float GetPropertyHeight( MaterialProperty prop, string label, MaterialEditor editor )
	{
		return 0f;
	}

	int GetCurrentIndex( MaterialProperty prop )
	{
		int index = 0;
		Object[] targets = prop.targets;
		foreach (var t in targets)
		{
			Material m = (Material)t;
			for (int i = 0; i < keywords.Length; i++)
			{
				if (m.IsKeywordEnabled(keywords[i]))
				{
					return i;
				}
			}
		}
		return index;
	}

	private void SetKeyword( MaterialProperty prop, int index )
	{
		string label = prop.targets.Length > 1 ? string.Format("modify Outline Normals of {0} Materials", prop.targets.Length) : string.Format("modify Outline Normals of {0}", prop.targets[0].name);
		Undo.RecordObjects(prop.targets, label);
		for (int i = 0; i < this.keywords.Length; i++)
		{
			string keywordName = keywords[i];
			UnityEngine.Object[] targets = prop.targets;
			for (int j = 0; j < targets.Length; j++)
			{
				Material material = (Material)targets[j];
				if (index == i)
				{
					material.EnableKeyword(keywordName);
				}
				else
				{
					material.DisableKeyword(keywordName);
				}
			}
		}
	}
}

//---------------------------------------------------------------------------------------------------------------------

public class TCP2HeaderDecorator : MaterialPropertyDrawer
{
	protected readonly string header;

	public TCP2HeaderDecorator( string header )
	{
		this.header = header;
	}

	public override void OnGUI( Rect position, MaterialProperty prop, string label, MaterialEditor editor )
	{
		position.y += 2;
		TCP2_GUI.Header(position, header);
	}

	public override float GetPropertyHeight( MaterialProperty prop, string label, MaterialEditor editor )
	{
		return 18f;
	}
}

//---------------------------------------------------------------------------------------------------------------------

public class TCP2KeywordFilterDrawer : MaterialPropertyDrawer
{
	protected readonly string[] keywords;

	public TCP2KeywordFilterDrawer( string keyword )
	{
		this.keywords = keyword.Split(',');
	}

	public override void OnGUI( Rect position, MaterialProperty prop, string label, MaterialEditor editor )
	{
		if (IsValid(editor))
		{
			EditorGUI.indentLevel++;
			editor.DefaultShaderProperty(prop, label);
			EditorGUI.indentLevel--;
		}
	}

	public override float GetPropertyHeight( MaterialProperty prop, string label, MaterialEditor editor )
	{
		//There's still a small space if we return 0, -2 seems to get rid of that
		return -2f;
	}

	bool IsValid( MaterialEditor editor )
	{
		bool valid = false;
		if (editor.target != null && editor.target is Material)
		{
			foreach (var kw in keywords)
				valid |= (editor.target as Material).IsKeywordEnabled(kw);
		}
		return valid;
	}
}
