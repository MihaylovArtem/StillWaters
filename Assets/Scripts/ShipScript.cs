using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour {

	private Rigidbody shipRigidbody;

	// Use this for initialization
	void Start () {
		shipRigidbody = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		setSpeed (GameManager.numberOfRowers);
	}

	void setSpeed(int numberOfRowers = 0) {
		shipRigidbody.velocity = new Vector3 (0, 0, 10 * numberOfRowers / GameManager.maxNumberOfRowers);
	}
}
