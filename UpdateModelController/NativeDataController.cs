using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NativeDataController {
	private static NativeDataController m_self = null;
	public static NativeDataController Instance
	{
		get
		{
			if(m_self == null)
			{
				m_self = new NativeDataController();
			}
			return m_self;
		}
	}

	public delegate void OnVoid();
	public OnVoid NativeDataLoadOver;

	public NativeData Data;

	public void LoadNativaData()
	{
		Data = new NativeData(PlayerPrefs.GetString("version",""));
		if(NativeDataLoadOver != null)
		{
			NativeDataLoadOver();
		}
	}


	public void UpdateVersion(string name,string version)
	{
		if(Data.native_update_version.ContainsKey(name))
		{
			Data.native_update_version[name] = version;
		}
		else
		{
			Data.native_update_version.Add(name,version);
		}
		SaveAllData();
	}


	public void SaveAllData()
	{
		PlayerPrefs.SetString("version",Data.GetVersionString());
	}

	public bool HasVersion(string ver)
	{
		return Data.native_update_version.ContainsKey(ver);
	}

	public string GetVersion(string key)
	{
		if(HasVersion(key))
		{
			return Data.native_update_version[key];
		}
		return "";
	}
}


public class NativeData
{
	/// <summary>
	/// NativeDataVersion
	/// </summary>
	public Dictionary<string,string> native_update_version;

	public NativeData(string version)
	{
		native_update_version = new Dictionary<string, string>();
		string[] ver = version.Split(';');
		for(int i = 0;i<ver.Length;i++)
		{
			string[] pair = ver[i].Split(':');
			if(pair.Length == 2)
			{
				native_update_version.Add(pair[0],pair[1]);
			}
		}
	}

	public string GetVersionString()
	{
		string result = "";
		foreach(string k in native_update_version.Keys)
		{
			result += k + ":" + native_update_version[k] + ";";
		}
		return result.Length > 0 ? result.Substring(0,result.Length - 1) : "";
	}
}