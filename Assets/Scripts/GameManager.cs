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
	public static readonly int maxFoodUnits = 10;

	public static int waterUnits;
	public static readonly int maxWaterUnits = 16;

	public static bool isFishing = false;
	public static bool isRelaxing = false;
	public static bool isRowingLeft = false;
	public static bool isRowingRight = false;

	public Text dayText;
	public Text descriptionText;
	public Text foodText;
	public Text waterText;
	public Text tutorialText;
	public Image tutorialPanel;
	public TimeOfDayManager timeManager;
	private float tutorialTimer;
	private int currentTutorialStep;
	private string[] tutorialStrings = new string[4] {"You are starting your journey\nYour goal is to reach the island by using paddles.", "Survive in this small world by controlling your hunger, thirst and tiredness", "Control it by interacting with the objects by pressing E\nBe careful, water is not recoverable!", "Switch between characters by pressing 1, 2, 3, 4 numbers"};
	private int deadDudes = 0;

	private int dayCount = 0;
	private int nightCount = 0;
	private bool countChanged = false;
	public Vector3 startingVector;
	private float overallDistance = 0f;

	private bool tutorialComplete = false;

	public GameObject gameOverPanel;
	public Text gameOverTitle;
	public Text gameOverDescription;

	public enum GameState {
		MainMenu,
		Playing,
		GameOver
	}
		
	static public GameState gameState;

	// Use this for initialization
	void Start () {
		UnityEngine.Cursor.visible = false;
		tutorialComplete = PlayerPrefs.HasKey ("tutorialComplete");
		gameState = GameState.Playing;
		deadDudes = 0;
		SwitchToPosition (0);
		StartCoroutine (addFoodCycle ());
		foodUnits = 4;
		waterUnits = maxWaterUnits;
		startingVector = ship.transform.position;
		overallDistance = Vector3.Distance (startingVector, targetObject.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		updateFoodAndWater ();
		updateDayCounter ();
		updateTutorial ();
		checkGameState ();
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
		if (position < 4) {
			var lastDudeScript = players [currentCameraPosition].GetComponent<DudeScript> ();
			var newDudeScript = players [position].GetComponent <DudeScript> ();
			if (newDudeScript.currentStatus != DudeScript.Status.Died) {
				lastDudeScript.MakeActive (false);
				cameraObject.transform.parent = players [position].transform;
				var dudeScript = players [position].GetComponent<DudeScript> ();
				dudeScript.MakeActive (true);
				cameraObject.transform.localRotation = Quaternion.identity;
				cameraObject.transform.localPosition = new Vector3 (0, 3.5f, 1f);
				currentCameraPosition = position;
			}
		} else {
			cameraObject.transform.parent = ship.transform;
			cameraObject.transform.localRotation = Quaternion.identity;
			cameraObject.transform.localPosition = new Vector3 (0, 3.5f, 1f);
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

	void updateTutorial() {
		if (!tutorialComplete) {
			if (currentTutorialStep < tutorialStrings.Length) {
				tutorialTimer += Time.deltaTime;
				tutorialText.text = tutorialStrings [currentTutorialStep];
				if (tutorialTimer < 1) {
					tutorialText.color = new Color (0, 0, 0, tutorialTimer);
					tutorialPanel.color = new Color (1, 1, 1, tutorialTimer * 0.4f);
				} else if (tutorialTimer > 5 && tutorialTimer < 6) {
					tutorialText.color = new Color (0, 0, 0, 6 - tutorialTimer);
					tutorialPanel.color = new Color (1, 1, 1, (6 - tutorialTimer) * 0.4f);
				} else if (tutorialTimer > 6) {
					tutorialTimer = 0.0f;
					currentTutorialStep++;
				}
			} else {
				tutorialPanel.gameObject.SetActive (false);
				PlayerPrefs.SetInt ("tutorialComplete", 1);
			}
		} else {
			tutorialPanel.gameObject.SetActive (false);
		}
	}

	void checkGameState() {

		float distanceToTarget = Vector3.Distance(targetObject.transform.position, ship.transform.position);
		int percent = (int)Math.Round((1 - (distanceToTarget / overallDistance))*100.0);
		if (deadDudes == players.Length) {
			gameState = GameState.GameOver;
			gameOverPanel.SetActive (true);
			gameOverTitle.text = "Game Over!";
			gameOverDescription.text = "You completed " + percent.ToString ("D") + "% of your journey";
			PlayerPrefs.SetInt ("HighscoreCompleted", percent);
		} else if (percent >= 100) {
			foreach (GameObject player in players) {
				player.GetComponent <DudeScript>().MakeActive (false);
			}
			gameState = GameState.GameOver;
			gameOverPanel.SetActive (true);
			gameOverTitle.text = "You won!";
			gameOverDescription.text = "You have completed your journey with " + (players.Length - deadDudes).ToString () + " people alive!\n\nWrite us \"Thank you, Hanzo mains!\" to prove your win ;)";
			PlayerPrefs.SetInt ("PeopleAlive", (players.Length - deadDudes));
		}
		if (gameState == GameState.GameOver) {
			if (Input.GetKeyUp (KeyCode.E)) {
				Application.LoadLevel ("menu");
			}
		}
	}

	public void killPersonAndAssignRandom(GameObject whoToKill) {
		if (whoToKill == players [currentCameraPosition]) {
			SwitchToPosition (5);
			foreach (GameObject player in players) {
				var dudeScript = player.GetComponent <DudeScript> ();
				if (dudeScript.currentStatus != DudeScript.Status.Died && player != whoToKill) {
					SwitchToPosition (dudeScript.dudeIndex);
					break;
				}
			}
		}
		whoToKill.GetComponent <DudeScript>().applyStatus (DudeScript.Status.Died);
		deadDudes++;
	}
}
