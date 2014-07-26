/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class River : MonoBehaviour {

	public List<GameObject> candies;
	public float force = -20f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		foreach (GameObject candy in candies) {
			candy.rigidbody.AddForce(new Vector3(0,0, force));
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Candy") {
			//Debug.Log("in tigger");
			if (candies.Contains(col.gameObject)){
				return;
			} else {
				candies.Add(col.gameObject);
			}
		}
	}

	void OnTriggerStay(Collider col) {

	}
}
