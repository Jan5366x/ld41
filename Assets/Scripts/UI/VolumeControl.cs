using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
	public class VolumeControl: MonoBehaviour {

		public Slider VolumeSlider;
		public AudioMixer VolumeAudio;
		public void VolumeController()
		{

			VolumeAudio.SetFloat("Master", VolumeSlider.value);
		}
		
	
	}
}
