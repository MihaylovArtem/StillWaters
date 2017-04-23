using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Camera camera;

	public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;

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
		
	}


}
