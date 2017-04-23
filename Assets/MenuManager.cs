using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
