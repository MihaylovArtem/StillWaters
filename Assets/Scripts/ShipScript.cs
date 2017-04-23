using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipScript : MonoBehaviour {

	private Rigidbody _shipRigidbody;
	private Rigidbody shipRigidbody {
		get {
			if (!_shipRigidbody) {
				_shipRigidbody = gameObject.GetComponent<Rigidbody> ();
			}
			return _shipRigidbody;
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		setSpeed ();
	}

	void setSpeed () {
		var speedMultiplier = (Convert.ToInt32 (GameManager.isRowingLeft) + Convert.ToInt32 (GameManager.isRowingRight));
		shipRigidbody.velocity = new Vector3 (0, 0, 10 * speedMultiplier);
	}
}
