using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour
{
	// Use this for initialization
	void Awake ()
	{
		Camera camera = GetComponent<Camera> ();
		camera.aspect = 33f / 8f;
	}

}
