using UnityEngine;
using UnityEngine.Audio;

public class BgmManager : MonoBehaviour {

	public AudioMixerSnapshot daySong;
	public AudioMixerSnapshot nightSong;
	public AudioMixerSnapshot silence;
    public bool mute;

    bool daytime = true; // cache the day or night time

	void Start () {
		DayController.Instance.DayNightShiftCallback += Shift;
	}
	
    public void ToggleMute (bool off) {
        mute = off;
        if (mute) {
            // mute this
            silence.TransitionTo (0.5f);
        } else {
            // switch to day or night song
            if (daytime) {
                daySong.TransitionTo (0.5f);
            } else {
                nightSong.TransitionTo (0.5f);
            }
        }
    }

	void Shift (bool day) {
        daytime = day;
        if (!mute) {
            if (day) {
                daySong.TransitionTo (1f);
            } else {
                nightSong.TransitionTo (1f);
            }
        }
	}
}
