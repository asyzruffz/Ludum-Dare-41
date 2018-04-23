using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : Animal {
	
	protected override void Start () {
		base.Start ();
		species = "Penguin";

		wanderAreas.AddRange (spots.boundaryAreas);
	}
	
	protected override void Update () {

		base.Update ();
	}
}
