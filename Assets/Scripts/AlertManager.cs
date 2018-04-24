using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertManager : MonoBehaviour {

	public GameObject alertPrefab;
	public float alertDelay = 0.2f;

	Queue<AlertInfo> alertQueue = new Queue<AlertInfo> ();
	float timer = 0;
	
	void Start () {
		
	}
	
	void Update () {
		if (alertQueue.Count > 0) {
			if (timer <= 0) {
				timer = alertDelay;
				AlertInfo info = alertQueue.Dequeue ();
				Play (info);
			}
		}
		
		if (timer > 0) {
			timer -= Time.deltaTime;
		}
	}

	public void DoAlert (Transform animalTransform, int val, Card.Suit cardSuit = Card.Suit.Unknown) {
		AlertInfo newAlert = new AlertInfo (animalTransform, val, cardSuit);
		alertQueue.Enqueue (newAlert);
	}

	void Play (AlertInfo alertInfo) {
		if (alertInfo.trans) {
			Vector3 offset = Vector3.up * 0.5f;
			GameObject alertObject = Instantiate (alertPrefab, alertInfo.trans.position + offset, Quaternion.identity);//, alertInfo.trans);
			Alert alert = alertObject.GetComponent<Alert> ();
			alert.SetAlert (alertInfo.value, alertInfo.suit);
		}
	}
}

public class AlertInfo {
	public Transform trans;
	public int value;
	public Card.Suit suit;

	public AlertInfo (Transform animalTransform, int val, Card.Suit cardSuit) {
		trans = animalTransform;
		value = val;
		suit = cardSuit;
	}
}