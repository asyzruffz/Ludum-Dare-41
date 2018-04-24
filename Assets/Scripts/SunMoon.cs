using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoon : MonoBehaviour {

	public GameObject sun;
	public GameObject moon;

	void Start () {
		DayController.Instance.DayNightShiftCallback += Change;
	}
	
	void Change (bool day) {
		sun.SetActive (day);
		moon.SetActive (!day);
	}
}
