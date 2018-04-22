using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
	public class VolumeControl: MonoBehaviour {

		public Slider VolumeSlider;

		public void UpdateVolume()
		{
			AudioListener.volume = VolumeSlider.value;
			PlayerPrefs.SetFloat("MasterVolumeLevel", VolumeSlider.value);
		}
		
	
	}
}
