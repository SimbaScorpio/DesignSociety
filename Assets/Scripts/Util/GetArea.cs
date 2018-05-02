using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GetArea : MonoBehaviour
{

	void Start ()
	{
		InvokeRepeating ("GetAreaIndex", 3, 3);
	}

	void GetAreaIndex ()
	{
		AstarPath.active.FloodFill ();
		GraphNode areaNode = AstarPath.active.GetNearest (transform.position).node;
		int index = (int)areaNode.Area;
		Debug.Log (index);
	}
}
