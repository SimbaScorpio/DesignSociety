using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class record : MonoBehaviour
{
	public float time = 5f;
	public float speed = 0.08f;
	public string path;
	public int rate = 30;
	public int index = 1;
	private bool stop = false;
	private string filename;


	void Awake ()
	{
		Application.targetFrameRate = rate;
		Screen.SetResolution (5760, 1200, true);
	}

	void Start ()
	{
		Invoke ("Stop", time);
		Time.timeScale = 0;
		Application.CaptureScreenshot (path + index + ".png");
		index++;
		Time.timeScale = speed;
		InvokeRepeating ("Record", 1f / rate, 1f / rate);
	}

	void Record ()
	{
		if (!stop) {
			Time.timeScale = 0;
			Application.CaptureScreenshot (path + index + ".png");
			index++;
			Time.timeScale = speed;
		}
	}

	void Stop ()
	{
		stop = true;
		Time.timeScale = 0;
		CancelInvoke ("Record");
		Debug.Log ("Finish");
	}
}
