using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignSociety
{
	public class UIClientBuildingMask : MonoBehaviour
	{
		public GameObject mask;

		#region instance

		private static UIClientBuildingMask instance;

		public static UIClientBuildingMask GetInstance ()
		{
			return instance;
		}

		void Awake ()
		{
			instance = this;
		}

		#endregion

		public void Show ()
		{
			if (mask != null)
				mask.SetActive (true);
		}

		public void Hide ()
		{
			if (mask != null)
				mask.SetActive (false);
		}
	}
}