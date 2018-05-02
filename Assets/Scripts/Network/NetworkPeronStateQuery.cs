using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DesignSociety
{
	public class NetworkPeronStateQuery : NetworkBehaviour
	{
		public GameObject waitingPerson;
		private UIClientMessageAvatarBlob uiblob;

		#region instance

		private static NetworkPeronStateQuery instance;

		public static NetworkPeronStateQuery GetInstance ()
		{
			return instance;
		}

		#endregion

		void Start ()
		{
			if (isLocalPlayer) {
				instance = this;
			}
		}

		public void AskForPersonState (GameObject serverai)
		{
			if (isLocalPlayer && serverai != null) {
				waitingPerson = serverai;
				CmdQueryState (serverai.name);
			}
		}

		[ClientRpc]
		void RpcQueryState (string name, string state)
		{
			if (isLocalPlayer && waitingPerson != null) {
				if (name == waitingPerson.name) {
					if (uiblob != null) {
						uiblob.PushBlob (waitingPerson, state);
					} else {
						uiblob = UIClientMessageAvatarBlob.GetInstance ();
						if (uiblob != null) {
							uiblob.PushBlob (waitingPerson, state);
						}
					}
					waitingPerson = null;
				}
			}
		}

		[Command] 
		void CmdQueryState (string name)
		{
			GameObject serverai = GameObjectCollection.GetInstance ().GetServerAI (name);
			Person person = serverai.GetComponent<Person> ();
			if (!person.isActiveAndEnabled)
				return;
			string state = "";
			PrincipalActivity principalActivity;
			FollowingActivity followingActivity;

			// principal
			if (person.isPrincipal) {
				principalActivity = person.currentPrincipalActivity;
				if (principalActivity != null)
					state = principalActivity.description;
			} else if (person.parent != null) {
				// controlled by principal or second parent
				if (person.isBeingControlled) {
					if (person.parent.parent == null) {
						principalActivity = person.parent.currentPrincipalActivity;
						if (principalActivity != null)
							state = principalActivity.description;
					} else {
						followingActivity = person.parent.currentFollowingActivity;
						if (followingActivity != null)
							state = followingActivity.description;
					}
				} 
				// act following activity
				else {
					followingActivity = person.parent.currentFollowingActivity;
					if (followingActivity != null)
						state = followingActivity.description;
				}
			} else if (person.isRandomPerson) {
				state = "";
			} else {
				state = "";
			}
			RpcQueryState (name, state);
		}
	}
}