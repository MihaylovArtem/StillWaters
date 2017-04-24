using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	private string[] thoughts = new string[15] {"What a great day!", "Does anyone else hear this song?", "Yummi, fish again!", 
		"* whistling*", "Let's make a pool-party!", "Did I turn off the iron?!", "Is the water warm?..", "Hmm... How did I get here?", 
		"Sh*t! I's a hockey match today!", "Bazinga!", "I wanna coffee...", "Yo ho ho and a bottle of rum!", 
		"Столица, водка, советский медведь наш!", "Tequila to all at my expense", "*Burp*"};
	public Text thoughtText;
	public GameObject thoughtPanel;

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
	public GameManager gameManager;

	public RaycastHit whatIHit;
	private AudioSource source { get { return GetComponent<AudioSource> (); } }
	public AudioClip deathSound;
	public AudioClip sighSound;
	public AudioClip eatSound;
	public AudioClip drinkSound;
	public int dudeIndex;


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
		gameObject.AddComponent<AudioSource>();
		source.playOnAwake = false;
		var coroutine = sayRandomThought();
		StartCoroutine(coroutine);
	}
	
	// Update is called once per frame
	void Update () {
		updateEnergy ();
		updateFood ();
		updateWater ();
		checkInteraction ();
		checkValues ();

		controllerScript.m_WalkSpeed = (currentStatus == Status.Idle && activeControl) ? 1 : 0;
		if (activeControl && currentStatus == Status.Idle) {
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
		food = Mathf.Max (food + coef * Time.deltaTime, 0);
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
		activeControl = active;
		controllerScript.enabled = active;
	}

	public void startAction(Status status) {
		if (currentStatus == Status.Died) {
			return;
		}
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

	public void applyStatus(Status status) {
		cancelStatus (currentStatus);
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
		if (activeControl) {
			if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out whatIHit, 1f) && activeControl) {
				if (Input.GetKeyUp (KeyCode.E)) {
					if (currentStatus != Status.Idle) {
						applyStatus (Status.Idle);
					} else {
						switch (whatIHit.collider.gameObject.tag) {
						case "FishingObject":
							applyStatus (Status.Fishing);
							break;
						case "RelaxObject":
							applyStatus (Status.Sleeping);
							break;
						case "FoodObject":
							eat ();
							break;
						case "WaterObject":
							drink ();
							break;
						case "LeftPaddle":
							applyStatus (Status.RowingLeft);
							break;
						case "RightPaddle":
							applyStatus (Status.RowingRight);
							break;
						}
					}
				}
			} else if (Input.GetKeyUp (KeyCode.E)) {
				if (currentStatus != Status.Idle) {
					applyStatus (Status.Idle);
				}
			}
		}
	}

	// every 2 seconds perform the print()
	private IEnumerator sayRandomThought()
	{
		while (true)
		{
			var waitTime = Random.Range (20f, 70f);
			yield return new WaitForSeconds(waitTime);

			if (currentStatus != Status.Died && !activeControl) {
				source.PlayOneShot (sighSound);
				thoughtPanel.SetActive (true);
				int index = Random.Range(0, thoughts.Length);
				var thought = thoughts [index];
				thoughtText.text = thought;
				Invoke ("hideThoughtPanel", 8.0f);
			}
		}
	}

	void eat() {
		source.PlayOneShot (eatSound);
		food = Mathf.Max (food + 50, maxFood);
		GameManager.foodUnits = Mathf.Max (GameManager.foodUnits - 1, 0);
	}

	void drink() {
		source.PlayOneShot (drinkSound);
		water = Mathf.Max (water + 50, maxWater);
		GameManager.waterUnits = Mathf.Max (GameManager.waterUnits - 1, 0);
	}

	void hideThoughtPanel() {
		thoughtPanel.SetActive (false);
	}

	void checkValues () {
		if (water <= 0 || food <= 0 || energy <= 0) {
			var newRotation = new Quaternion ();
			newRotation.eulerAngles = new Vector3 (-90f, 0f, 0f);
			this.transform.rotation = newRotation;
			gameManager.killPersonAndAssignRandom (this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "WaterPlane") {
			gameManager.killPersonAndAssignRandom (this.gameObject);
			source.PlayOneShot (deathSound);
		}
	}
}
