/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class PlayerInTrigger : MonoBehaviour {

	public Y_PlayerController player;
	public SwitchCamera currentCamera;
	public Y_FollowCamera followingCamera;


	// Use this for initialization
	void Start () {
		currentCamera = GameObject.Find("Y_Cameras").GetComponent<SwitchCamera>();
		player = GameObject.Find("Player2").GetComponent<Y_PlayerController>();
		followingCamera = GameObject.Find("Main Camera4").GetComponent<Y_FollowCamera>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other){
		if(this.gameObject.tag == "c0"  && currentCamera.currentCamera != 3){
			currentCamera.currentCamera = 0;
		}
		else if(this.gameObject.tag == "c1"  && currentCamera.currentCamera != 3){
			currentCamera.currentCamera = 1;
		}
		else if(this.gameObject.tag == "c2" && currentCamera.currentCamera != 3){
			currentCamera.currentCamera = 2;
		}
		/*
		else if(this.gameObject.tag == "c3"){
			followingCamera.turnAngle = 90.0f;
			
		}
		else if(this.gameObject.tag == "c4"){
			followingCamera.turnAngle = 240.0f;
			
		}
		*/
		
	}

}
