using UnityEngine;
using System.Collections;

public class UISingleton<T> : UICollectController where T :Component {
	public static T Instance { get; protected set; }

	protected override void Awake () {
		base.Awake ();
		if (Instance != null) {
			Debug.LogError (string.Format ("{0} Initialize Twice !!! In {1}", typeof (T), gameObject.name), gameObject);
		}
		Instance = GetComponent<T> ();
		DebugUtil.Log (string.Format ("Create Instance {0} In {1}", typeof(T), gameObject.name), gameObject);
	}

	protected virtual void OnDestroy () {
		Instance = null;
		DebugUtil.Log (string.Format ("Destroy Instance {0} In {1}", typeof(T), gameObject.name), gameObject);
	}
}