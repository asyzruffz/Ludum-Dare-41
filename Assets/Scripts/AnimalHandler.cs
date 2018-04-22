using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalHandler : MonoBehaviour {

	public GameObject offspringPrefab;
	public GameObject[] enemyPrefabs;

	public int dayToEnemySpawn = 3;

	[Header ("AnimalPool")]
	public List<Animal> livestockList = new List<Animal> ();
	public List<Animal> enemyList = new List<Animal> ();

	int spawnDayCount;

	void Start () {
		spawnDayCount = dayToEnemySpawn;
		DayController.NextDayCallback += SpawnAnotherEnemy;
	}
	
	public void Evaluate (Card.Info cardInfo) {
		int amount;

		switch (cardInfo.value) {
			case Card.Value.Ace:
			case Card.Value.Two:
			case Card.Value.Three:
			case Card.Value.Four:
			case Card.Value.Five:
			case Card.Value.Six:
			case Card.Value.Seven:
			case Card.Value.Eight:
			case Card.Value.Nine:
			case Card.Value.Ten:
				amount = (int)cardInfo.value;
				CalculateForNumbers (cardInfo.suit, amount);
				break;
			case Card.Value.Jack:
				CalculateForJack (cardInfo.suit);
				break;
			case Card.Value.Queen:
				CalculateForQueen (cardInfo.suit);
				break;
			case Card.Value.King:
				CalculateForKing (cardInfo.suit);
				break;
			default:
				break;
		}
	}

	public void CheckGameCondition () {
		if (livestockList.Count == 0) {
			Debug.Log ("You lose!");
		} else if (enemyList.Count == 0) {
			Debug.Log ("You win!");
		}
	}

	void CalculateForNumbers (Card.Suit suit, int amount) {
		RandomSample sample;
		int randIndex;

		switch (suit) {
			case Card.Suit.Spade:
				sample = new RandomSample (enemyList.Count, true);
				
				for (int i = 0; i < amount; i++) {
					randIndex = sample.Next ();
					enemyList[randIndex].status.threat -= 1;
					enemyList[randIndex].status.threat = Mathf.Max (enemyList[randIndex].status.threat, 0);
				}
				break;
			case Card.Suit.Heart:
				sample = new RandomSample (livestockList.Count, true);

				for (int i = 0; i < amount; i++) {
					livestockList[sample.Next ()].status.health += 1;
				}
				break;
			case Card.Suit.Club:
				sample = new RandomSample (livestockList.Count, true);

				for (int i = 0; i < amount; i++) {
					randIndex = sample.Next ();
					livestockList[randIndex].status.nutrient += 1;
					if (livestockList[randIndex].status.nutrient >= 10) {
						livestockList[randIndex].Breed ();
					}
				}
				break;
			case Card.Suit.Diamond:
				sample = new RandomSample (livestockList.Count, true);

				for (int i = 0; i < amount; i++) {
					randIndex = sample.Next ();
					livestockList[randIndex].status.experience += 1;
					if (livestockList[randIndex].status.experience >= 10) {
						livestockList[randIndex].Evolve ();
					}
				}
				break;
			default:
				break;
		}
	}

	void CalculateForJack (Card.Suit suit) {
		RandomSample sample;
		int randIndex;

		switch (suit) {
			case Card.Suit.Spade:
				sample = new RandomSample (enemyList.Count, true);
				enemyList[sample.Next ()].status.threat = 0;
				break;
			case Card.Suit.Heart:
				sample = new RandomSample (livestockList.Count, true);
				livestockList[sample.Next ()].status.health += 10;
				break;
			case Card.Suit.Club:
				sample = new RandomSample (livestockList.Count, true);
				randIndex = sample.Next ();
				livestockList[randIndex].status.nutrient += 10;
				livestockList[randIndex].Breed ();
				break;
			case Card.Suit.Diamond:
				sample = new RandomSample (livestockList.Count, true);
				randIndex = sample.Next ();
				livestockList[randIndex].status.experience += 10;
				livestockList[randIndex].Evolve ();
				break;
			default:
				break;
		}
	}

	void CalculateForQueen (Card.Suit suit) {
		switch (suit) {
			case Card.Suit.Spade:
				for (int i = 0; i < enemyList.Count; i++) {
					enemyList[i].status.threat = Mathf.Max (1, enemyList[i].status.threat);
				}
				break;
			case Card.Suit.Heart:
				for (int i = 0; i < livestockList.Count; i++) {
					livestockList[i].status.health = Mathf.Min (9, livestockList[i].status.health);
				}
				break;
			case Card.Suit.Club:
				int total = livestockList.Count;
				for (int i = 0; i < total; i++) {
					livestockList[i].status.nutrient = Mathf.Min (9, livestockList[i].status.nutrient);
				}
				break;
			case Card.Suit.Diamond:
				for (int i = 0; i < livestockList.Count; i++) {
					livestockList[i].status.experience = Mathf.Min (9, livestockList[i].status.experience);
				}
				break;
			default:
				break;
		}
	}

	void CalculateForKing (Card.Suit suit) {
		switch (suit) {
			case Card.Suit.Spade:
				for (int i = 0; i < enemyList.Count; i++) {
					enemyList[i].status.threat = 0;
				}
				break;
			case Card.Suit.Heart:
				for (int i = 0; i < livestockList.Count; i++) {
					livestockList[i].status.health += 10;
				}
				break;
			case Card.Suit.Club:
				int total = livestockList.Count;
				for (int i = 0; i < total; i++) {
					livestockList[i].status.nutrient += 10;
					livestockList[i].Breed ();
				}
				break;
			case Card.Suit.Diamond:
				for (int i = 0; i < livestockList.Count; i++) {
					livestockList[i].status.experience += 10;
					livestockList[i].Evolve ();
				}
				break;
			default:
				break;
		}
	}

	void SpawnAnotherEnemy () {
		if (spawnDayCount > 0) {
			spawnDayCount--;
			return;
		} else {
			spawnDayCount = dayToEnemySpawn;
		}

		GameObject newEnemyGameObject = Instantiate (enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], 
										new Vector3(8.86f, 2.77f, 0), Quaternion.identity, transform);
		Animal enemy = newEnemyGameObject.GetComponent<Animal> ();
		enemy.handler = this;
		enemyList.Add (enemy);
	}
}
