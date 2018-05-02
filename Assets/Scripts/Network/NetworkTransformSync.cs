using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DesignSociety
{
	[NetworkSettings (channel = 0, sendInterval = 0.1f)]
	public class NetworkTransformSync : NetworkBehaviour
	{
		public bool isServerControl;
		public bool sendPosition;
		public bool sendRotation;
		public float lerpPosRate = 4f;
		public float lerpRotRate = 10f;

		public bool lag = true;
		public float lerpPosThres = 0.1f;
		public float lerpRotThres = 5f;

		[SyncVar]
		private Vector3 syncPosition;
		[SyncVar]
		private float syncRotationY;

		private Vector3 rot = Vector3.zero;

		void Start ()
		{
			syncPosition = transform.position;
			syncRotationY = transform.rotation.eulerAngles.y;
		}

		void FixedUpdate ()
		{
			if (isServerControl && isServer) {
				if (sendPosition) {
					if (lag) {
						if (SendPosition ()) {
							syncPosition = transform.position;
						}
					} else {
						syncPosition = transform.position;
					}
				}
				if (sendRotation) {
					if (lag) {
						if (SendRotation ()) {
							syncRotationY = transform.rotation.eulerAngles.y;
						}
					} else {
						syncRotationY = transform.rotation.eulerAngles.y;
					}
				}
			} else if (isLocalPlayer) {
				if (sendPosition) {
					if (lag) {
						if (SendPosition ()) {
							CmdSendPosition (transform.position);
						}
					} else {
						CmdSendPosition (transform.position);
					}
				}
				if (sendRotation) {
					if (lag) {
						if (SendRotation ()) {
							CmdSendRotation (transform.rotation.eulerAngles.y);
						}
					} else {
						CmdSendRotation (transform.rotation.eulerAngles.y);
					}
				}
			} else {
				if (sendPosition)
					transform.position = Vector3.Lerp (transform.position, syncPosition, lerpPosRate * Time.deltaTime);
				if (sendRotation) {
					rot.y = Mathf.LerpAngle (transform.rotation.eulerAngles.y, syncRotationY, lerpRotRate * Time.deltaTime);
					transform.rotation = Quaternion.Euler (rot);
				}
			}
		}

		bool SendPosition ()
		{
			return Vector3.Distance (syncPosition, transform.position) >= lerpPosThres;
		}

		bool SendRotation ()
		{
			return Mathf.Abs (syncRotationY - transform.rotation.eulerAngles.y) >= lerpRotThres;
		}

		[Command]
		void CmdSendPosition (Vector3 position)
		{
			syncPosition = position;
		}

		[Command]
		void CmdSendRotation (float rotation)
		{
			syncRotationY = rotation;
		}
	}
}