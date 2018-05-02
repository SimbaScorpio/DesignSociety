using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DesignSociety
{
	public class UIHomeButton : MonoBehaviour
	{
		NetworkPlayerIdleCheck script;

		void Start ()
		{
			script = FindObjectOfType<NetworkPlayerIdleCheck> ();
		}

		public void OnClicked ()
		{
			if (script != null) {
				script.Idle ();
			}
		}
	}
}