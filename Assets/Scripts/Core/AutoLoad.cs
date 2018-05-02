using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DesignSociety
{
	[System.Serializable]
	class IP
	{
		public string ip = "";
		public bool isserver = false;
		public int width = 1920;
		public int height = 1080;
	}

	public class AutoLoad : MonoBehaviour
	{
		private NetworkManager manager;
		private IP ip;

		#region instance

		private static AutoLoad instance;

		public static AutoLoad GetInstance ()
		{
			return instance;
		}

		void Awake ()
		{
			instance = this;
		}

		#endregion

		void Start ()
		{
			manager = FindObjectOfType<NetworkManager> ();
		}

		void SetFullScreen ()
		{
			Screen.fullScreen = true;
			Screen.SetResolution (ip.width, ip.height, true);
		}

		void Fire ()
		{
			SetFullScreen ();
			if (ip.isserver) {
				manager.StartServer ();
				FileManager.GetInstance ().StartLoading ();
			} else {
				manager.networkAddress = ip.ip;
				manager.StartClient ();
			}
		}

		public void LoadIpData ()
		{
			StartCoroutine (LoadIpDataCoroutine (Global.IpConfigJsonURL));
		}

		IEnumerator LoadIpDataCoroutine (string jsonurl)
		{
			WWW www = new WWW (jsonurl);
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				Log.error ("无法打开IP文件");
			} else {
				string json = www.text;
				ip = JsonUtility.FromJson<IP> (json);
				Fire ();
			}
		}
	}
}