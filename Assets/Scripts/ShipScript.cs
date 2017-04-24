using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		setSpeed ();
	}

	void setSpeed () {
		var speedMultiplier = (Convert.ToInt32 (GameManager.isRowingLeft) + Convert.ToInt32 (GameManager.isRowingRight));
		transform.Translate (new Vector3(1,0,0) * Time.deltaTime * speedMultiplier);
	}
}
