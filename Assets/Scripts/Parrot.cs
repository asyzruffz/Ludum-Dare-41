using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : Animal {

	float orig_overallSpeed;
	float orig_amplitude;
	float orig_rollWidth;

	protected override void Start () {
		base.Start ();

		orig_overallSpeed = overallSpeed;
		orig_amplitude = amplitude;
		orig_rollWidth = rollWidth;

		wanderAreas.AddRange (spots.boundaryAreas);
		ActiveTimeToggle (true);

		DayController.DayNightShiftCallback += ActiveTimeToggle;
	}
	
	protected override void Update () {

		base.Update ();
	}

	void ActiveTimeToggle (bool active) {
		overallSpeed = active ? orig_overallSpeed * 1.5f : orig_overallSpeed;
		amplitude = active ? orig_amplitude : orig_amplitude * 0.8f;
		rollWidth = active ? orig_rollWidth : orig_rollWidth * 0.9f;

		wanderAreas.Clear ();
		wanderAreas.AddRange (active ? spots.boundaryAreas : spots.coopAreas);
		SetDestination (wanderAreas[Random.Range (0, wanderAreas.Count)].position);
	}
}
