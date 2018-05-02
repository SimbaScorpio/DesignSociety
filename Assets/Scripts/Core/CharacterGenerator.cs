using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignSociety
{
	public class CharacterGenerator : MonoBehaviour
	{
		private string hairPath = "Prefabs/Hair/";
		private string bodyPath = "Prefabs/Body/";

		private static CharacterGenerator instance;

		public static CharacterGenerator GetInstance ()
		{
			return instance;
		}

		void Awake ()
		{
			instance = this;
		}

		public void Generate (GameObject model, CharacterData data, bool player)
		{
			// 发型
			string hairName = GetHairName (data.hair);
			Transform hair = model.transform.Find ("hair");
			Transform[] children = hair.GetComponentsInChildren<Transform> ();
			for (int i = 1; i < children.Length; ++i)
				Destroy (children [i].gameObject);
			if (!player)
				Instantiate (Resources.Load (hairPath + hairName), hair);
			else
				Instantiate (Resources.Load (hairPath + hairName + "_player"), hair);

			// 体型
			string bodyName = GetBodyName (data.body_type);
			GameObject obj = (GameObject)Resources.Load (bodyPath + bodyName);
			Mesh mesh = obj.GetComponent<SkinnedMeshRenderer> ().sharedMesh;
			model.GetComponentInChildren<SkinnedMeshRenderer> ().sharedMesh = mesh;

			// 服装贴图
			Material clothMat;
			if (!player)
				clothMat = MaterialCollection.GetInstance ().GetMaterial (data.clothing);
			else
				clothMat = MaterialCollection.GetInstance ().GetAlphaMaterial (data.clothing);
			model.transform.Find ("mesh").GetComponent<Renderer> ().material = clothMat;

			// 眼镜
			string glassesName = GetGlasses (data.glasses);
			Transform glasses = model.transform.Find ("hip_ctrl/root/spline/right_chest/neck/head/glasses");
			if (glasses != null) {
				Transform[] trs = glasses.GetComponentsInChildren<Transform> ();
				for (int i = 1; i < trs.Length; ++i) {
					if (trs [i].name == glassesName)
						trs [i].gameObject.SetActive (true);
					else
						trs [i].gameObject.SetActive (false);
				}
			}
		}

		string GetHairName (int style)
		{
			if (style == 1) {
				return "hair_man_0";
			} else if (style == 2) {
				return "hair_man_1";
			} else if (style == 3) {
				return "hair_man_2";
			} else if (style == 4) {
				return "hair_woman_0";
			} else if (style == 5) {
				return "hair_woman_1";
			} else if (style == 6) {
				return "hair_woman_2";
			} else {
				return "hair_man_0";
			}
		}

		string GetBodyName (string type)
		{
			if (type == "男瘦") {
				return "body_man_0";
			} else if (type == "男胖") {
				return "body_man_1";
			} else if (type == "女裙") {
				return "body_woman_0";
			} else if (type == "女裤") {
				return "body_woman_1";
			} else {
				return "body_man_0";
			}
		}

		string GetGlasses (int style)
		{
			if (style == 1) {
				return "glasses_1";
			} else if (style == 2) {
				return "glasses_2";
			} else if (style == 3) {
				return "glasses_3";
			} else if (style == 4) {
				return "glasses_4";
			} else if (style == 5) {
				return "glasses_5";
			} else {
				return null;
			}
		}
	}
}