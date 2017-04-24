using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	public Text highscore;

	// Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey ("HighscoreCompleted")) {
			highscore.text = "";
		} else {
			highscore.text = "Highscore: " + PlayerPrefs.GetInt ("HighscoreCompleted") + "%";
		}
		if (PlayerPrefs.HasKey ("PeopleAlive")) {
			highscore.text = "Highscore: " + PlayerPrefs.GetInt ("PeopleAlive") + " people alive";
		}
	}
	
	// Update is called once per frame
	void Update () {
		UnityEngine.Cursor.lockState = CursorLockMode.Confined;
		UnityEngine.Cursor.visible = true;
	}

	public void StartGame() {
		Invoke ("LoadLevel", 0.5f);
	}

	private void LoadLevel() {
		Application.LoadLevel ("main");
	}

	public void ExitGame() {
		Application.Quit ();
	}
}
