using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : UISingleton <SoundManager> {
	public void Play (string sound) {
		GetCtrl (sound).GetComponent <AudioSource> ().Play ();
	}

	public void Stop (string sound) {
		GetCtrl (sound).GetComponent <AudioSource> ().Stop ();
	}
}