using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class DudeScript : MonoBehaviour {
	[Range(0, 100)]
	public float water = 100;
	public static readonly float maxWater = 100;

	[Range(0, 100)]
	public float food = 100;
	public static readonly float maxFood = 100;

	[Range(0, 100)]
	public float energy = 100;
	public static readonly float maxEnergy = 100;

	public bool activeControl = false;

	public enum Status {
		Idle,
		Fishing,
		Sleeping,
		RowingLeft,
		RowingRight,
		Died
	} 

	private string currentAnimation;
	private string previousAnimation;
	public Status currentStatus;

	public RaycastHit whatIHit;

	private FirstPersonController _controllerScript;
	public  FirstPersonController  controllerScript { 
		get { 
			if (!_controllerScript) {
				_controllerScript = this.GetComponent <FirstPersonController> ();
			}
			return _controllerScript;
		} 
	}

	public Animator anim { get { return this.GetComponent <Animator> (); } }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		updateEnergy ();
		updateFood ();
		updateWater ();
		checkInteraction ();

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
		case Status.Died:
			coef = 0.0f * standartCoef;
			break;
		case Status.Idle:
			coef = 0.3f * standartCoef;
			break;
		case Status.Sleeping:
			coef = 0.5f * standartCoef;
			break;
		case Status.RowingLeft:
		case Status.RowingRight:
			coef = 2.0f * standartCoef;
			break;
		}
		food += coef * Time.deltaTime;
	}

	void updateWater () {
		float standartCoef = -1.0f;
		float coef = 0.0f;
		switch (currentStatus) {
		case Status.Idle:
			coef = 1.0f * standartCoef;
			break;
		case Status.Died:
			coef = 0.0f * standartCoef;
			break;
		case Status.Sleeping:
			coef = 1.5f * standartCoef;
			break;
		case Status.RowingLeft:
		case Status.RowingRight:
		case Status.Fishing:
			coef = 2.0f * standartCoef;
			break;
		}
		water += coef * Time.deltaTime;
	}

	void updateEnergy () {
		float standartCoef = -1.0f;
		float coef = 0.0f;
		switch (currentStatus) {
		case Status.Died:
		case Status.Idle:
			coef = 0.0f * standartCoef;
			break;
		case Status.RowingLeft:
		case Status.RowingRight:
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

	public void startAction(Status status) {
		if (currentStatus == Status.Died) {
			return;
		}
		cancelStatus (currentStatus);
		applyStatus (status);
	}

	private void cancelStatus(Status status) {
		switch (status) {
		case Status.Died:
		case Status.Idle:
			break;
		case Status.Fishing:
			GameManager.isFishing = false;
			break;
		case Status.RowingLeft:
			GameManager.isRowingLeft = false;
			break;
		case Status.RowingRight:
			GameManager.isRowingRight = false;
			break;
		case Status.Sleeping:
			GameManager.isRelaxing = false;
			break;
		}
	}

	private void applyStatus(Status status) {
		currentStatus = status;
		switch (status) {
		case Status.Died:
		case Status.Idle:
			break;
		case Status.Fishing:
			GameManager.isFishing = true;
			break;
		case Status.RowingLeft:
			GameManager.isRowingLeft = true;
			break;
		case Status.RowingRight:
			GameManager.isRowingRight = true;
			break;
		case Status.Sleeping:
			GameManager.isRelaxing = true;
			break;
		}
	}

	void checkInteraction () {
		if (Physics.Raycast (this.transform.position, Camera.main.transform.forward, out whatIHit, 1f) && activeControl) {
			//TODO: надпись press "E"
		}
	}
}
