using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DesignSociety
{
	public class MaterialCollection : MonoBehaviour
	{
		private Dictionary<string, Material> materials = new Dictionary<string, Material> ();
		private Dictionary<string, Material> alphaMaterials = new Dictionary<string, Material> ();
		private NetworkManagerHUD hud;

		#region instance

		private static MaterialCollection instance;

		public static MaterialCollection GetInstance ()
		{
			return instance;
		}

		void Awake ()
		{
			instance = this;
		}

		#endregion

		void Start ()
		{
			hud = FindObjectOfType<NetworkManagerHUD> ();
			hud.showGUI = false;

			LoadClothesData ();
		}

		public Texture2D GetTexture (string name, string path)
		{
			if (name != null && clothesTextures.ContainsKey (name)) {
				return clothesTextures [name];
			} else if (name != null && iconTextures.ContainsKey (name)) {
				return iconTextures [name];
			} else if (name != null && screenTextures.ContainsKey (name)) {
				return screenTextures [name];
			} else {
				Log.error ("【" + name + "】贴图缺失");
				return null;
			}
		}

		#region AI材质

		public Material GetMaterial (string name)
		{
			if (name != null && materials.ContainsKey (name)) {
				return materials [name];
			} else {
				return LoadMaterial (name);
			}
		}

		Material LoadMaterial (string name)
		{
			Texture2D texture = GetTexture (name, Global.TexturePath);
			if (texture != null) {
				Material mat = CreateMaterialWithTexture (texture);
				materials.Add (name, mat);
				return mat;
			}
			return null;
		}

		Material CreateMaterialWithTexture (Texture2D texture)
		{
			Material mat = new Material (Shader.Find ("Standard"));
			mat.SetTexture ("_EmissionMap", texture);
			mat.SetColor ("_EmissionColor", Color.white);
			mat.EnableKeyword ("_EMISSION");
			return mat;
		}


		#endregion

		#region 玩家材质

		public Material GetAlphaMaterial (string name)
		{
			if (name != null && alphaMaterials.ContainsKey (name)) {
				return alphaMaterials [name];
			} else {
				return LoadAlphaMaterial (name);
			}
		}

		Material LoadAlphaMaterial (string name)
		{
			Texture2D texture = GetTexture (name, Global.TexturePath);
			if (texture != null) {
				Material mat = CreateAlphaMaterialWithTexture (texture);
				alphaMaterials.Add (name, mat);
				return mat;
			}
			return null;
		}

		Material CreateAlphaMaterialWithTexture (Texture2D texture)
		{
			Material mat = new Material (Shader.Find ("ApcShader/XRayEffect2"));
			mat.SetTexture ("_MainTex", texture);
			return mat;
		}

		#endregion

		#region 资源预加载

		public Dictionary<string, Texture2D> clothesTextures = new Dictionary<string, Texture2D> ();
		public Dictionary<string, Texture2D> iconTextures = new Dictionary<string, Texture2D> ();
		public Dictionary<string, Texture2D> screenTextures = new Dictionary<string, Texture2D> ();

		public bool loadClothes;
		public bool loadIcon;
		public bool loadScreen = true;

		private string[] paths;
		private string filename;
		private List<string> urlpaths = new List<string> ();
		private List<string> filenames = new List<string> ();

		private int batch = 20;
		private int bunch = -1;
		private int start;
		private int end;

		public void OnClothesLoaded ()
		{
			loadClothes = true;
//			if (loadScreen && loadIcon)
//				hud.showGUI = true;
			LoadIconData ();
		}

		public void OnIconLoaded ()
		{
			loadIcon = true;
			if (loadClothes && loadScreen) {
				//hud.showGUI = true;
				urlpaths.Clear ();
				filenames.Clear ();
				StopAllCoroutines ();
				Resources.UnloadUnusedAssets ();
				AutoLoad.GetInstance ().LoadIpData ();
			}
		}

		public void OnScreenLoaded ()
		{
			loadScreen = true;
			if (loadClothes && loadIcon)
				hud.showGUI = true;
		}

		#region 加载衣物贴图

		public void LoadClothesData ()
		{
			if (clothesTextures.Count != 0)
				return;

			string directory = FineTuneDirectoryPath (Global.TexturePath);
			try {
				paths = System.IO.Directory.GetFiles (directory);
			} catch (Exception e) {
				Log.error (e.Message);
				OnClothesLoaded ();
				return;
			}
			if (paths.Length == 0) {
				OnClothesLoaded ();
			}

			urlpaths.Clear ();
			filenames.Clear ();
			bunch = -1;

			for (int i = 0; i < paths.Length; ++i) {
				if (paths [i].Contains ("meta"))
					continue;
				urlpaths.Add (paths [i]);
				filename = GetFileNameWithoutDot (GetFileName (paths [i]));
				filenames.Add (filename);
				clothesTextures [filename] = null;
			}
			LoadBatchClothes ();
		}

		void LoadBatchClothes ()
		{
			bunch += 1;
			start = bunch * batch;
			if (start >= filenames.Count) {
				OnClothesLoaded ();
				return;
			}

			end = (bunch + 1) * batch - 1;
			if (end >= filenames.Count)
				end = filenames.Count - 1;
			
			for (int i = start; i <= end; ++i) {
				StartCoroutine (LoadClothesDataCoroutine (GetFileURL (urlpaths [i]), filenames [i]));
			}
		}

		IEnumerator LoadClothesDataCoroutine (string url, string filename)
		{
			WWW www = new WWW (url);
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				clothesTextures.Remove (filename);
			} else {
				try {
					Texture2D texture = new Texture2D (2, 2);
					www.LoadImageIntoTexture (texture);
					clothesTextures [filename] = texture;
				} catch (Exception e) {
					Log.error (e.ToString ());
					clothesTextures.Remove (filename);
				}
			}
			DestroyImmediate (www.texture);
			www.Dispose ();
			www = null;

			for (int i = start; i <= end; ++i) {
				string file = filenames [i];
				if (string.IsNullOrEmpty (file) || !clothesTextures.ContainsKey (file))
					continue;
				if (clothesTextures [file] == null)
					yield break;
			}
			LoadBatchClothes ();
		}

		#endregion

		#region 加载icon贴图

		public void LoadIconData ()
		{
			if (iconTextures.Count != 0)
				return;
			
			string directory = FineTuneDirectoryPath (Global.IconPath);
			try {
				paths = System.IO.Directory.GetFiles (directory);
			} catch (Exception e) {
				Log.error (e.Message);
				OnIconLoaded ();
				return;
			}
			if (paths.Length == 0) {
				OnIconLoaded ();
			}

			urlpaths.Clear ();
			filenames.Clear ();
			bunch = -1;

			for (int i = 0; i < paths.Length; ++i) {
				if (paths [i].Contains ("meta"))
					continue;
				urlpaths.Add (paths [i]);
				filename = GetFileNameWithoutDot (GetFileName (paths [i]));
				filenames.Add (filename);
				iconTextures [filename] = null;
			}
			LoadBatchIcons ();
		}

		void LoadBatchIcons ()
		{
			bunch += 1;
			start = bunch * batch;
			if (start >= filenames.Count) {
				OnIconLoaded ();
				return;
			}

			end = (bunch + 1) * batch - 1;
			if (end >= filenames.Count)
				end = filenames.Count - 1;

			for (int i = start; i <= end; ++i) {
				StartCoroutine (LoadIconDataCoroutine (GetFileURL (urlpaths [i]), filenames [i]));
			}
		}

		IEnumerator LoadIconDataCoroutine (string url, string filename)
		{
			WWW www = new WWW (url);
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				iconTextures.Remove (filename);
			} else {
				try {
					Texture2D texture = new Texture2D (2, 2);
					www.LoadImageIntoTexture (texture);
					iconTextures [filename] = texture;
				} catch (Exception e) {
					Log.error (e.ToString ());
					iconTextures.Remove (filename);
				}
			}
			DestroyImmediate (www.texture);
			www.Dispose ();
			www = null;

			for (int i = start; i <= end; ++i) {
				string file = filenames [i];
				if (string.IsNullOrEmpty (file) || !iconTextures.ContainsKey (file))
					continue;
				if (iconTextures [file] == null)
					yield break;
			}
			LoadBatchIcons ();
		}

		#endregion

		#region 加载屏幕贴图

		public void LoadScreenData ()
		{
			if (screenTextures.Count != 0)
				return;
			
			string directory = FineTuneDirectoryPath (Global.DashboardPath);
			try {
				paths = System.IO.Directory.GetFiles (directory);
			} catch (Exception e) {
				Log.error (e.Message);
				OnScreenLoaded ();
				return;
			}
			if (paths.Length == 0)
				OnScreenLoaded ();

			filenames.Clear ();
			for (int i = 0; i < paths.Length; ++i) {
				filename = GetFileNameWithoutDot (GetFileName (paths [i]));
				filenames.Add (filename);
				screenTextures [filename] = null;
			}
			for (int i = 0; i < paths.Length; ++i) {
				StartCoroutine (LoadScreenDataCoroutine (GetFileURL (paths [i]), filenames [i]));
			}
		}

		IEnumerator LoadScreenDataCoroutine (string url, string filename)
		{
			WWW www = new WWW (url);
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				screenTextures.Remove (filename);
			} else {
				try {
					Texture2D texture = new Texture2D (2, 2);
					www.LoadImageIntoTexture (texture);
					screenTextures [filename] = texture;
				} catch (Exception e) {
					Log.error (e.ToString ());
					screenTextures.Remove (filename);
				}
			}
			foreach (string file in screenTextures.Keys) {
				if (screenTextures [file] == null)
					yield break;
			}
			OnScreenLoaded ();
		}

		#endregion

		#endregion

		# region 文件名处理

		string GetFileURL (string path)
		{
			return (new System.Uri (path)).AbsoluteUri;
		}

		string GetFileName (string path)
		{
			int index = path.LastIndexOf (Path.DirectorySeparatorChar);
			return (index < 0) ? path : path.Substring (index + 1, path.Length - index - 1);
		}

		string GetFileNameWithoutDot (string filename)
		{
			int index = filename.LastIndexOf ('.');
			return (index < 0) ? filename : filename.Substring (0, index);
		}

		string FineTuneDirectoryPath (string path)
		{
			if (path.Contains ("file:")) {
				for (int i = 5; i < path.Length; ++i) {
					if (path [i] != Path.DirectorySeparatorChar)
						return Path.DirectorySeparatorChar + path.Substring (i, path.Length - i - 1);
				}
			}
			return path;
		}

		#endregion
	}
}