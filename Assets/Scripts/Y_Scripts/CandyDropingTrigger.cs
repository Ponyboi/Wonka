/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */

using UnityEngine;
using System.Collections;

public class CandyDropingTrigger : MonoBehaviour {

	bool playerInBox = false;
	GameObject[] mints;


	// Use this for initialization
	void Start () {
		mints =  GameObject.FindGameObjectsWithTag ("mint");

	}
	
	// Update is called once per frame
	void Update () {
		if(playerInBox){
			for(int i = 0 ; i < mints.Length ; i ++){
				if(mints[i].gameObject.rigidbody != null){
					mints[i].gameObject.rigidbody.useGravity = true;
				}

			}


		}
	}

	void OnTriggerEnter(Collider col){

		playerInBox = true;

		Debug.Log("entered");

	}


}
