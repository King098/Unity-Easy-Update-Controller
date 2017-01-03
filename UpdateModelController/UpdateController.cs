using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateController : MonoBehaviour {
	public string update_link;
	public string download_target;
	public delegate void OnVoid();
	public delegate void OnObject(object data);
	public delegate void OnOver(object data_1, object data_2);
	//更新日志出错
	public OnObject OnUpdateTableError;
	//更新日志成功
	public OnObject OnUpdateTableOver;
	//更新数据出错
	public OnObject OnUpdateDataError;
	//更新数据成功
	public OnOver OnUpdateDataOver;
	//所有更新都完成了
	public OnVoid OnUpdateOver;

	private bool isDownLoading = false;

//	public Image img;

	private static UpdateController m_self = null;
	public static UpdateController Instance
	{
		get
		{
			return m_self;
		}
	}

	void Awake()
	{
		m_self = this;
		#if UNITY_EDITOR
		download_target = Application.dataPath;
		#elif UNITY_ANDROID
		download_target = Application.persistentDataPath;
		#endif
	}

	public void StartUpdate()
	{
		if(isDownLoading)
			return;
		isDownLoading = true;
		OnUpdateTableError += UpdateTableError;
		OnUpdateTableOver += UpdateTableOver;
		OnUpdateDataError += UpdateDataError;
		OnUpdateDataOver += UpdateDataOver;
		CancelDownload();
		StartCoroutine(DownLoadUpdateInfo(update_link));
	}

	IEnumerator DownLoadUpdateInfo(string link)
	{
		//InitLoadingUI.Instance.SetLabel("从" + link + "下载更新数据");
		//InitLoadingUI.Instance.ShowSlider(0f,1f);
		WWW www = new WWW(link);
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			if(OnUpdateTableError != null)
			{
				OnUpdateTableError(www.error);
			}
		}
		else
		{
			if(OnUpdateTableOver != null)
			{
				OnUpdateTableOver(www.text);
			}
		}
	}

	void UpdateTableError(object data)
	{
		//InitLoadingUI.Instance.SetLabel((string)data);
		Debug.Log ((string)data);
		//错误就直接加载没有网络的资源
		StopAllCoroutines();
//		InitController.Instance.NoNetworkLoad();
	}

	void UpdateTableOver(object data)
	{
		UpdateTable update_info = new UpdateTable((string)data);
		StartCoroutine (UpdateAll(update_info));
	}

	void UpdateDataError(object data)
	{
		Debug.Log ((string)data);
		//错误就直接跳过
//		if(OnUpdateOver != null)
//		{
//			OnUpdateOver();
//		}
		StopAllCoroutines();
//		InitController.Instance.UpdateErrorLoad();
	}

	void UpdateDataOver(object info,object bytes)
	{
		System.IO.File.WriteAllBytes(download_target + "/" + ((UpdateInfo)info).name,(byte[])bytes);
		//记录当前更新的版本
		NativeDataController.Instance.UpdateVersion(((UpdateInfo)info).name,((UpdateInfo)info).version);
		//InitLoadingUI.Instance.SetLabel("更新" + ((UpdateInfo)info).name + "完成");
		Debug.Log ("更新" + ((UpdateInfo)info).name + "完成");
	}

	void UpdateOver()
	{
		Debug.Log ("更新全部完成");
	}

	IEnumerator UpdateAll(UpdateTable update_info)
	{
		//InitLoadingUI.Instance.SetLabel("开始检测更新");
		for(int i = 0;i<update_info.list.Count;i++)
		{
			//如果本地存在资源将不更新
			// if(update_info.list[i].type == UpdateType.Model)
			// {
			// 	if(PetStateChang.currenPetModel != null)
			// 	{
			// 		if(update_info.list[i].name != "pet_" + PetStateChang.currenPetModel.sex.ToString() + ".assetbundle")
			// 		{
			// 			continue;
			// 		}
			// 	}
			// 	else
			// 	{
			// 		if(update_info.list[i].name != UpdateController.Instance.test_ab_name + ".assetbundle")
			// 		{
			// 			continue;
			// 		}
			// 	}
			// }

			// if(update_info.list[i].type == UpdateType.Dress)
			// {
			// 	if(PetStateChang.currenPetModel != null)
			// 	{
			// 		if(!update_info.list[i].name.StartsWith(PetStateChang.currenPetModel.sex.ToString()))
			// 		{
			// 			continue;
			// 		}
			// 	}
			// 	else
			// 	{
			// 		if(!update_info.list[i].name.StartsWith(UpdateController.Instance.test_ab_name.Replace("pet_","")))
			// 		{
			// 			continue;
			// 		}
			// 	}
			// }

			if(!CheckNeedUpdate(update_info.list[i]))
			{
				//InitLoadingUI.Instance.ShowSlider(i + 1f,(float)update_info.list.Count);
				continue;
			}
			//InitLoadingUI.Instance.SetLabel("正在更新" + update_info.list[i].name);
			#if UNITY_EDITOR
			WWW www = new WWW(update_info.list[i].link);
			#elif UNITY_ANDROID
			WWW www = new WWW(update_info.list[i].android_link);
			#endif
			yield return www;
			if(!string.IsNullOrEmpty(www.error))
			{
				if(OnUpdateDataError != null)
				{
					OnUpdateDataError(www.error);
				}
			}
			else
			{
				if(OnUpdateDataOver != null)
				{
					//InitLoadingUI.Instance.ShowSlider(i + 1f,(float)update_info.list.Count);
					OnUpdateDataOver(update_info.list[i],www.bytes);
				}
			}
		}
		yield return new WaitForEndOfFrame();
		if(OnUpdateOver != null)
		{
			//InitLoadingUI.Instance.ShowSlider(1f,1f);
			OnUpdateOver();
		}
		isDownLoading = false;
	}

	public void CancelDownload()
	{
		StopAllCoroutines();
		isDownLoading = false;
	}
	
	bool CheckNeedUpdate(UpdateInfo info)
	{
		if(NativeDataController.Instance.HasVersion(info.name))
		{
			if(NativeDataController.Instance.GetVersion(info.name) != info.version)
			{
				return true;
			}
			return false;
		}
		else
		{
			return true;
		}
	}
}
