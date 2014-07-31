/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;

public class Player1Controller : MonoBehaviour {
	public PlayerController player2;
	private bool isRiding = false;
	public float proxThresh;
	public float buttonPressTime = 0.0f;
	public float buttonTimeout = 0.4f;
	private GameController gameController;

	// Use this for initialization
	void Start () {
		player2 = GameObject.Find("Player2").GetComponent<PlayerController>();
		gameController = GameObject.Find("_GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	if (gameController.getIdSelect() == this.GetComponent<PlayerController>().id) {

			if ((Input.GetAxisRaw("LeftTrigger") > 0.8f || Input.GetButtonDown("Ability")) && Time.time > buttonPressTime + buttonTimeout) {// ||  Input.GetButtonDown("PlayerAbility")) {
				buttonPressTime = Time.time;
				if ((player2.transform.position - transform.position).magnitude < proxThresh) {
					if (isRiding) {
						player2.transform.parent = null;
						player2.GetComponent<CharacterController>().enabled = true;
						isRiding = false;
						gameController.camMode = CameraController.CameraMode.Coop;
					} else {
						player2.transform.position = transform.position + transform.up - transform.forward;
						player2.transform.parent = transform;
						player2.GetComponent<CharacterController>().enabled = false;
						isRiding = true;
						gameController.camMode = CameraController.CameraMode.Solo;
					}
				}
			}
		}
	}
}
