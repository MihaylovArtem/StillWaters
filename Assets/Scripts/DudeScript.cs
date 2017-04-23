using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class DudeScript : MonoBehaviour {
	[Range(0, 100)]
	public float water = 100;

	[Range(0, 100)]
	public float food = 100;

	[Range(0, 100)]
	public float energy = 100;

	public bool activeControl = false;

	public enum Status {
		Idle,
		Fishing,
		Sleeping,
		Drinking,
		Eating,
		Rowing,
		Died
	} 

	private string currentAnimation;
	private string previousAnimation;

	private readonly float energyPlusCoef = 1.0f;
	private readonly float energyMinusCoef = 1.0f;

	public Status currentStatus;

	public FirstPersonController controllerScript { get { return this.GetComponent <FirstPersonController> ();} }

	public Animator anim { get { return this.GetComponent <Animator> (); } }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		updateEnergy ();
		updateFood ();
		updateWater ();

		if (activeControl) {
			if (Input.GetKey (KeyCode.W)) {
				currentAnimation = "walk";
			} else if (Input.GetKey (KeyCode.S)) {
				currentAnimation = "walk_back";
			} else if (Input.GetKey (KeyCode.A)) {
				currentAnimation = "walk_left";
			} else if (Input.GetKey (KeyCode.D)) {
				currentAnimation = "walk_right";
			} else {
				currentAnimation = "idle";
			}
		} else {
			currentAnimation = "idle";
		}
		anim.Play (currentAnimation);
	}

	void updateFood () {
		float standartCoef = -1.0f;
		float coef = 0.0f;
		switch (currentStatus) {
		case Status.Fishing:
		case Status.Idle:
		case Status.Drinking:
			coef = 1.0f * standartCoef;
			break;
		case Status.Died:
			coef = 0.0f * standartCoef;
			break;
		case Status.Sleeping:
			coef = 0.5f * standartCoef;
			break;
		case Status.Rowing:
			coef = 2.0f * standartCoef;
			break;
		case Status.Eating:
			coef = -5.0f * standartCoef;
			break;
		}
		food += coef * Time.deltaTime;
	}

	void updateWater () {
		float standartCoef = -1.0f;
		float coef = 0.0f;
		switch (currentStatus) {
		case Status.Idle:
		case Status.Eating:
			coef = 1.0f * standartCoef;
			break;
		case Status.Died:
			coef = 0.0f * standartCoef;
			break;
		case Status.Sleeping:
			coef = 1.5f * standartCoef;
			break;
		case Status.Rowing:
		case Status.Fishing:
			coef = 2.0f * standartCoef;
			break;
		case Status.Drinking:
			coef = -5.0f * standartCoef;
			break;
		}
		water += coef * Time.deltaTime;
	}

	void updateEnergy () {
		float standartCoef = -1.0f;
		float coef = 0.0f;
		switch (currentStatus) {
		case Status.Eating:
		case Status.Drinking:
			coef = -0.5f * standartCoef;
			break;
		case Status.Died:
		case Status.Idle:
			coef = 0.0f * standartCoef;
			break;
		case Status.Rowing:
		case Status.Fishing:
			coef = 2.5f * standartCoef;
			break;
		case Status.Sleeping:
			coef = -5.0f * standartCoef;
			break;
		}
		energy += coef * Time.deltaTime;
	}

	public void MakeActive(bool active) {
		activeControl = true;
		controllerScript.enabled = active;
	}
}
