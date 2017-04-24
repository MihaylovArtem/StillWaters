using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC.TimeOfDaySystemFree;

public class AudioScript : MonoBehaviour {

	public AudioSource dayMusic;
	public AudioSource nightMusic;
	public TimeOfDayManager timeManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var ratio = 1f;
		if (timeManager.timeline > 7 && timeManager.timeline < 18) {
			ratio = 1f;
		} else if (timeManager.timeline < 6 || timeManager.timeline > 19) {
			ratio = 0f;
		} else if (timeManager.timeline <= 7) {
			ratio = 1-(7.0f - timeManager.timeline);
		} else {
			ratio = (19.0f - timeManager.timeline);
		}
		dayMusic.volume = ratio;

		nightMusic.volume = (1f - ratio)*0.7f; // night music is louder :D
		Debug.Log (ratio + " " + nightMusic.volume);
	}
}
