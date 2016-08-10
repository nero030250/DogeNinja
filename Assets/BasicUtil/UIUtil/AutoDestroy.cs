using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoDestroy : MonoBehaviour{
	public float delay = 2f;
	private float time = 0;
	void Update () {
		time += Time.deltaTime;
		if (time > delay)
			Destroy (gameObject);
	}
}