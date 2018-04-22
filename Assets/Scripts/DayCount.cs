using UnityEngine;
using UnityEngine.UI;

public class DayCount : MonoBehaviour {

	public int count = 0;

	Text label;

	void Start () {
		label = GetComponent<Text> ();
		DayController.NextDayCallback += UpdateNewDay;
		label.text = "Day:  " + count;
	}
	
	void UpdateNewDay () {
		count++;
		label.text = "Day:  " + count;
	}
}
