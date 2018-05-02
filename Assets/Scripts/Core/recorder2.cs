using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recorder2 : MonoBehaviour
{
	public float time = 5f;
	public string path;
	public int index = 1;

	void FixedUpdate ()
	{
		Application.CaptureScreenshot (path + index + ".png");
		index++;
	}
}
