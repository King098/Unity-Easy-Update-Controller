using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitController : MonoBehaviour {
	private static InitController m_self = null;
	public static InitController Instance
	{
		get
		{
			return m_self;
		}
	}

	void Awake()
	{
		m_self = this;
	}

	void Start()
	{
		UpdateData();
	}

	/// <summary>
	/// 更新数据
	/// </summary>
	public void UpdateData()
	{
		UpdateController.Instance.OnUpdateOver = OnUpdateOver;
		UpdateController.Instance.StartUpdate();
	}

	void OnUpdateOver()
	{
		Debug.Log ("Update Over");
	}

}