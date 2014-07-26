/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class moveFence : MonoBehaviour {

	public GameObject node1;
	public FenceTrigger button;
	public Vector3 endPosition;
	public float delay;
	public bool audioPlayed = false;

	// Use this for initialization
	void Start () {
		button = GameObject.Find("trigger1").GetComponent<FenceTrigger>();
		node1 = GameObject.Find("node1");
		endPosition = node1.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		if(button.fenceButtonPushed){
			transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * delay);
			if(audioPlayed == false){
				audio.Play();
				audioPlayed = true;
			}

		}
	}




}
