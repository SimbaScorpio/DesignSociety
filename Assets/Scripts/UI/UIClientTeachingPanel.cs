using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignSociety
{
	public class UIClientTeachingPanel : MonoBehaviour
	{
		public List<GameObject> idles;
		public List<GameObject> teleporting;
		public List<GameObject> landing;
		public GameObject specialTeaching;

		#region instance

		private static UIClientTeachingPanel instance;

		public static UIClientTeachingPanel GetInstance ()
		{
			return instance;
		}

		void Awake ()
		{
			instance = this;
		}

		#endregion

		public void OnIdle ()
		{
			for (int i = 0; i < idles.Count; ++i) {
				idles [i].SetActive (true);
			}
			for (int i = 0; i < teleporting.Count; ++i) {
				teleporting [i].SetActive (false);
			}
			for (int i = 0; i < landing.Count; ++i) {
				landing [i].SetActive (false);
			}
			if (specialTeaching != null) {
				UIClientDetailPanels.GetInstance ().OnOpenButtonClicked (specialTeaching);
			}
		}

		public void OnMoving ()
		{
			for (int i = 0; i < idles.Count; ++i) {
				idles [i].SetActive (false);
			}
			for (int i = 0; i < teleporting.Count; ++i) {
				teleporting [i].SetActive (false);
			}
			for (int i = 0; i < landing.Count; ++i) {
				landing [i].SetActive (false);
			}
		}

		public void OnTeleporting ()
		{
			for (int i = 0; i < idles.Count; ++i) {
				idles [i].SetActive (false);
			}
			for (int i = 0; i < teleporting.Count; ++i) {
				teleporting [i].SetActive (true);
			}
			for (int i = 0; i < landing.Count; ++i) {
				landing [i].SetActive (false);
			}
		}

		public void OnLanding ()
		{
			for (int i = 0; i < idles.Count; ++i) {
				idles [i].SetActive (false);
			}
			for (int i = 0; i < teleporting.Count; ++i) {
				teleporting [i].SetActive (false);
			}
			for (int i = 0; i < landing.Count; ++i) {
				landing [i].SetActive (true);
			}
		}
	}
}