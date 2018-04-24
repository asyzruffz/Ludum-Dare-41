using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Alert : MonoBehaviour {

	public float lifetime = 0.5f;
	public float force = 10;
	public GameObject[] suits;

	TextMeshPro label;
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
		label = GetComponent<TextMeshPro> ();
		label.text = info;
		SetSuit (suit);
	}

	void SetSuit (Card.Suit suit) {
		if (suit != Card.Suit.Unknown) {
			for (int i = 1; i < 5; i++) {
				suits[i - 1].SetActive (i == (int)suit);
			}

			switch (suit) {
				default:
					label.color = Color.black;
					break;
				case Card.Suit.Heart:
					label.color = Color.red;
					break;
				case Card.Suit.Spade:
					label.color = Color.blue;
					break;
				case Card.Suit.Club:
					label.color = Color.green;
					break;
				case Card.Suit.Diamond:
					label.color = new Color32 (255, 176, 0, 255);
					break;
			}
		}
	}
}
