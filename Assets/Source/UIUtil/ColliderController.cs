using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderController : MonoBehaviour {

	public SystemDelegate.ColliderDelegate OnColliderEnterEvent;
	public void OnColliderEnter (Collider e) {
		if (OnColliderEnterEvent != null)
			OnColliderEnterEvent (e);
	}

	public SystemDelegate.ColliderDelegate OnColliderExitEvent;
	public void OnColliderExit (Collider e) {
		if (OnColliderExitEvent != null)
			OnColliderExitEvent (e);	
	}

	public SystemDelegate.ColliderDelegate OnColliderStayEvent;
	public void OnColliderStay (Collider e) {
		if (OnColliderStayEvent != null)
			OnColliderStayEvent (e);	
	}
}