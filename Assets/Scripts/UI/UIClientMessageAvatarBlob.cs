using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DesignSociety
{
	public class UIClientMessageAvatarBlob : MonoBehaviour
	{
		public GameObject prefab;
		private List<GameObject> factory = new List<GameObject> ();

		#region instance

		private static UIClientMessageAvatarBlob instance;

		public static UIClientMessageAvatarBlob GetInstance ()
		{
			return instance;
		}

		void Awake ()
		{
			instance = this;
		}

		#endregion

		public void PushBlob (GameObject serverai, string state)
		{
			if (serverai == null)
				return;
			GameObject obj = Get ();
			CharacterData cdata = serverai.GetComponent<NetworkServerAI> ().character;
			RawImage avatar = obj.transform.Find ("Avatar").GetComponent<RawImage> ();
			avatar.texture = AvatarSnapManager.GetInstance ().GetAvatar (cdata);

			Text[] texts = obj.GetComponentsInChildren<Text> ();
			texts [1].text = cdata.chinesename;
			texts [2].text = cdata.job;
			if (!string.IsNullOrEmpty (state))
				texts [3].text = "正在" + state;
			else
				texts [3].text = "";

			obj.GetComponent<UITempNameSaver> ().text = cdata.name;

			UIClientMessageMenu.GetInstance ().PushMessage (obj, 1);
		}

		GameObject Get ()
		{
			GameObject obj;
			if (factory.Count > 0) {
				obj = factory [0];
				factory.RemoveAt (0);
			} else {
				obj = Instantiate (prefab) as GameObject;
			}
			return obj;
		}

		public void Remove (GameObject obj)
		{
			UITempNameSaver temp = obj.GetComponent<UITempNameSaver> ();
			AvatarSnapManager.GetInstance ().RemoveAvatar (temp.text);
			factory.Add (obj);
		}
	}
}