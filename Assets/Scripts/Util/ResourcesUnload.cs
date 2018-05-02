using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesUnload : MonoBehaviour
{

	void Start ()
	{
		InvokeRepeating ("Unload", 60, 60);
	}

	void Unload ()
	{
		Resources.UnloadUnusedAssets ();
	}

}
