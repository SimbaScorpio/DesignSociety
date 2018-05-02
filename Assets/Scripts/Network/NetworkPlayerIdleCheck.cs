using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace DesignSociety
{
	public class NetworkPlayerIdleCheck : NetworkBehaviour
	{
		public float maxTimeToIdle = 60f;
		public GameObject idleMark;
		public float idleCount = 0f;
		public bool isIdleForALongTime = false;

		private NetworkActionDealer ad;

		private GameObject bodymesh;

		void Start ()
		{
			CheckLocal ();
			ad = GetComponent<NetworkActionDealer> ();
			bodymesh = transform.Find ("mesh").gameObject;
			idleCount = maxTimeToIdle;
		}

		void CheckLocal ()
		{
			if (!isLocalPlayer)
				enabled = false;
		}

		void Update ()
		{
			CheckLocal ();
			if (UIClientDetailPanels.GetInstance ().isActive)
				return;
			if (isIdleForALongTime) {
				if (Input.GetMouseButtonUp (0) && !EventSystem.current.IsPointerOverGameObject ()) {
					idleCount = 0;
					NotIdle ();
				}
			} else {
				if (!ad.IsPlaying () && !Input.anyKeyDown && bodymesh.activeInHierarchy) {
					idleCount += Time.deltaTime;
					if (idleCount > maxTimeToIdle)
						Idle ();
				} else {
					idleCount = 0;
				}
			}
		}

		public void Idle ()
		{
			isIdleForALongTime = true;
			if (idleMark != null) {
				idleMark.SetActive (true);
			}
			UIInformationMenu.GetInstance ().ShowIdlePanel ();
			UIClientTeachingPanel.GetInstance ().OnIdle ();
		}

		void NotIdle ()
		{
			isIdleForALongTime = false;
			if (idleMark != null) {
				idleMark.SetActive (false);
			}
			UIInformationMenu.GetInstance ().ShowActivePanel ();
			UIClientTeachingPanel.GetInstance ().OnMoving ();
		}
	}
}