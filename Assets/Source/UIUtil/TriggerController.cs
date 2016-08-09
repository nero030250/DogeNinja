using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerController : MonoBehaviour {
	
	public SystemDelegate.ColliderDelegate OnTriggerEnterEvent;
	public void OnTriggerEnter (Collider e) {
		if (e == null || e.GetComponent<TriggerController> () == null)
			return;
		if (OnTriggerEnterEvent != null)
			OnTriggerEnterEvent (e);
	}

	public SystemDelegate.ColliderDelegate OnTriggerExitEvent;
	public void OnTriggerExit (Collider e) {
		if (e == null || e.GetComponent<TriggerController> () == null)
			return;
		if (OnTriggerExitEvent != null)
			OnTriggerExitEvent (e);	
	}

	public SystemDelegate.ColliderDelegate OnTriggerStayEvent;
	public void OnTriggerStay (Collider e) {
		if (e == null || e.GetComponent<TriggerController> () == null)
			return;
		if (OnTriggerStayEvent != null)
			OnTriggerStayEvent (e);	
	}
}