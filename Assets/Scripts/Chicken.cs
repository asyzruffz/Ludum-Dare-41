using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Animal {
	
	protected override void Start () {
		base.Start ();

		wanderAreas.AddRange (spots.coopAreas);
		wanderAreas.AddRange (spots.boundaryAreas);
	}
	
	protected override void Update () {

		base.Update ();
	}
}
