using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DesignSociety
{
	public class UIClientDetailPanels : MonoBehaviour
	{
		public bool isActive = false;

		#region instance

		private static UIClientDetailPanels instance;

		public static UIClientDetailPanels GetInstance ()
		{
			return instance;
		}

		void Awake ()
		{
			instance = this;
		}

		#endregion

		public void OnCloseButtonClicked (GameObject obj)
		{
			isActive = false;
			obj.SetActive (false);
		}

		public void OnOpenButtonClicked (GameObject obj)
		{
			isActive = true;
			obj.SetActive (true);
			ScrollRect scroll = obj.GetComponentInChildren<ScrollRect> ();
			if (scroll != null)
				scroll.verticalNormalizedPosition = 1;
		}
	}
}