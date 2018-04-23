using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationStabilizer : MonoBehaviour {

	Vector3 position;
	float angle = 0;

	void Start () {
		position = transform.localPosition;
		angle = Random.value;
	}
	
	void Update () {
		transform.localRotation = Quaternion.Inverse (transform.parent.rotation);
	}

	void LateUpdate () {
		Idling ();
	}

	void Idling () {
		Vector3 offsetPos = transform.localRotation * (new Vector3 (0, 0.05f * Mathf.Sin (angle), 0));
		transform.localPosition = position + offsetPos;
		angle += 15f * Time.deltaTime;
	}

}
