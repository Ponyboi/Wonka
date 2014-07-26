/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class LadderClimb : MonoBehaviour {

	public Y_PlayerController player1;
	public Y_PlayerController player2;
	public SwitchCamera camera;

	public bool climbing1 = false;
	public bool climbing2 = false;
	public CharacterController controller1;
	public CharacterController controller2;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find("Y_Cameras").GetComponent<SwitchCamera>();

		player1 = GameObject.Find("Player1").GetComponent<Y_PlayerController>();
		player2 = GameObject.Find("Player2").GetComponent<Y_PlayerController>();

		controller1 = GameObject.FindGameObjectWithTag("BigPlayer").GetComponent<CharacterController>();
		controller2 = GameObject.FindGameObjectWithTag("SmallPlayer").GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(climbing1){
			player1.onLadder1 = true;
			player2.onLadder1 = true;
			player1.gravity = 0.0f;
			if(Input.GetKey(KeyCode.W)){
				controller1.Move(Vector3.up);
			}
		}
		if(climbing2){
			player1.onLadder2 = true;
			player2.onLadder2 = true;
			player2.gravity = 0.0f;
			if(Input.GetKey(KeyCode.P)){
				controller2.Move(Vector3.up);
			}

		}

		if(climbing1 == false){
			player1.onLadder1 = false;
			player2.onLadder1 = false;
			player1.gravity = 0.0f;
		}

		if(climbing2 == false){
			player1.onLadder2 = false;
			player2.onLadder2 = false;
			player2.gravity = 10.0f;
		}

	

	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "BigPlayer"){
			climbing1 = true;
			camera.currentCamera = 1;

		}
		if(other.gameObject.tag == "SmallPlayer"){
			climbing2 = true;
			camera.currentCamera = 1;
		}

	}


	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "BigPlayer"){
			climbing1 = false;
			
		}
		if(other.gameObject.tag == "SmallPlayer"){
			climbing2 = false;
			

		}
		
	}


}
