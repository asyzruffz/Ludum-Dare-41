using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DimSky : MonoBehaviour {

	Image skyLight;

	void Start () {
		skyLight = GetComponent<Image> ();
		DayController.DayNightShiftCallback += SetSkyLight;
	}
	
	void SetSkyLight (bool bright) {
		float val = bright ? 0f : 0.6f;
		StopCoroutine ("DimUntil");
		StartCoroutine (DimUntil (val));
	}

	IEnumerator DimUntil (float value) {
		float currentValue = skyLight.color.a;
		while (Mathf.Abs (currentValue - value) >= 0.001f) {
			yield return new WaitForEndOfFrame ();
			skyLight.color = new Color (skyLight.color.r,
										skyLight.color.g,
										skyLight.color.b,
										Mathf.Lerp (currentValue, value, Time.deltaTime));
			currentValue = skyLight.color.a;
		}
	}
}
