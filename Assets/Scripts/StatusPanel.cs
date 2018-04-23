using UnityEngine;
using TMPro;

public class StatusPanel : MonoBehaviour {

	[Header ("References")]
	public AnimalHandler handler;
	public GameObject panel;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI threatText;
	public TextMeshProUGUI nutrientText;
	public TextMeshProUGUI expText;

	void Start () {
		
	}
	
	void Update () {
		panel.SetActive (IsStatValid ());

		nameText.text = (IsStatValid () ? handler.animalSelected.species : "???");
		healthText.text = ": " + (IsStatValid () ? handler.animalSelected.status.health.ToString () : "?");
		threatText.text = ": " + (IsStatValid () ? handler.animalSelected.status.threat.ToString () : "?");
		nutrientText.text = ": " + (IsStatValid () ? handler.animalSelected.status.nutrient.ToString () : "?") + "/10";
		expText.text = ": " + (IsStatValid () ? handler.animalSelected.status.experience.ToString () : "?") + "/10";
	}

	bool IsStatValid () {
		return (handler != null) && (handler.animalSelected != null);
	}
}
