/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class OnRoof : MonoBehaviour {

	public bool player1onRoof = false;
	public bool player2onRoof = false;
	public SwitchCamera camera;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find("Y_Cameras").GetComponent<SwitchCamera>();

	}
	
	// Update is called once per frame
	void Update () {
		if(player1onRoof && player2onRoof){
			camera.currentCamera = 2;

		}

	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "BigPlayer"){
			player1onRoof = true;
			Debug.Log("player 1 on roof");
		}
		if(other.gameObject.tag == "SmallPlayer"){
			player2onRoof = true;
			Debug.Log("player 2 on roof");

		}

	}
}
