/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour {

	public AudioSource source;
	public AudioClip clip;

	public enum CandyType {Reeses, GumDrop, Peppermint, BubbleGum, CandyBar};
	public CandyType candyType;

	// Use this for initialization
	void Start () {
		source = audio;
		source.clip = clip;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayAudio() {
		source.Play();
	}

	void OnCollisionEnter(Collision col) {
		//Debug.Log("play sound");
		//source.Play ();
		if (col.gameObject.tag == "Player") {
			source.Play ();
		}

		switch (candyType) {
			case CandyType.BubbleGum:

			break;
		}
	}
}
