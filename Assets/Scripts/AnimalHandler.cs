using System.Collections.Generic;
using UnityEngine;

public class AnimalHandler : MonoBehaviour {

	public GameObject[] enemyPrefabs;

	public int dayToEnemySpawn = 3;
	public Animal animalSelected;
	public AlertManager alertManager;

	[Header ("AnimalPool")]
	public List<Animal> livestockList = new List<Animal> ();
	public List<Animal> enemyList = new List<Animal> ();

	[Header ("Sounds")]
	public AudioClip breedSfx;
	public AudioClip evolveSfx;
	public AudioClip attackSfx;

	AudioSource source;
	int spawnDayCount;

	void Start () {
		source = GetComponent<AudioSource> ();

		spawnDayCount = dayToEnemySpawn;
		DayController.Instance.NextDayCallback += SpawnAnotherEnemy;
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

	void CalculateForNumbers (Card.Suit suit, int amount) {
		RandomSample sample;
		int randIndex;
		int[] prev;

		switch (suit) {
			case Card.Suit.Spade:
				if(enemyList.Count == 0) { return; }
				sample = new RandomSample (enemyList.Count, true);

				prev = new int[enemyList.Count];
				for (int i = 0; i < prev.Length; i++) {
					prev[i] = enemyList[i].status.threat;
				}

				for (int i = 0; i < amount; i++) {
					randIndex = sample.Next ();
					enemyList[randIndex].status.threat -= 1;
					enemyList[randIndex].status.threat = Mathf.Max (enemyList[randIndex].status.threat, 0);
				}

				for (int i = 0; i < prev.Length; i++) {
					prev[i] = enemyList[i].status.threat - prev[i];
					if (prev[i] != 0) {
						alertManager.DoAlert (enemyList[i].transform, prev[i], suit);
					}
				}

				break;
			case Card.Suit.Heart:
				if (livestockList.Count == 0) { return; }
				sample = new RandomSample (livestockList.Count, true);

				prev = new int[livestockList.Count];
				for (int i = 0; i < prev.Length; i++) {
					prev[i] = livestockList[i].status.health;
				}

				for (int i = 0; i < amount; i++) {
					randIndex = sample.Next ();
					livestockList[randIndex].status.health += 1;
				}

				for (int i = 0; i < prev.Length; i++) {
					prev[i] = livestockList[i].status.health - prev[i];
					if (prev[i] != 0) {
						alertManager.DoAlert (livestockList[i].transform, prev[i], suit);
					}
				}

				break;
			case Card.Suit.Club:
				if (livestockList.Count == 0) { return; }
				sample = new RandomSample (livestockList.Count, true);

				prev = new int[livestockList.Count];
				for (int i = 0; i < prev.Length; i++) {
					prev[i] = livestockList[i].status.nutrient;
				}

				for (int i = 0; i < amount; i++) {
					randIndex = sample.Next ();
					livestockList[randIndex].status.nutrient += 1;
					if (livestockList[randIndex].status.nutrient >= 10) {
						livestockList[randIndex].Breed ();
						prev[randIndex] -= 10;
					}
				}

				for (int i = 0; i < prev.Length; i++) {
					prev[i] = livestockList[i].status.nutrient - prev[i];
					if (prev[i] != 0) {
						alertManager.DoAlert (livestockList[i].transform, prev[i], suit);
					}
				}

				break;
			case Card.Suit.Diamond:
				if (livestockList.Count == 0) { return; }
				sample = new RandomSample (livestockList.Count, true);

				prev = new int[livestockList.Count];
				for (int i = 0; i < prev.Length; i++) {
					prev[i] = livestockList[i].status.experience;
				}

				for (int i = 0; i < amount; i++) {
					randIndex = sample.Next ();
					livestockList[randIndex].status.experience += 1;
					if (livestockList[randIndex].status.experience >= 10) {
						livestockList[randIndex].Evolve ();
						prev[randIndex] -= 10;
					}
				}

				for (int i = 0; i < prev.Length; i++) {
					prev[i] = livestockList[i].status.experience - prev[i];
					if (prev[i] != 0) {
						alertManager.DoAlert (livestockList[i].transform, prev[i], suit);
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
				randIndex = sample.Next ();
				enemyList[randIndex].status.threat = 0;
				alertManager.DoAlert (enemyList[randIndex].transform, -10, suit);
				break;
			case Card.Suit.Heart:
				sample = new RandomSample (livestockList.Count, true);
				randIndex = sample.Next ();
				livestockList[randIndex].status.health += 10;
				alertManager.DoAlert (livestockList[randIndex].transform, 10, suit);
				break;
			case Card.Suit.Club:
				sample = new RandomSample (livestockList.Count, true);
				randIndex = sample.Next ();
				livestockList[randIndex].status.nutrient += 10;
				alertManager.DoAlert (livestockList[randIndex].transform, 10, suit);
				livestockList[randIndex].Breed ();
				break;
			case Card.Suit.Diamond:
				sample = new RandomSample (livestockList.Count, true);
				randIndex = sample.Next ();
				livestockList[randIndex].status.experience += 10;
				livestockList[randIndex].Evolve ();
				alertManager.DoAlert (livestockList[randIndex].transform, 10, suit);
				break;
			default:
				break;
		}
	}

	void CalculateForQueen (Card.Suit suit) {
		int prev;

		switch (suit) {
			case Card.Suit.Spade:
				for (int i = 0; i < enemyList.Count; i++) {
					prev = enemyList[i].status.threat;
					enemyList[i].status.threat = Mathf.Min (1, enemyList[i].status.threat);
					alertManager.DoAlert (enemyList[i].transform, enemyList[i].status.threat - prev, suit);
				}
				break;
			case Card.Suit.Heart:
				for (int i = 0; i < livestockList.Count; i++) {
					prev = livestockList[i].status.health;
					livestockList[i].status.health = Mathf.Max (9, livestockList[i].status.health);
					alertManager.DoAlert (livestockList[i].transform, livestockList[i].status.health - prev, suit);
				}
				break;
			case Card.Suit.Club:
				int total = livestockList.Count;
				for (int i = 0; i < total; i++) {
					prev = livestockList[i].status.nutrient;
					livestockList[i].status.nutrient = Mathf.Max (9, livestockList[i].status.nutrient);
					alertManager.DoAlert (livestockList[i].transform, livestockList[i].status.nutrient - prev, suit);
				}
				break;
			case Card.Suit.Diamond:
				for (int i = 0; i < livestockList.Count; i++) {
					prev = livestockList[i].status.experience;
					livestockList[i].status.experience = Mathf.Max (9, livestockList[i].status.experience);
					alertManager.DoAlert (livestockList[i].transform, livestockList[i].status.experience - prev, suit);
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
					alertManager.DoAlert (enemyList[i].transform, -10, suit);
				}
				break;
			case Card.Suit.Heart:
				for (int i = 0; i < livestockList.Count; i++) {
					livestockList[i].status.health += 10;
					alertManager.DoAlert (livestockList[i].transform, 10, suit);
				}
				break;
			case Card.Suit.Club:
				int total = livestockList.Count;
				for (int i = 0; i < total; i++) {
					livestockList[i].status.nutrient += 10;
					alertManager.DoAlert (livestockList[i].transform, 10, suit);
					livestockList[i].Breed ();
				}
				break;
			case Card.Suit.Diamond:
				for (int i = 0; i < livestockList.Count; i++) {
					livestockList[i].status.experience += 10;
					livestockList[i].Evolve ();
					alertManager.DoAlert (livestockList[i].transform, 10, suit);
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
			dayToEnemySpawn--;
			spawnDayCount = dayToEnemySpawn;
		}

		int spawnCount = (Mathf.Abs (dayToEnemySpawn) / 2) + 1;
		for (int i = 0; i < spawnCount; i++) {
			GameObject newEnemyGameObject = Instantiate (enemyPrefabs[Random.Range (0, enemyPrefabs.Length)],
											new Vector3 (8.86f, 2.77f, 0), Quaternion.identity, transform);
			Animal enemy = newEnemyGameObject.GetComponent<Animal> ();
			enemy.handler = this;
			enemyList.Add (enemy);
		}
	}

	public void PlayBreedSound () {
		if (source) {
			if (breedSfx) {
				source.PlayOneShot (breedSfx);
			}
		}
	}

	public void PlayEvolveSound () {
		if (source) {
			if (evolveSfx) {
				source.PlayOneShot (evolveSfx);
			}
		}
	}

	public void PlayAttackSound () {
		if (source) {
			if (attackSfx) {
				source.PlayOneShot (attackSfx);
			}
		}
	}
}
