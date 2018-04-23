using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Animal : MonoBehaviour {

	public string species = "???";
	public bool villain;
	public Status status;
	public GameObject[] evolvable;
	public GameObject offspring;
	public bool isWandering = true;

	[Space]
	public AnimalHandler handler;
	public Spots spots;

	Vector3 position;
	float angle = 0;
	Vector3 destination;
	float wanderCooldown = 0;
	bool isDayTime = true;
	UnityAction arrivedCallback;
	protected List<Transform> wanderAreas;

	// Idling parameters
	protected float overallSpeed = 15f;
	protected float amplitude = 0.05f;
	protected float rollWidth = 0.1f;
	protected float rotateOffset = 0.7f;

	protected virtual void Start () {
		DayController.DayNightShiftCallback += AwareOfDayNightCycle;
		wanderAreas = new List<Transform> ();
		position = transform.position;
		destination = position;
		angle = Random.value;
	}

	protected virtual void Update () {
		if (isWandering) {
			if (wanderCooldown <= 0) {
				Wandering ();
			}

			wanderCooldown -= Time.deltaTime;
		}

		Move ();
	}

	public void SetDestination (Vector3 dest, UnityAction callback = null) {
		destination = dest;
		arrivedCallback = callback;
	}

	public virtual void Breed () {
		if (handler) {
			status.nutrient -= 10;
			GameObject newAnimalGameObject = Instantiate (offspring.gameObject, position, Quaternion.identity, transform.parent);
			Animal offspringAnimal = newAnimalGameObject.GetComponent<Animal> ();
			ModifyGene (offspringAnimal);
			offspringAnimal.handler = handler;

			if (villain) {
				handler.enemyList.Add (offspringAnimal);
			} else {
				handler.livestockList.Add (offspringAnimal);
			}
		} else {
			Debug.Log (gameObject.name + " failed to breed!");
		}
	}

	public void Evolve () {
		if (handler) {
			int evolvedIndex = 0;
			if (evolvable.Length > 0) {
				if (evolvable.Length > 1) {
					evolvedIndex = CheckEvolvingCondition ();
				}
			} else {
				Debug.Log (gameObject.name + " has no further evolution!");
				return;
			}

			GameObject newAnimalGameObject = Instantiate (evolvable[evolvedIndex].gameObject, position, Quaternion.identity, transform.parent);

			Animal newAnimal = newAnimalGameObject.GetComponent<Animal> ();
			newAnimal.position = position;
			newAnimal.angle = angle;
			newAnimal.destination = destination;
			newAnimal.handler = handler;

			status.experience -= 10;
			newAnimal.status.experience = status.experience;
			newAnimal.status.health += status.health;
			newAnimal.status.nutrient += status.nutrient;
			newAnimal.status.threat += status.threat;

			RemoveMe (newAnimal);

			if (newAnimal.status.nutrient >= 10) {
				newAnimal.Breed ();
			}
		} else {
			Debug.Log (gameObject.name + " failed to evolve!");
		}
	}

	int CheckEvolvingCondition () {
		if (isDayTime) {
			return HighestStatusValue ();
		} else {
			return 1;
		}
	}

	public void RemoveMe (Animal toBeReplaced = null) {
		if (handler) {
			int index;
			if (villain) {
				index = handler.enemyList.IndexOf (this);
				if (index != -1) {
					if (toBeReplaced) {
						handler.enemyList[index] = toBeReplaced;
					} else {
						handler.enemyList.RemoveAt (index);
					}
				}
			} else {
				index = handler.livestockList.IndexOf (this);
				if (index != -1) {
					if (toBeReplaced) {
						handler.livestockList[index] = toBeReplaced;
					} else {
						handler.livestockList.RemoveAt (index);
					}
				}
			}
		}
		Destroy (gameObject);
	}

	public void HitAnother (Animal victim) {
		// Reduce hp of victim
		victim.status.health -= status.threat;

		// If the victim survives
		if (victim.status.health > 0) {
			// Counter-attack by victim
			status.health -= victim.status.threat;

			if (status.health <= 0) {
				// Attacker dies
				RemoveMe ();
			}
		} else {
			// Victim dies
			victim.RemoveMe ();
			// Absorb nutrients from victims
			status.nutrient += victim.status.nutrient;
			if (status.nutrient >= 10) {
				Breed ();
			}
			// Attacker gains exp
			status.experience += Mathf.Max (0, victim.status.worth - status.worth);
			if (status.experience >= 10) {
				Evolve ();
			}
		}
	}

	void Move () {
		if (Vector2.Distance (position, destination) > 0.1f) {
			MoveTo (destination);
		} else if (arrivedCallback != null) {
			// Event for arriving at destination
			arrivedCallback ();
			arrivedCallback = null;
		}
	}

	void MoveTo (Vector3 target) {
		position = Vector3.Lerp (position, target, Time.deltaTime);
	}

	void LateUpdate () {
		Idling ();
	}

	void Idling () {
		Vector3 offsetPos = new Vector3 (0, amplitude * Mathf.Sin (angle), 0);
		transform.position = position + offsetPos;
		transform.localRotation = Quaternion.Euler (0, 0, rollWidth * Mathf.Sin (rotateOffset * angle) * Mathf.Rad2Deg);
		angle += overallSpeed * Time.deltaTime;
	}

	void Wandering () {
		if (wanderAreas.Count > 0) {
			SetDestination (wanderAreas[Random.Range (0, wanderAreas.Count)].position);
		} else {
			Debug.Log ("Wander areas for " + gameObject.name + " are not set!");
		}
		wanderCooldown = Random.Range (4f, 7f);
	}

	void AwareOfDayNightCycle (bool daytime) {
		isDayTime = daytime;
	}

	int HighestStatusValue () {
		if ((status.health > status.threat) && 
			(status.health >= status.experience) && 
			(status.health >= status.nutrient)) {
			return 2;
		}
		if ((status.threat > status.health) &&
			(status.threat >= status.experience) &&
			(status.threat >= status.nutrient)) {
			return 3;
		}
		return 0;
	}

	protected virtual void ModifyGene (Animal animal) { }

	void OnMouseOver () {
		handler.animalSelected = this;
	}

	void OnMouseExit () {
		handler.animalSelected = null;
	}

	[System.Serializable]
	public struct Status {
		public int health;
		public int threat;
		public int nutrient;
		public int experience;
		public int worth;
	}
}
