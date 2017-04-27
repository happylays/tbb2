using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Common;


public enum UISubSection
{
	UI_Texture,
	UI_Sound,

	Max,
}


public class UIConfig
{
	Dictionary<string, List<string>[]> m_ConfigData = new Dictionary<string, List<string>[]>();

	public void LoadConfig(string strOutFilePath, string strInFilePath)
	{
		string destPath = strOutFilePath;
		if (!File.Exists(strOutFilePath))
		{
			destPath = strInFilePath;
		}

		if (File.Exists(destPath))
		{
			using (StreamReader sr = new StreamReader(destPath, CommonFunc.GetCharsetEncoding()))
			{
				string strLine = null;
				char[] trimStart = { ' ', '\t' };
				char[] trimEnd = { ' ', '\r', '\n', '\t' };

				List<string>[] sectionData = null;
				List<string> subData = null;

				while ( ( strLine = sr.ReadLine() ) != null )
				{
					string strContent = XQConvert.Decrypt( strLine );
					
					strContent = strContent.TrimEnd( trimEnd );
					strContent = strContent.TrimStart( trimStart );
					
					if ( IsSection( ref strContent ) )
					{
						sectionData = GetSection( strContent );
					}
					else
					{
						if ( IsTextureSub( strContent ) )
						{
							subData = GetSubData( sectionData, UISubSection.UI_Texture );
						}
						else if ( IsSoundSub( strContent ) )
						{
							subData = GetSubData( sectionData, UISubSection.UI_Sound );
						}
						else if ( subData != null )
						{
							subData.Add( strContent );
						}
					}
				}

				sr.Close();
			}
		}
		else
		{
			Debug.LogError( "UI config file not exist: " + destPath );
		}
	}

	public void SaveConfig(string strFilePath)
	{
		using (StreamWriter sw = new StreamWriter(strFilePath, false, CommonFunc.GetCharsetEncoding())) 
		{
			foreach( KeyValuePair<string, List<string>[]> curSection in m_ConfigData )
			{
				string strContent = "[" + curSection.Key + "]";
				string strLine = XQConvert.Encrypt( strContent );
				sw.WriteLine( strLine );

				strContent = "\tTexture:";
				strLine = XQConvert.Encrypt( strContent ) ;
				sw.WriteLine( strLine );

				List<string> textureData = GetSubData( curSection.Value, UISubSection.UI_Texture );
				foreach( string textureName in textureData )
				{
					strContent = "\t\t" + textureName ;
					strLine = XQConvert.Encrypt( strContent ) ;
					sw.WriteLine( strLine );
				}

				strContent = "\tSound:";
				strLine = XQConvert.Encrypt( strContent );
				sw.WriteLine( strLine );

				List<string> soundData = GetSubData( curSection.Value, UISubSection.UI_Sound );
				foreach( string soundName in soundData )
				{
					strContent = "\t\t" + soundName;
					strLine = XQConvert.Encrypt( strContent ) ;
					sw.WriteLine( strLine );
				}
			}

			sw.Close();
		}
	}

	public void ClearSection(string sectionName)
	{
		m_ConfigData.Remove( sectionName );
	}

	public void AddConfig(string sectionName, UISubSection subType, string configData)
	{
		List<string>[] sectionData = GetSection( sectionName );
		if ( sectionData != null )
		{
			List<string> subData = GetSubData( sectionData, subType );
			if ( subData != null )
			{
				subData.Add( configData );
			}
		}
	}

	public List<string> GetConfig(string sectionName, UISubSection subType)
	{
		if ( m_ConfigData.ContainsKey( sectionName ) )
		{
			List<string>[] sectionData = m_ConfigData[sectionName];
			if ( sectionData != null )
			{
				return GetSubData( sectionData, subType );
			}
		}

		return null;
	}

	bool IsSection(ref string curline)
	{
		if ( curline.Length > 0 && curline[0] == '[' && curline[curline.Length - 1] == ']' )
		{
			curline = curline.Substring( 1, curline.Length - 2 );
			return true;
		}

		return false;
	}

	bool IsTextureSub(string curline)
	{
		if ( curline.StartsWith( "Texture:" ) )
		{
			return true;
		}

		return false;
	}

	bool IsSoundSub( string curline )
	{
		if ( curline.StartsWith( "Sound:" ) )
		{
			return true;
		}

		return false;
	}

	List<string>[] GetSection(string sectionName)
	{
		List<string>[] sectionData = null;

		if ( m_ConfigData.ContainsKey( sectionName ) )
		{
			sectionData = m_ConfigData[sectionName];
		}
		else
		{
			sectionData = new List<string>[(int)UISubSection.Max];
			sectionData[(int)UISubSection.UI_Texture] = new List<string>();
			sectionData[(int)UISubSection.UI_Sound] = new List<string>();
			m_ConfigData.Add( sectionName, sectionData );
		}

		return sectionData;
	}

	List<string> GetSubData(List<string>[] sectionData, UISubSection subType)
	{
		if ( sectionData != null )
		{
			if ( subType == UISubSection.UI_Texture )
			{
				return sectionData[(int)UISubSection.UI_Texture];
			}
			else if ( subType == UISubSection.UI_Sound )
			{
				return sectionData[(int)UISubSection.UI_Sound];
			}
		}

		return null;
	}
}
