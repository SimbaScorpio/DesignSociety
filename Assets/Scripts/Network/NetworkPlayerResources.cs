using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DesignSociety
{
	public class NetworkPlayerResources : NetworkBehaviour
	{
		[SyncVar (hook = "OnPlayerCharacterChanged")]
		public CharacterData playerCharacterData;

		private CharacterGenerator generator;
		private GameObject ghost;

		#region instance

		private static NetworkPlayerResources instance;

		public static NetworkPlayerResources GetInstance ()
		{
			return instance;
		}

		#endregion

		void Start ()
		{
			generator = CharacterGenerator.GetInstance ();
			if (playerCharacterData != null) {
				generator.Generate (gameObject, playerCharacterData, false);
			}
			if (isLocalPlayer) {
				instance = this;
				playerCharacterData = new CharacterData ();
				playerCharacterData.body_type = "男瘦";
				playerCharacterData.clothing = "1_1";
				playerCharacterData.hair = 1;
				playerCharacterData.bag = 0;
				playerCharacterData.glasses = 0;
				generator.Generate (gameObject, playerCharacterData, true);
				CmdChangeCharacter (playerCharacterData);
				ghost = GetComponent<NetworkPlayerTeleport> ().ghost;
				if (ghost != null)
					generator.Generate (ghost, playerCharacterData, false);
				FileManager.GetInstance ().LoadKeywordData ();
			}
		}

		// 1_1-50 man, 2_1-40 skirt, 3_1-10 short
		public void RandomlyChangeClothes ()
		{
			if (isLocalPlayer) {
				playerCharacterData.clothing = RandomlyGetClothes ();
				generator.Generate (gameObject, playerCharacterData, true);
				if (ghost != null)
					generator.Generate (ghost, playerCharacterData, false);
				CmdChangeCharacter (playerCharacterData);
			}
		}

		string RandomlyGetClothes ()
		{
			string name = "1_1";
			if (playerCharacterData.body_type == "男瘦" || playerCharacterData.body_type == "男胖") {
				int index = Random.Range (0, 50) + 1;
				name = "1_" + index.ToString ();
			} else if (playerCharacterData.body_type == "女裤") {
				int index = Random.Range (0, 40) + 1;
				name = "2_" + index.ToString ();
			} else if (playerCharacterData.body_type == "女裙") {
				int index = Random.Range (0, 10) + 1;
				name = "3_" + index.ToString ();
			}
			return name;
		}

		public void RandomlyChangeBodyType ()
		{
			if (isLocalPlayer) {
				if (playerCharacterData.body_type == "男瘦") {
					playerCharacterData.body_type = "男胖";
				} else if (playerCharacterData.body_type == "男胖") {
					playerCharacterData.body_type = "女裙";
					playerCharacterData.hair = 4;
					playerCharacterData.clothing = RandomlyGetClothes ();
				} else if (playerCharacterData.body_type == "女裙") {
					playerCharacterData.body_type = "女裤";
					playerCharacterData.clothing = RandomlyGetClothes ();
				} else if (playerCharacterData.body_type == "女裤") {
					playerCharacterData.body_type = "男瘦";
					playerCharacterData.hair = 0;
					playerCharacterData.clothing = RandomlyGetClothes ();
				}
				generator.Generate (gameObject, playerCharacterData, true);
				if (ghost != null)
					generator.Generate (ghost, playerCharacterData, false);
				CmdChangeCharacter (playerCharacterData);
			}
		}

		[Command]
		void CmdChangeCharacter (CharacterData cdata)
		{
			OnPlayerCharacterChanged (cdata);
		}

		void OnPlayerCharacterChanged (CharacterData playerCharacterData)
		{
			if (!isLocalPlayer) {
				this.playerCharacterData = playerCharacterData;
				generator.Generate (gameObject, playerCharacterData, false);
			}
		}
	}
}