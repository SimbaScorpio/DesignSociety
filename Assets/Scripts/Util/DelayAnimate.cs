using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignSociety
{
	public class DelayAnimate : MonoBehaviour
	{
		public float delay = 2f;
		private Animator anim;

		void Start ()
		{
			anim = GetComponent<Animator> ();
			if (anim == null)
				return;
			anim.enabled = false;
			StartCoroutine (WaitToStart ());
		}

		IEnumerator WaitToStart ()
		{
			float count = 0f;
			while (count < delay) {
				count += Time.deltaTime;
				yield return null;
			}
			if (anim != null)
				anim.enabled = true;
		}
	}
}