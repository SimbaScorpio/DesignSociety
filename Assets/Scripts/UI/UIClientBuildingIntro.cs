using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DesignSociety
{
	public class UIClientBuildingIntro : MonoBehaviour
	{
		public Text title;
		//public Text title_en;
		public Text description;

		#region instance

		private static UIClientBuildingIntro instance;

		public static UIClientBuildingIntro GetInstance ()
		{
			return instance;
		}

		void Awake ()
		{
			instance = this;
		}

		#endregion

		public void IntroBuilding (string name)
		{
			if (title != null && !string.IsNullOrEmpty (name))
				title.text = name;
			Keyword keyword = KeywordCollection.GetInstance ().Get (name);
			if (keyword != null) {
				//title_en.text = keyword.keyword_en;
				description.text = keyword.description;
			}
		}
	}
}