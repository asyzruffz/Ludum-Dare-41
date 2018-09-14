using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	
	void Update () {
		if (Input.GetButtonDown("Cancel")) {
			Application.Quit ();
		}
	}
	
	public void StartGame () {
        Invoke ("StartDelayed", 0.5f);
	}

    void StartDelayed () {
        SceneManager.LoadScene ("GamePlay");
    }
}
