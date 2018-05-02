﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace DesignSociety
{
	public class NetworkActionWalk : MonoBehaviour
	{
		public bool isPlaying;
		public Landmark landmark;
		private IActionCompleted callback;

		private bool shouldTurn = true;
		private bool finalRotate = false;
		private float animationSpeed = 1.5f;

		public MyRichAI ai;
		private Animator anim;
		private NetworkActionDealer dealer;
		private NavmeshCut navcut;
		private bool navcutactive = false;

		private float lastH, speedV = 0f, speedH = 0f;
		private float initAISpeed;

		private readonly int hashSpeedParaH = Animator.StringToHash ("SpeedH");
		private readonly int hashSpeedParaV = Animator.StringToHash ("SpeedV");

		void Start ()
		{
			ai = GetComponent<MyRichAI> ();
			anim = GetComponent<Animator> ();
			dealer = GetComponent<NetworkActionDealer> ();
			navcut = GetComponent<NavmeshCut> ();
			if (navcut != null)
				navcutactive = navcut.enabled;
			initAISpeed = ai.maxSpeed;
			ai.OnTargetReached += OnTargetReached;
		}

		public void Setting (string stateName, Landmark landmark, bool turn, IActionCompleted callback)
		{
			this.landmark = landmark;
			this.callback = callback;
			this.shouldTurn = turn;
			lastH = transform.position.y;
			finalRotate = false;

			Navcut (false);
			ai.target = landmark;	// make sure target set before enabled
			ai.enabled = true;
			if (!ai.repeatedlySearchPaths)
				ai.UpdatePath ();

			if (!isPlaying && !string.IsNullOrEmpty (stateName)) {
				anim.Play (stateName, 0, 0f);
			}
			isPlaying = true;
		}

		void OnTargetReached (object sender, EventArgs e)
		{
			finalRotate = true;
		}

		void Update ()
		{
			if (!ai.enabled)
				return;
			speedH = ai.Velocity.magnitude;
			speedV = Mathf.Lerp (speedV, (transform.position.y - lastH) / Time.deltaTime, Time.deltaTime * 5);
			anim.SetFloat (hashSpeedParaH, speedH);
			anim.SetFloat (hashSpeedParaV, speedV);
			anim.speed = speedH * animationSpeed;
			dealer.SyncActionSpeed (anim.speed);

			lastH = transform.position.y;

			DetectFrontPerson ();
			if (finalRotate && (!shouldTurn || FinalRotate ()))
				RealFinish ();
		}

		bool FinalRotate ()
		{
			Vector3 tan = landmark.rotation.eulerAngles;
			Vector3 ran = transform.rotation.eulerAngles;
			float deltaTime = Mathf.Min (Time.smoothDeltaTime * 2, Time.deltaTime);
			ran.y = Mathf.MoveTowardsAngle ((ran.y + 360f) % 360f, (tan.y + 360f) % 360f, ai.rotationSpeed * deltaTime);
			transform.rotation = Quaternion.Euler (ran);
			return Mathf.Abs (ran.y - tan.y) < 0.1f;
		}

		public void Finish ()
		{
			if (ai.TraversingSpecial) {
				StopAllCoroutines ();
				StartCoroutine (WaitForSpecialTraverse ());
			} else {
				RealFinish ();
			}
		}

		void RealFinish ()
		{
			ai.enabled = false;
			ai.maxSpeed = initAISpeed;
			Navcut (true);

			anim.speed = 1;
			dealer.SyncActionSpeed (anim.speed);
			anim.SetFloat (hashSpeedParaH, 0);
			anim.SetFloat (hashSpeedParaV, 0);

			isPlaying = false;
			if (callback != null) {
				callback.OnActionCompleted (null);
			}
		}

		public void RealStop ()
		{
			ai.Stopstop ();
			ai.enabled = false;
			ai.maxSpeed = initAISpeed;
			Navcut (true);

			anim.speed = 1;
			dealer.SyncActionSpeed (anim.speed);
			anim.SetFloat (hashSpeedParaH, 0);
			anim.SetFloat (hashSpeedParaV, 0);

			isPlaying = false;
			if (callback != null) {
				callback.OnActionCompleted (null);
			}
		}

		IEnumerator WaitForSpecialTraverse ()
		{
			while (ai.TraversingSpecial)
				yield return null;
			RealFinish ();
		}

		void Navcut (bool flag)
		{
			if (navcut != null && navcutactive) {
				if (flag != navcut.enabled) {
					navcut.enabled = flag;
					navcut.ForceUpdate ();
				}
			}
		}

		void DetectFrontPerson ()
		{
			float d = 1.5f;
			Vector3 start = transform.position;
			start -= transform.TransformDirection (Vector3.forward) * 0.25f;
			start.y += 1f;

			Ray ray1 = new Ray (start, transform.TransformDirection (Vector3.forward));
			Debug.DrawLine (ray1.origin, ray1.origin + ray1.direction * d, Color.blue);
			Ray ray2 = new Ray (start, transform.TransformDirection ((Vector3.forward + Vector3.right / 2)));
			Debug.DrawLine (ray2.origin, ray2.origin + ray2.direction * d, Color.blue);
			Ray ray3 = new Ray (start, transform.TransformDirection ((Vector3.forward + Vector3.left / 2)));
			Debug.DrawLine (ray3.origin, ray3.origin + ray3.direction * d, Color.blue);

			RaycastHit hit;
			bool flag = false;
			if (!flag && Physics.Raycast (ray1, out hit, d)) {
				if (hit.collider.tag == "Player") {
					flag = DealWithDetect (hit.collider);
				}
			} else if (!flag && Physics.Raycast (ray2, out hit, d)) {
				if (hit.collider.tag == "Player") {
					flag = DealWithDetect (hit.collider);
				}
			} else if (!flag && Physics.Raycast (ray3, out hit, d)) {
				if (hit.collider.tag == "Player") {
					flag = DealWithDetect (hit.collider);
				}
			}
			if (!flag) {
				ai.maxSpeed = initAISpeed;
			}
		}

		bool DealWithDetect (Collider collider)
		{
			NetworkActionWalk sw = collider.GetComponent<NetworkActionWalk> ();
			if (sw != null) {
				Vector3 v13 = this.ai.Velocity;
				Vector2 v12 = new Vector2 (v13.x, v13.z);
				Vector3 v23 = sw.ai.Velocity;
				Vector2 v22 = new Vector2 (v23.x, v23.z);
				if (Vector2.Dot (v12, v22) > 0) {
					ai.maxSpeed = sw.initAISpeed - 0.5f;
					ai.maxSpeed = ai.maxSpeed <= 0 ? 0.5f : ai.maxSpeed;
				} 
				return true;
			}
			return false;
		}
	}
}