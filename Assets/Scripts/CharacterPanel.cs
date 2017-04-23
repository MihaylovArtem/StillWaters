using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour {

	public Image hungerImage;
	public Image waterImage;
	public Image sleepImage;

	public DudeScript dude;

	// Use this for initializatiosn
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		updateEnergyColor ();
		updateFoodColor ();
		updateWaterColor ();
	}

	void updateFoodColor () {
		var food = dude.food;
		var red = (food <= DudeScript.maxFood / 2) ? 1.0f : (DudeScript.maxFood - food) / (DudeScript.maxFood / 2);
		var green = (food >= DudeScript.maxFood / 2) ? 1.0f : (food / (DudeScript.maxFood / 2));
		hungerImage.color = new Color (red, green, 0.0f);
	}

	void updateWaterColor () {
		var water = dude.water;
		var red = (water <= DudeScript.maxWater / 2) ? 1.0f : (DudeScript.maxWater - water) / (DudeScript.maxWater / 2);
		var green = (water >= DudeScript.maxWater / 2) ? 1.0f : (water / (DudeScript.maxWater / 2));
		waterImage.color = new Color (red, green, 0.0f);
	}

	void updateEnergyColor () {
		var energy = dude.energy;
		var red = (energy <= DudeScript.maxEnergy / 2) ? 1.0f : (DudeScript.maxEnergy - energy) / (DudeScript.maxEnergy / 2);
		var green = (energy >= DudeScript.maxEnergy / 2) ? 1.0f : (energy / (DudeScript.maxEnergy / 2));
		sleepImage.color = new Color (red, green, 0.0f);
	}
}
