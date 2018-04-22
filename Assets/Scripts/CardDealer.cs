using UnityEngine;
using UnityEngine.UI;

public class CardDealer : MonoBehaviour {

	public Transform panel;
	public GameObject cardPrefab;
	public AnimalHandler animalHandler;

	[Header ("Card Resources")]
	public Sprite[] diamondCards;
	public Sprite[] clubCards;
	public Sprite[] heartCards;
	public Sprite[] spadeCards;
	public Sprite backCard;

	AudioController aud;

	void Start () {
		aud = GetComponent<AudioController> ();
	}
	
	public void AddCard (Card.Suit suit, Card.Value value) {
		Card.Info cardInfo = new Card.Info (suit, value);

		GameObject newCard = Instantiate (cardPrefab, panel);
		Card card = newCard.GetComponent<Card> ();
		card.info = cardInfo;
		card.dealer = this;

		int valueIndex = (int)cardInfo.value - 1;
		switch (cardInfo.suit) {
			case Card.Suit.Diamond:
				newCard.GetComponent<Image> ().sprite = diamondCards[valueIndex];
				break;
			case Card.Suit.Club:
				newCard.GetComponent<Image> ().sprite = clubCards[valueIndex];
				break;
			case Card.Suit.Heart:
				newCard.GetComponent<Image> ().sprite = heartCards[valueIndex];
				break;
			case Card.Suit.Spade:
				newCard.GetComponent<Image> ().sprite = spadeCards[valueIndex];
				break;
			default:
				break;
		}
	}

	public void ActivateCard (Card.Info cardInfo) {
		aud.PlaySoundType ("Use");
		Debug.Log ("Activating " + cardInfo.value.ToString () + " of " + cardInfo.suit.ToString ());

		animalHandler.Evaluate (cardInfo);
	}
}
