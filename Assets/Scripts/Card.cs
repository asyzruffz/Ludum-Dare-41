using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

	public Info info;
	public CardDealer dealer;
	
	public void Use () {
		if (dealer) {
			dealer.ActivateCard (info);
		} else {
			Debug.Log ("Dealer for the card is null!");
		}

		Destroy (gameObject);
	}

	public enum Suit {
		Unknown,
		Diamond,
		Club,
		Heart,
		Spade
	}
	
	public enum Value {
		Unknown,
		Ace,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Jack,
		Queen,
		King,
		Joker
	}

	[System.Serializable]
	public struct Info {
		public Suit suit;
		public Value value;

		public Info (Suit sut, Value val) {
			suit = sut;
			value = val;
		}
	}
}
