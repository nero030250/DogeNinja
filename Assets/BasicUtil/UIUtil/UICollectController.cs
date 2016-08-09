using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UICollectController : MonoBehaviour {
	public bool collectUI = true;
	private Dictionary <string, Transform> uiDic = new Dictionary<string, Transform> ();

	protected virtual void Awake () {
		if (collectUI) {
			AddToDic (transform);
		}
	}

	private void AddToDic (Transform transform) {
		for (int index = 0; index < transform.childCount; index++) {
			Transform child = transform.GetChild (index);
			if (uiDic.ContainsKey (child.name)) {
				Debug.LogError (string.Format ("Same Name!!! {0} {1}", gameObject.name,  child.name));
				continue;
			}
			uiDic.Add (child.name, child);
			if (child.GetComponent<UICollectController> () == null)
				AddToDic (child);
		}
	}

	protected Transform GetChildTrans (string uiName) {
		return uiDic [uiName];
	}

	protected GameObject GetCtrl (string uiName) {
		return GetChildTrans (uiName).gameObject;
	}

	protected V GetChildComponent<V> (string uiName) where V : MonoBehaviour {
		return GetChildTrans (uiName).GetComponent<V> ();
	}

	protected UILabel GetLabel (string uiName) {
		return GetChildComponent <UILabel> (uiName);
	}

	protected UIGrid GetGrid (string uiName) {
		return GetChildComponent <UIGrid> (uiName);
	}

	protected UIGridEx GetGridEx (string uiName) {
		return GetChildComponent <UIGridEx> (uiName);
	}

	protected UISprite GetSprite (string uiName) {
		return GetChildComponent <UISprite> (uiName);
	}

	protected UIButton GetButton (string uiName) {
		return GetChildComponent <UIButton> (uiName);
	}

	protected UIInput GetInput (string uiName) {
		return GetChildComponent <UIInput> (uiName);
	}
}