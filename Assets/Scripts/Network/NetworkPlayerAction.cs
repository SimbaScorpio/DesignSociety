using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using TMPro;

namespace DesignSociety
{
	// 控制行走、动作和关键词气泡点击
	public class NetworkPlayerAction : NetworkBehaviour
	{
		private NetworkActionDealer ad;
		private NetworkPeronStateQuery query;
		private CameraPerspectiveEditor cameraEditor;
		private UIClientBuildingIntro uibuilding;

		private Ray ray;
		private RaycastHit hit;

		void Start ()
		{
			CheckLocal ();
			ad = GetComponent<NetworkActionDealer> ();
			query = GetComponent<NetworkPeronStateQuery> ();
			cameraEditor = Camera.main.GetComponent<CameraPerspectiveEditor> ();
			if (isLocalPlayer)
				InvokeRepeating ("LocationDisplay", 0f, 1f);
		}

		void CheckLocal ()
		{
			if (!isLocalPlayer)
				enabled = false;
		}

		public void WalkTo (Vector3 position)
		{
			Landmark lm = new Landmark ();
			lm.Set (position);
			ad.ApplyWalkAction (lm, false, null);
		}

		public void ApplyAction (string name)
		{
			ad.ApplyAction (name, null);
		}

		void Update ()
		{
			CheckLocal ();
			PressEvent ();
		}

		void LocationDisplay ()
		{
			string building = BuildingAreaCollection.GetInstance ().GetBuilding (transform.position);
			if (string.IsNullOrEmpty (building))
				return;
			if (uibuilding == null) {
				uibuilding = UIClientBuildingIntro.GetInstance ();
			}
			if (uibuilding != null)
				uibuilding.IntroBuilding (building);
		}

		void PressEvent ()
		{
			if (EventSystem.current == null || EventSystem.current.IsPointerOverGameObject ())
				return;
			if (Input.GetMouseButtonDown (0)) {
				ray = cameraEditor.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, float.MaxValue)) {
					CheckKeywordPressed (hit.collider.gameObject);
					CheckServerAIPressed (hit.collider.gameObject);
				}
			}
		}

		public void CheckKeywordPressed (GameObject collider)
		{
			if (collider.tag == "KeywordBubble") {
				// show keyword in menu
				//print ("check bubble: " + collider.name);
				NetworkBubbleDealer bd = collider.transform.parent.parent.GetComponent<NetworkBubbleDealer> ();
				UIClientMessageKeywordBlob.GetInstance ().PushBlob (bd.lastKeyword);
			}
		}

		public void CheckServerAIPressed (GameObject collider)
		{
			if (collider.tag == "Player") {
				//print ("click serverai: " + collider.name);
				Person person = collider.GetComponent<Person> ();
				if (person != null) {
					//UIClientMessageAvatarBlob.GetInstance ().PushBlob (collider);
					query.AskForPersonState (collider);
				}
			}
		}
	}
}