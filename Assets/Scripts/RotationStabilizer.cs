using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationStabilizer : MonoBehaviour {

	Quaternion parentRotation;
	Vector3 position;
	Quaternion rotation;
	float angle = 0;

	void Start () {
		angle = Random.value;
	}
	
	void Update () {
		parentRotation = transform.parent.rotation;
		transform.localRotation = Quaternion.Inverse (parentRotation);
		position = transform.position;
		rotation = transform.localRotation;
	}

	void LateUpdate () {
		Idling ();
	}

	void Idling () {
		Vector3 offsetPos = new Vector3 (0, 0.05f * Mathf.Sin (angle), 0);
		transform.position = position + offsetPos;
		transform.localRotation = rotation * Quaternion.Euler (0, 0, 0.1f * Mathf.Sin (0.7f * angle) * Mathf.Rad2Deg);
		angle += 15f * Time.deltaTime;
	}

}
