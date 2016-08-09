using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHelper : MonoBehaviour {
	private static Transform _uiPoint;
	public static Transform UiPoint {
		get {
			if (_uiPoint == null)
				_uiPoint = GameObject.Find ("UI Root/Camera").transform;
			return _uiPoint;
		}
	}

	public static GameObject Create (string path) {
		return _Create (UiPoint.gameObject, path);
	}

	public static GameObject Create (GameObject parent, string path) {
		return _Create (parent, path);
	}

	private static GameObject _Create (GameObject parent, string path) {
		GameObject obj = ResourcesManager.Instance.CreateGameObject (path);
		obj = NGUITools.AddChild (parent, obj);
		return obj;
	}
}