using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

	public int initialDraw = 5;
	[Header("Reference")]
	public CardDealer dealer;

	RandomSample deckPopulation;
	AudioController aud;
	Button btn;

	void Start () {
		aud = GetComponent<AudioController> ();
		btn = GetComponent<Button> ();
		DayController.NextDayCallback += TakeFromDeck;

		deckPopulation = new RandomSample (52);

		StartCoroutine (StartDrawingCard ());
	}
	
	IEnumerator StartDrawingCard () {
		btn.interactable = false;
		for (int i = 0; i < initialDraw; i++) {
			TakeFromDeck ();
			yield return new WaitForSeconds (0.4f);
		}
		btn.interactable = true;
	}

	public void TakeFromDeck () {
		if (deckPopulation.IsEmpty()) {
			Debug.Log ("Deck is empty.");
			return;
		}

		int sampleCard = deckPopulation.Next ();

		Card.Value cardValue = (Card.Value)((sampleCard % 13) + 1);
		Card.Suit cardSuit = (Card.Suit)((sampleCard / 13) + 1);

		if (dealer) {
			dealer.AddCard (cardSuit, cardValue);
			aud.PlaySoundType ("Draw");
		} else {
			Debug.Log ("Dealer for the deck is null!");
		}
	}
}
