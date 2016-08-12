using UnityEngine;
using System.Collections;

public class InputM : UISingleton<InputM> {

	public SystemDelegate.DirectionDelegate OnDirection;
	public SystemDelegate.PosDelegate OnMouseDown;

	private Vector3 lastClickPos;
	public float MIN_MOVE = 30f;

	protected override void Awake () {
		base.Awake ();
		DontDestroyOnLoad (gameObject);
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			lastClickPos = Input.mousePosition;
			if (OnMouseDown != null)
				OnMouseDown (UICamera.currentCamera.ScreenToWorldPoint (lastClickPos));
		} else if (Input.GetMouseButtonUp (0)) {
			MoveDirection dir = MoveDirectionHelper.GetDirection (lastClickPos, Input.mousePosition);
			if (OnDirection != null)
				OnDirection (dir);
		}
	}
}