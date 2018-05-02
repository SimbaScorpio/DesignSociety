using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DesignSociety
{
	public class UIClientKeywordTextEvent : MonoBehaviour
	{
		private TextMeshProUGUI pro;

		void Start ()
		{
			pro = GetComponentInChildren<TextMeshProUGUI> ();
		}

		public void OnPageLeft ()
		{
			if (pro.pageToDisplay <= 1) {
				pro.pageToDisplay = 1;
				return;
			}
			pro.pageToDisplay -= 1;
		}

		public void OnPageRight ()
		{
			print (pro.isTextOverflowing);
			if (pro.isTextOverflowing && pro.pageToDisplay == 2)
				return;
			pro.pageToDisplay += 1;
		}
	}
}