using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	float angle = 0;

	void Start () {
		
	}
	
	void Update () {
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.back);
		angle += 30 * Time.deltaTime;
	}
}
