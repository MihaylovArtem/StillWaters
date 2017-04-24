using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectScript : MonoBehaviour {

	public bool isVisisble = false;
	public Text text;
	public Image panel;
	private string startingText;
	public GameObject dude;

	// Use this for initialization
	void Start () {
		startingText = text.text;
	}
	
	// Update is called once per frame
	void Update () {
		if (isVisisble) {
			Debug.Log ("Im visible!");
			panel.color = new Color (1, 1, 1, 0.4f);
			text.color = new Color (0, 0, 0, 1);
		} else {
			panel.color = new Color (1, 1, 1, 0);
			text.color = new Color (0, 0, 0, 0);
		}
		if (dude != null) {
			if (dude.GetComponent <DudeScript> ().currentStatus == DudeScript.Status.Idle || dude.GetComponent <DudeScript> ().currentStatus == DudeScript.Status.Died) {
				dude = null;
			}
			text.text = "Press E to cancel";
		} else {
			text.text = startingText;
		}
		isVisisble = false;
	}
}
