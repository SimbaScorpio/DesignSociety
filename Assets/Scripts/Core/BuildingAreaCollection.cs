using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignSociety
{
	public class BuildingAreaCollection : MonoBehaviour
	{
		public List<GameObject> areas = new List<GameObject> ();
		private List<Vector4> vecs = new List<Vector4> ();
		private List<Vector3> centers = new List<Vector3> ();

		float distance = 0f;
		float minDistance = 0f;
		string nearestName = "";

		#region instance

		private static BuildingAreaCollection instance;

		public static BuildingAreaCollection GetInstance ()
		{
			return instance;
		}

		void Awake ()
		{
			instance = this;
		}

		#endregion

		void Start ()
		{
			for (int i = 0; i < areas.Count; ++i) {
				Transform[] corners = areas [i].GetComponentsInChildren<Transform> ();
				Vector3 max = corners [1].position;
				Vector3 min = corners [2].position;
				vecs.Add (new Vector4 (min.x, min.z, max.x, max.z));
				centers.Add (new Vector3 ((min.x + max.x) / 2f, 0, (min.z + max.z) / 2f));
			}
		}

		// return nearest building
		public string GetBuilding (Vector3 pos)
		{
			minDistance = float.MaxValue;
			nearestName = "";
			pos.y = 0;
			for (int i = 0; i < centers.Count; ++i) {
				distance = Vector3.Distance (pos, centers [i]);
				if (distance < minDistance) {
					nearestName = areas [i].name;
					minDistance = distance;
				}
			}
			return nearestName;
		}
	}
}