﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeScript : MonoBehaviour {
	[Range(0, 100)]
	public int water = 100;

	[Range(0, 100)]
	public int food = 100;

	[Range(0, 100)]
	public int energy = 100;

	enum Status {
		Idle,
		Fishing,
		Sleeping,
		Drinking,
		Eating,
		Rowing,
		Died
	} 

	private readonly float energyPlusCoef = 1.0;
	private readonly float energyMinusCoef = 1.0;

	public Status currentStatus;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		updateEnergy ();
		updateFood ();
		updateWater ();
	}

	void updateFood () {
		float standartCoef = -1.0;
		float coef;
		switch (currentStatus) {
		case Status.Fishing:
		case Status.Idle:
		case Status.Drinking:
			coef = 1.0 * standartCoef;
			break;
		case Status.Died:
			coef = 0.0 * standartCoef;
			break;
		case Status.Sleeping:
			coef = 0.5 * standartCoef;
			break;
		case Status.Rowing:
			coef = 2.0 * standartCoef;
			break;
		case Status.Eating:
			coef = -5.0 * standartCoef;
			break;
		}
		food += coef * Time.deltaTime;
	}

	void updateWater () {
		float standartCoef = -1.0;
		float coef;
		switch (currentStatus) {
		case Status.Idle:
		case Status.Eating:
			coef = 1.0 * standartCoef;
			break;
		case Status.Died:
			coef = 0.0 * standartCoef;
			break;
		case Status.Sleeping:
			coef = 1.5 * standartCoef;
			break;
		case Status.Rowing:
		case Status.Fishing:
			coef = 2.0 * standartCoef;
			break;
		case Status.Drinking:
			coef = -5.0 * standartCoef;
			break;
		}
		water += coef * Time.deltaTime;
	}

	void updateEnergy () {
		float standartCoef = -1.0;
		float coef;
		switch (currentStatus) {
		case Status.Eating:
		case Status.Drinking:
			coef = -0.5 * standartCoef;
			break;
		case Status.Died:
		case Status.Idle:
			coef = 0.0 * standartCoef;
			break;
		case Status.Rowing:
		case Status.Fishing:
			coef = 2.5 * standartCoef;
			break;
		case Status.Sleeping:
			coef = -5.0 * standartCoef;
			break;
		}
		energy += coef * Time.deltaTime;
	}
}