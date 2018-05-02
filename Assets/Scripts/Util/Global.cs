using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignSociety
{
	public static class Global
	{
		public static char c = Path.DirectorySeparatorChar;
		public static string StorylineJsonURL = Application.dataPath + c + "Data" + c + "storyline" + c;
		public static string LandmarkJsonURL = Application.dataPath + c + "Data" + c + "location" + c;
		public static string KeywordJsonURL = "file://" + Application.dataPath + c + "Data" + c + "keyword" + c + "keyword.json";
		public static string TexturePath = Application.dataPath + c + "Data" + c + "texture" + c;
		public static string IconPath = Application.dataPath + c + "Data" + c + "icon" + c;
		public static string DashboardPath = Application.dataPath + c + "Data" + c + "screen" + c;
		public static string IpConfigJsonURL = "file://" + Application.dataPath + c + "Data" + c + "ipconfig" + c + "ipconfig.json";

		public static float iconDuration = 4f;
		public static float keywordDuration = 6f;
		public static float chatDuration = 3f;
	}

	public enum SceneState
	{
		READY,
		STARTED,
		ENDED,
		KILLED
	}

	public enum ComState
	{
		ARRIVING,
		ARRIVINGSTOP,
		PREPARING,
		PREPARINGSTOP,
		STARTING,
		STARTINGSTOP,
		ENDING,
		ENDINGSTOP,
		LEAVING
	}

	public enum StuffType
	{
		SmallStuff,
		MiddleStuff,
		BigStuff,
		BookStuff,
		PaperStuff,
		LargepaperStuff,
		StickStuff
	}

	public enum BodyType
	{
		男瘦,
		男胖,
		女裙,
		女裤
	}
}