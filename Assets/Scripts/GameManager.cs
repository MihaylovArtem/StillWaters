using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AC.TimeOfDaySystemFree;
using System;


public class GameManager : MonoBehaviour {

	public Camera cameraObject;

	public GameObject[] players;
	public GameObject ship;
	public GameObject targetObject;

	public int currentCameraPosition;

	public static int foodUnits;
	public static readonly int maxFoodUnits = 15;

	public static int waterUnits;
	public static readonly int maxWaterUnits = 10;

	public static bool isFishing;
	public static bool isRelaxing;
	public static bool isRowingLeft = true;
	public static bool isRowingRight = true;

	public Text dayText;
	public Text descriptionText;
	public Text foodText;
	public Text waterText;
	public TimeOfDayManager timeManager;

	private int dayCount = 0;
	private int nightCount = 0;
	private bool countChanged = false;
	public Vector3 startingVector;
	private float overallDistance = 0f;

	public enum GameState {
		MainMenu,
		Playing,
		GameOver
	}
		
	public GameState gameState;

	// Use this for initialization
	void Start () {
		SwitchToPosition (0);
		StartCoroutine (addFoodCycle ());
		foodUnits = maxFoodUnits;
		waterUnits = maxWaterUnits;
		startingVector = ship.transform.position;
		overallDistance = Vector3.Distance (startingVector, targetObject.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		updateFoodAndWater ();
		updateDayCounter ();
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

	IEnumerator addFoodCycle() {
		if (isFishing) {
			foodUnits = Mathf.Min (maxFoodUnits, foodUnits + 1);
		}
		yield return new WaitForSeconds (5.0f);
		StartCoroutine(addFoodCycle ());
	}

	void SwitchToPosition (int position) {
		var lastDudeScript = players[currentCameraPosition].GetComponent<DudeScript>();
		var newDudeScript = players [position].GetComponent <DudeScript> ();
		if (newDudeScript.currentStatus != DudeScript.Status.Died) {
			lastDudeScript.MakeActive (false);
			cameraObject.transform.parent = players [position].transform;
			var dudeScript = players [position].GetComponent<DudeScript> ();
			dudeScript.MakeActive (true);
			cameraObject.transform.localRotation = Quaternion.identity;
			cameraObject.transform.localPosition = new Vector3 (0, 3.5f, 0.5f);
			currentCameraPosition = position;
		}
	}

	void updateFoodAndWater() {
		foodText.text = "x" + foodUnits.ToString ();
		waterText.text = "x" + waterUnits.ToString ();
	}

	void updateDayCounter() {
		if (timeManager.timeline > 7 && timeManager.timeline < 8 && !countChanged) {
			Debug.Log ("Day: " + timeManager.timeline + " " + dayCount);
			dayCount++;
			countChanged = true;
		} else if (timeManager.timeline > 18 && timeManager.timeline < 19 && !countChanged) {
			nightCount++;
			countChanged = true;
		}
		if (timeManager.timeline > 8 && timeManager.timeline < 9) {
			countChanged = false;
		} else if (timeManager.timeline > 19 && timeManager.timeline < 20) {
			countChanged = false; 
		}
		if (timeManager.timeline > 6 && timeManager.timeline < 7) {
			dayText.color = new Color (0, 0, 0, timeManager.timeline - 6);
			descriptionText.color = new Color (0, 0, 0, timeManager.timeline - 6);
			dayText.text = "Day " + dayCount.ToString ();
		} else if (timeManager.timeline > 7.5f && timeManager.timeline < 8.5f) {
			dayText.color = new Color (0, 0, 0, 8.5f - timeManager.timeline);
			descriptionText.color = new Color (0, 0, 0, 8.5f - timeManager.timeline);
			dayText.text = "Day " + dayCount.ToString ();
		} else if (timeManager.timeline > 17 && timeManager.timeline < 18) {
			dayText.color = new Color (0, 0, 0, timeManager.timeline - 17);
			descriptionText.color = new Color (0, 0, 0, timeManager.timeline - 17);
			dayText.text = "Night " + nightCount.ToString ();
		} else if (timeManager.timeline > 18.5f && timeManager.timeline < 19.5f) {
			dayText.color = new Color (0, 0, 0, 19.5f - timeManager.timeline);
			descriptionText.color = new Color (0, 0, 0, 19.5f - timeManager.timeline);
			dayText.text = "Night " + nightCount.ToString ();
		} else if (timeManager.timeline >= 18 && timeManager.timeline <= 18.5f) {
			dayText.color = new Color (0, 0, 0, 1);
			descriptionText.color = new Color (0, 0, 0, 1);
		} else if (timeManager.timeline >= 7 && timeManager.timeline <= 7.5f) {
			dayText.color = new Color (0, 0, 0, 1);
			descriptionText.color = new Color (0, 0, 0, 1);
		} else {
			dayText.color = new Color (0, 0, 0, 0);
			descriptionText.color = new Color (0, 0, 0, 0);
		}

		float distanceToTarget = Vector3.Distance(targetObject.transform.position, ship.transform.position);
		int percent = (int)Math.Floor((1 - (distanceToTarget / overallDistance))*100.0);
		descriptionText.text = "You completed " + percent.ToString ("D") + "% of your journey";

	}

	public void killPersonAndAssignRandom(GameObject whoToKill) {
		foreach (GameObject player in players) {
			var dudeScript = player.GetComponent <DudeScript> ();
			if (dudeScript.currentStatus != DudeScript.Status.Died && player != whoToKill) {
				SwitchToPosition (dudeScript.dudeIndex);
				break;
			}
		}
		whoToKill.GetComponent <DudeScript>().applyStatus (DudeScript.Status.Died);
	}
}
