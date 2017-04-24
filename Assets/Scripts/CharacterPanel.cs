using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour {

	public Image hungerImage;
	public Image waterImage;
	public Image sleepImage;
	public Text status;
	public Image panelBack  { get { return this.GetComponent<Image> (); } }
	public DudeScript dude;
	private Color activeColor = new Color (0f, 1f, 0f, 0.4f);
	private Color nonActiveColor = new Color (1f, 1f, 1f, 0.4f);
	private Color deadColor = new Color (1f, 0f, 0f, 0.4f);

	// Use this for initializatiosn
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		updateStatus ();
		if (GameManager.gameState != GameManager.GameState.GameOver) {
			updateEnergyColor ();
			updateFoodColor ();
			updateWaterColor ();
		}
	}

	void updateStatus () {
		panelBack.color = dude.activeControl ? activeColor : nonActiveColor;
		if (dude.currentStatus == DudeScript.Status.Died) {
			panelBack.color = deadColor;
		}
		var text = "";
		switch (dude.currentStatus) {
		case DudeScript.Status.Fishing:
			text = "Fishing...";
			break;
		case DudeScript.Status.RowingLeft:
		case DudeScript.Status.RowingRight:
			text = "Rowing...";
			break;
		case DudeScript.Status.Sleeping:
			text = "Resting...";
			break;
		case DudeScript.Status.Idle:
			text = "Not busy";
			break;
		case DudeScript.Status.Died:
			text = "Dead";
			break;
		}
		status.text = text;
	}

	void updateFoodColor () {
		
		var food = dude.food;
		var alpha = food > DudeScript.maxFood / 4f * 3f ? 0f : (DudeScript.maxFood - food) / (DudeScript.maxFood / 4f);
		var red = (food <= DudeScript.maxFood / 2) ? 1.0f : (DudeScript.maxFood - food) / (DudeScript.maxFood / 2);
		var green = (food >= DudeScript.maxFood / 2) ? 1.0f : (food / (DudeScript.maxFood / 2));
		hungerImage.color = new Color (red, green, 0.0f, alpha);
	}

	void updateWaterColor () {
		var water = dude.water;
		var alpha = water > DudeScript.maxWater / 4f * 3f ? 0f : (DudeScript.maxWater - water) / (DudeScript.maxWater / 4f);
		var red = (water <= DudeScript.maxWater / 2) ? 1.0f : (DudeScript.maxWater - water) / (DudeScript.maxWater / 2);
		var green = (water >= DudeScript.maxWater / 2) ? 1.0f : (water / (DudeScript.maxWater / 2));
		waterImage.color = new Color (red, green, 0.0f, alpha);
	}

	void updateEnergyColor () {
		var energy = dude.energy;
		var alpha = energy > DudeScript.maxEnergy / 4f * 3f ? 0f : (DudeScript.maxEnergy - energy) / (DudeScript.maxEnergy / 4f);
		var red = (energy <= DudeScript.maxEnergy / 2) ? 1.0f : (DudeScript.maxEnergy - energy) / (DudeScript.maxEnergy / 2);
		var green = (energy >= DudeScript.maxEnergy / 2) ? 1.0f : (energy / (DudeScript.maxEnergy / 2));
		sleepImage.color = new Color (red, green, 0.0f, alpha);
	}
}
