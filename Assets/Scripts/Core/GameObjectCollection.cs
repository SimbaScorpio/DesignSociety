using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignSociety
{
	public class GameObjectCollection : MonoBehaviour
	{
		Dictionary<string, GameObject[]> tagToGameObject = new Dictionary<string, GameObject[]> ();
		Dictionary<string, GameObject> nameToServerAI = new Dictionary<string, GameObject> ();

		#region instance

		private static GameObjectCollection instance;

		public static GameObjectCollection GetInstance ()
		{
			return instance;
		}

		void Awake ()
		{
			instance = this;
		}

		#endregion

		public GameObject[] Get (string tag)
		{
			if (tag != null && tagToGameObject.ContainsKey (tag))
				return tagToGameObject [tag];
			tagToGameObject [tag] = GameObject.FindGameObjectsWithTag (tag);
			return tagToGameObject [tag];
		}

		public void Clear (string tag)
		{
			tagToGameObject.Remove (tag);
		}

		public GameObject GetServerAI (string name)
		{
			if (name != null && nameToServerAI.ContainsKey (name))
				return nameToServerAI [name];
			nameToServerAI [name] = GameObject.Find (name);
			return nameToServerAI [name];
		}
			
	}
}