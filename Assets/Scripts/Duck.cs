using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : Animal {
	
	protected override void Start () {
		base.Start ();
		species = "Duck";

		wanderAreas.AddRange (spots.coopAreas);
	}
	
	protected override void ModifyGene (Animal animal) {
		Status inheritedStatus = new Status();
		inheritedStatus.experience = animal.status.experience + status.experience;
		inheritedStatus.health = animal.status.health + status.health;
		inheritedStatus.threat = animal.status.threat + status.threat;

		animal.status = inheritedStatus;
	}
}
