/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class LevelTriggerToTaffy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
	
		if (col.gameObject.tag == "Player") {
			Application.LoadLevel(2);
		}

		
	}
}
