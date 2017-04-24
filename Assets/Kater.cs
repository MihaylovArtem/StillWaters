using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kater : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent <Rigidbody>().velocity = Vector3.back * 45f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
