using UnityEngine;
using TMPro;

public class DayCount : MonoBehaviour {

	public int count = 0;

	TextMeshProUGUI label;

	void Start () {
		label = GetComponent<TextMeshProUGUI> ();
		DayController.Instance.NextDayCallback += UpdateNewDay;
		label.text = "Day:  " + count;
	}
	
	void UpdateNewDay () {
		count++;
		label.text = "Day:  " + count;
	}
}
