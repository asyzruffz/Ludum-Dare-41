using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : Animal {

	float orig_overallSpeed;
	float orig_amplitude;
	float orig_rollWidth;

	protected override void Start () {
		base.Start ();
		species = "Owl";

		orig_overallSpeed = overallSpeed;
		orig_amplitude = amplitude;
		orig_rollWidth = rollWidth;

		wanderAreas.AddRange (spots.coopAreas);
		SleepTimeToggle (true);

		DayController.DayNightShiftCallback += SleepTimeToggle;
	}
	
	void SleepTimeToggle (bool asleep) {
		// Change speed based on time of day
		overallSpeed = asleep ? orig_overallSpeed * 0.5f : orig_overallSpeed;
		amplitude = asleep ? orig_amplitude * 0.5f : orig_amplitude;
		rollWidth = asleep ? orig_rollWidth * 0.5f : orig_rollWidth;
		// No wandering in daytime
		isWandering = !asleep;

		// Double the attack at night and double health in daytime
		status.threat = asleep ? (status.threat + 1) / 2 : status.threat * 2;
		status.health = asleep ? status.health * 2 : (status.health + 1) / 2;

		// Swap wandering areas
		wanderAreas.Clear ();
		wanderAreas.AddRange (asleep ? spots.coopAreas : spots.boundaryAreas);
		// Immediately go to preferred area once time changes
		SetDestination (wanderAreas[Random.Range (0, wanderAreas.Count)].position);
	}
}
