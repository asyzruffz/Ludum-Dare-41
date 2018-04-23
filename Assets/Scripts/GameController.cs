using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public AnimalHandler animalHandler;
	public GameObject gameOverPanel;

	bool gameEnd;

	void Start () {
		
	}
	
	void Update () {
		if (Input.GetButtonDown("Cancel")) {
			Application.Quit ();
		}

		if (animalHandler.livestockList.Count == 0 && !gameEnd) {
			gameEnd = true;
			Invoke ("EndGame", 1f);
		}
	}

	void EndGame () {
		gameOverPanel.SetActive (true);
		Invoke ("BackToMenu", 3f);
	}

	void BackToMenu () {
		SceneManager.LoadScene ("MainMenu");
	}
}
