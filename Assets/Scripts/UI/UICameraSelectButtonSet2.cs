using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DesignSociety
{
	public class UICameraSelectButtonSet2 : MonoBehaviour
	{
		public CameraGentlemanMovement gentleman;
		public CameraPerspectiveMovement perspective;

		public CameraGentlemanMovement gentleman2;
		public CameraPerspectiveMovement perspective2;

		private int index;
		private bool displayMode;

		public void OnGentlemanClicked ()
		{
			if (gentleman == null)
				return;
			TurnOnCamera (0);
		}

		public void OnPerspectiveClicked ()
		{
			if (perspective == null)
				return;
			TurnOnCamera (1);
		}

		public void OnGentleman2Clicked ()
		{
			if (gentleman == null)
				return;
			TurnOnCamera (2);
		}

		public void OnPerspective2Clicked ()
		{
			if (perspective == null)
				return;
			TurnOnCamera (3);
		}

		void TurnOffAllCamera ()
		{
			if (gentleman != null) {
				gentleman.gameObject.SetActive (false);
				gentleman.tag = "Untagged";
			}
			if (perspective != null) {
				perspective.gameObject.SetActive (false);
				perspective.tag = "Untagged";
			}
			if (gentleman2 != null) {
				gentleman2.gameObject.SetActive (false);
				gentleman2.tag = "Untagged";
			}
			if (perspective2 != null) {
				perspective2.gameObject.SetActive (false);
				perspective2.tag = "Untagged";
			}
		}

		void TurnOnCamera (int index)
		{
			if (index < 0 || index > 4)
				return;
			TurnOffAllCamera ();
			this.index = index;
			if (index == 0 && gentleman != null) {
				gentleman.gameObject.SetActive (true);
				gentleman.tag = "MainCamera";
			}
			if (index == 1 && perspective != null) {
				perspective.gameObject.SetActive (true);
				perspective.tag = "MainCamera";
			}
			if (index == 2 && gentleman2 != null) {
				gentleman2.gameObject.SetActive (true);
				gentleman2.tag = "MainCamera";
			}
			if (index == 3 && perspective2 != null) {
				perspective2.gameObject.SetActive (true);
				perspective2.tag = "MainCamera";
			}
		}

		public void OnDisplayModeClicked ()
		{
			if (!displayMode) {
				TurnOnCamera (0);
				gentleman.DisplayAspect (true);
				displayMode = true;
			} else {
				gentleman.DisplayAspect (false);
				displayMode = false;
			}
		}

		public void OnResetClicked ()
		{
			if (index == 0 && gentleman != null) {
				gentleman.ResetTransform ();
			}
			if (index == 1 && perspective != null) {
				perspective.ResetTransform ();
			}
			if (index == 2 && gentleman2 != null) {
				gentleman2.ResetTransform ();
			}
			if (index == 3 && perspective2 != null) {
				perspective2.ResetTransform ();
			}
		}
	}
}
