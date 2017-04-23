﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Camera cameraObject;

	public GameObject[] players;

	public int currentCameraPosition;

	public int foodUnits;
	public static readonly int maxFoodUnits = 15;

	public int waterUnits;
	public static readonly int maxWaterUnits = 10;

	public static bool isFishing;
	public static bool isRelaxing;
	public static bool isRowingLeft;
	public static bool isRowingRight;

	public Text foodText;
	public Text waterText;

	public enum GameState {
		MainMenu,
		Playing,
		GameOver
	}
		
	public GameState gameState;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		AddFood ();
		updateFoodAndWater ();
		if (Input.GetKeyUp (KeyCode.Alpha1)) {
			SwitchToPosition (0);
		} else if (Input.GetKeyUp (KeyCode.Alpha2)) {
			SwitchToPosition (1);
		} else if (Input.GetKeyUp (KeyCode.Alpha3)) {
			SwitchToPosition (2);
		} else if (Input.GetKeyUp (KeyCode.Alpha4)) {
			SwitchToPosition (3);
		}
	}

	void AddFood () {
		if (!isFishing) {
			return;
		}
		StartCoroutine ("addOneFoodUnit");
	}

	IEnumerator addOneFoodUnit() {
		foodUnits = Mathf.Max (maxFoodUnits, foodUnits + 1);
		yield return new WaitForSeconds (5.0f);
	}

	void SwitchToPosition (int position) {
		var lastDudeScript = players[currentCameraPosition].GetComponent<DudeScript>();
		lastDudeScript.MakeActive (false);
		cameraObject.transform.parent = players[position].transform;
		var dudeScript = players[position].GetComponent<DudeScript>();
		dudeScript.MakeActive (true);
		cameraObject.transform.localRotation = Quaternion.identity;
		var scale = players [position].transform.localScale.x;
		cameraObject.transform.localPosition = new Vector3(0, 0.5f/scale, -0.6f/scale);
		currentCameraPosition = position;
	}

	void updateFoodAndWater() {
		foodText.text = "x" + foodUnits.ToString ();
		waterText.text = "x" + waterUnits.ToString ();
	}
}
