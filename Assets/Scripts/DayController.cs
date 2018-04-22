using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayController : MonoBehaviour {

	public float dayDuration = 5;
	public float nightDuration = 5;
	
	public static UnityAction NextDayCallback;
	public delegate void DayNightShiftDelegate (bool daytime);
	public static event DayNightShiftDelegate DayNightShiftCallback;

	float timer;
	bool isDayTime;

	void Start () {
		timer = -2; // Extra time for initial card drawing
		isDayTime = true;
	}
	
	void Update () {
		if (timer >= (isDayTime ? dayDuration : nightDuration)) {
			timer = 0;
			isDayTime = !isDayTime;
			DayNightShiftCallback (isDayTime);

			if (isDayTime) {
				NextDayCallback ();
			}
		}

		timer += Time.deltaTime;
	}
}
