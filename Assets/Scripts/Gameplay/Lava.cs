/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {

	public GameObject resetPos;
	public float sinkRate = 0.3f;
	public float sinkDist = 1;
	public PlayerController player;

	// Use this for initialization
	void Start () {
		resetPos = Nozzle.TraverseHierarchy(transform, "ResetPos").gameObject;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider col) {
//		if (col.gameObject.tag == "Player") {
//			col.transform.position = resetPos.transform.position;
//		}

		if (col.gameObject.tag == "Player") {
			player = col.GetComponent<PlayerController>();
			player.transform.position = resetPos.transform.position;
		}


	}
}
