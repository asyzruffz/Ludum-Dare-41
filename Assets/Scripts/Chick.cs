using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : Animal {
	
	protected override void Start () {
		base.Start ();
		species = "Chick";

		wanderAreas.AddRange (spots.coopAreas);
	}
	
	protected override void Update () {

		base.Update ();
	}
}
