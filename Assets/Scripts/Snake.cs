using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Animal {

	bool isHunting = false;
	bool isHidden = false;
	float huntingCooldown = 0;
	float attackTimer = 0;

	protected override void Start () {
		base.Start ();
		species = "Snake";

		wanderAreas.AddRange (spots.fieldAreas);
		HuntTimeToggle ();
		
		DayController.Instance.DayNightShiftCallback += DayTimeToggle;
	}
	
	protected override void Update () {
		if (huntingCooldown <= 0) {
			isHunting = !isHunting;
			HuntTimeToggle ();
			huntingCooldown = Random.Range (4f, 6f);
		}

		if (isHunting) {
			if (attackTimer >= 6f) {
				attackTimer = 0;
				Attack ();
				Attack ();
			}

			attackTimer += Time.deltaTime;
		}

		huntingCooldown -= Time.deltaTime;
		base.Update ();
	}

	void DayTimeToggle (bool daylight) {
		if (daylight) {
			isHidden = false;
		} else {
			isHidden = !isHunting;
		}
		
		HuntTimeToggle ();
	}

	void HuntTimeToggle () {
		if (isHunting) {
			isHidden = false;
		}

		// Swap wandering areas
		wanderAreas.Clear ();
		wanderAreas.AddRange (isHunting ? spots.nearbyAreas : isHidden ? spots.hiddenAreas : spots.fieldAreas);
		// Immediately go to preferred area once time changes
		SetDestination (wanderAreas[Random.Range (0, wanderAreas.Count)].position);
		// Not wandering while not hunting
		isWandering = !isHidden;
	}

	void Attack () {
		if (handler.livestockList.Count > 0) {
			RandomSample sample = new RandomSample (handler.livestockList.Count, true);
			HitAnother (handler.livestockList[sample.Next ()]);
		}
	}
}
