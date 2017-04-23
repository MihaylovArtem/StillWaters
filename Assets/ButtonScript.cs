using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	public AudioClip hoverSound;
	public AudioClip clickSound;
	public Color hoverColor = new Color(0.8f,0.8f,0.8f);
	public Color unhoverColor = new Color(1,1,1);

	private Button button { get { return GetComponent<Button> (); } }

	private Text text { get { return this.transform.GetChild (0).GetComponent<Text> (); } }
	private AudioSource source { get { return GetComponent<AudioSource> (); } }

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<AudioSource>();
		source.playOnAwake = false;
	}

	public void Unhover() {
		text.color = unhoverColor;
	}

	public void PlayHover() {
		text.color = hoverColor;
		source.PlayOneShot (hoverSound);
	}

	public void PlayClick() {
		source.PlayOneShot (clickSound);
	}
}
