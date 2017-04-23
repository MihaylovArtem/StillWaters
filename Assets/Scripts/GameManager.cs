using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Camera camera;

	public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;
	public static int foodUnits;
	public static readonly int maxFoodUnits = 15;

	public static int waterUnits;
	public static readonly int maxWaterUnits = 10;

	public static int numberOfRowers;
	public static readonly int maxNumberOfRowers = 2;

	public static bool isFishing;

	public enum GameState {
		MainMenu,
		Playing,
		GameOver
	}
		
	public GameState gameState;

	public static int numberOfRowers;
	public static readonly int maxNumberOfRowers = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		AddFood ();
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


}
