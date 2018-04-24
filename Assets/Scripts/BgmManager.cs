using UnityEngine;
using UnityEngine.Audio;

public class BgmManager : MonoBehaviour {

	public AudioMixerSnapshot daySong;
	public AudioMixerSnapshot nightSong;

	void Start () {
		DayController.Instance.DayNightShiftCallback += Shift;
	}
	
	void Shift (bool day) {
		if (day) {
			daySong.TransitionTo (1f);
		} else {
			nightSong.TransitionTo (1f);
		}
	}
}
