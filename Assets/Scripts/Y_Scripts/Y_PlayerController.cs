/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class Y_PlayerController : MonoBehaviour {

	public SwitchCamera cameraScript;
	public CharacterController controller1;
	public CharacterController controller2;
	public bool onLadder1 = false;
	public bool onLadder2 = false;
	public GameObject player1;
	public GameObject player2;
	public float runSpeed;
	public bool player1active = false;
	public bool player2active = false;
	public GameObject camera;
	public bool isGrounded1 = true;
	public bool isGrounded2 = true;
	public float jumpHeight1;
	public float jumpHeight2;
	public float gravity;
	public float groundY1;
	public float groundY2;

	// Use this for initialization
	void Start () {
		controller1 = GameObject.FindGameObjectWithTag("BigPlayer").GetComponent<CharacterController>();
		controller2 = GameObject.FindGameObjectWithTag("SmallPlayer").GetComponent<CharacterController>();
		cameraScript = GameObject.Find("Y_Cameras").GetComponent<SwitchCamera>();
		camera = cameraScript.currentCam;

		groundY1 = controller1.transform.position.y;
		groundY2 = controller2.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		camera = cameraScript.currentCam;
		getInputs();
		jump ();
	}

	void applyGravity(){

		Vector3 p1m = new Vector3(0,0, 0);
		p1m.y -= gravity * Time.deltaTime;
		controller1.Move(p1m);

		Vector3 p2m = new Vector3(0,0, 0);
		p2m.y -= gravity * Time.deltaTime;
		controller2.Move(p2m);
	}

	void jump(){
		
		//jump
		if(Input.GetKeyDown(KeyCode.Space) && isGrounded1){
			controller1.Move (Vector3.up * jumpHeight1);
			isGrounded1 = false;
		}
		//jump
		if(Input.GetKeyDown(KeyCode.RightAlt) && isGrounded2){
			controller2.Move (Vector3.up * jumpHeight2);
			isGrounded2 = false;
		}

	}

	void getInputs(){
		if(player1active && onLadder1 == false){

			if( Input.GetKey(KeyCode.W)){
				controller1.Move (camera.transform.forward * runSpeed);
			}
			if( Input.GetKey(KeyCode.S)){
				controller1.Move (camera.transform.forward * -1.0f * runSpeed);
				applyGravity();

			}
			if( Input.GetKey(KeyCode.D)){
				controller1.Move (camera.transform.right * runSpeed);
			}
			if( Input.GetKey(KeyCode.A)){
				controller1.Move (camera.transform.right * -1.0f * runSpeed);
			}
			applyGravity();
		

		}

		if(player2active && onLadder2 == false ){


			if( Input.GetKey(KeyCode.P)){
				controller2.Move (camera.transform.forward * runSpeed);
			}
			if( Input.GetKey(KeyCode.Semicolon)){
				controller2.Move (camera.transform.forward * -1.0f * runSpeed);
				applyGravity();

			}
			if( Input.GetKey(KeyCode.Quote)){
				controller2.Move (camera.transform.right * runSpeed);
			}
			if( Input.GetKey(KeyCode.L)){
				controller2.Move (camera.transform.right * -1.0f * runSpeed);
			}

			applyGravity();

		}



	}

	void OnControllerColliderHit(ControllerColliderHit hit){
		if(this.gameObject.tag == "BigPlayer"){
			isGrounded1 = true;
		}
		if(this.gameObject.tag == "SmallPlayer"){
			isGrounded2 = true;
		}
	}


}
