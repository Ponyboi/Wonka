/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class FenceTrigger : MonoBehaviour {

	public bool playerIsInTrigger = false;
	public bool fenceButtonPushed = false;
	public SwitchCamera camera;
	public Y_PlayerController player1;
	public Y_PlayerController player2;
	public int timeDelay;


	// Use this for initialization
	void Start () {
		camera = GameObject.Find("Y_Cameras").GetComponent<SwitchCamera>();
		player1 = GameObject.Find("Player1").GetComponent<Y_PlayerController>();
		player2 = GameObject.Find("Player2").GetComponent<Y_PlayerController>();

	}
	
	// Update is called once per frame
	void Update () {
		if(fenceButtonPushed){
			timeDelay--;
			if(timeDelay == 0){
				//camera.currentCamera = 3;

			}
			
		}
	}

	void OnTriggerStay(Collider other){
		if(this.gameObject.tag == "t1" && other.gameObject.tag == "SmallPlayer"){
			if(Input.GetKey(KeyCode.O)){
				fenceButtonPushed = true;
			}
		}




		
	}
}
