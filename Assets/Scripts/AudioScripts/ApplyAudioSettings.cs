using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyAudioSettings : MonoBehaviour {
	void Start () {
		AudioListener.volume = PlayerPrefs.GetFloat("MasterVolumeLevel", 1.0F);
	}
}
