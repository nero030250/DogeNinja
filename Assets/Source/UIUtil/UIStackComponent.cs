using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIStackComponent : MonoBehaviour {
	private const string MASK_PATH = "Prefab/Mask";
	public bool closeWhenClick = false;
	public bool backClickEnable = false;

	private GameObject _mask;

	void Start () {
		UIManager.Instance.Push (this);
		if (closeWhenClick) {
			AddMaskCollider ();
			EventDelegate.Add (_mask.GetComponent<UIButton> ().onClick, End);
		} else {
			if (!backClickEnable)
				AddMaskCollider ();
		}
	}

	void OnDestroy () {
		if (UIManager.Instance != null)
			UIManager.Instance.Pop ();
	}

	private void AddMaskCollider () {
		_mask = UIHelper.Create (gameObject, MASK_PATH);
	}

	public void End () {
		Destroy (gameObject);
	}
}