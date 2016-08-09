using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ResourcesManager : UISingleton <ResourcesManager> {
	public GameObject CreateGameObject (string path) {
		return Resources.Load<GameObject> (path);
	}

	public string[] ReadConfigFile (string fileName) {
		// 外部优先
		string path = string.Format ("{0}/../Config/{1}", Application.dataPath, fileName);
		if (File.Exists (path)) {
			return File.ReadAllLines (path);
		}

		path = string.Format ("Config/{0}", fileName);
		TextAsset textAsset = Resources.Load <TextAsset> (path);
		Debug.LogWarning (textAsset.text);
		string text = textAsset.text;
		text.Replace ("\r\n", "\n");
		string[] split = text.Split ('\n');
		return split;
	}
}