using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Alert : MonoBehaviour {

	public float lifetime = 0.5f;
	public float force = 10;
	
	Rigidbody2D body;
	//AudioSource aud;
	float timer = 0;
	bool halt = false;

	void Start () {
		body = GetComponent<Rigidbody2D> ();
		//aud = GetComponent<AudioSource> ();
	}
	
	void Update () {
		if (!halt) {
			body.AddForce (Vector2.up * force);

			if (timer >= 0.2f) {
				body.velocity = Vector2.zero;
				halt = true;

				Destroy (gameObject, lifetime);
			}
		}

		timer += Time.deltaTime;
	}

	public void SetAlert (int value, Card.Suit suit) {
		string info = ((value > 0) ? "+" : "") + value.ToString ();
		if (suit != Card.Suit.Unknown) {
			info += " ";// + suit.ToString ();
		}

		TextMeshPro label = GetComponent<TextMeshPro> ();
		label.text = info;
		Debug.Log (info + " " + suit.ToString ());
	}
}
