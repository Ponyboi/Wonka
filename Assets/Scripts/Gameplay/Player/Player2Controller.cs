/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player2Controller : MonoBehaviour {
	public GameController gameController;
	public PlayerController player1;
	public PlayerController player2;
	public Nozzle nozzle;
	public GameObject[] panels;
	public List<Door> doors = new List<Door>();
	private bool isRiding = false;
	public float proxThresh;
	public float buttonPressTime = 0.0f;
	public float buttonTimeout = 0.4f;
	
	private bool nozzleBool = false;
	
	// Use this for initialization
	void Start () {
		gameController = GameObject.Find("_GameController").GetComponent<GameController>();
		player1 = GameObject.Find("Player1").GetComponent<PlayerController>();
		player2 = GameObject.Find("Player2").GetComponent<PlayerController>();
		if (GameObject.Find ("Dispenser") != null) {
						nozzle = GameObject.Find ("Dispenser").GetComponent<Nozzle> ();
				}

		panels = GameObject.FindGameObjectsWithTag("panel");
		GameObject[] doorsArray = GameObject.FindGameObjectsWithTag("door");
		for (int i=0; i<doorsArray.Length; i++) {
			//if (doorsArray[i] == ) {
			doors.Add(doorsArray[i].GetComponent<Door>());
			//}
		}
	}
	
	// Update is called once per frame
	void Update () {
		//if (gameController.getIdSelect() == this.GetComponent<PlayerController>().id) {
		//Debug.Log(Input.GetAxisRaw("RightTrigger"));
		if ((Input.GetAxisRaw("RightTrigger") < -0.8f || Input.GetButtonDown("Ability2")) && Time.time > buttonPressTime + buttonTimeout) {// ||  Input.GetButtonDown("PlayerAbility")) {
			buttonPressTime = Time.time;
			foreach(GameObject panel in panels) {
					if ((panel.transform.position - transform.position).magnitude < proxThresh) {
						if (panel.name == "Panel_Nozzle") {
							nozzleBool = !nozzleBool;
							if (nozzleBool) {
								gameController.camMode = CameraController.CameraMode.Nozzle;
								gameController.camShotIndex = 0;
								player2.enabled = false;
								nozzle.enabled = true;
								Debug.Log(gameController.camController.GetCamMode());
							} else {
								gameController.camMode = CameraController.CameraMode.Coop;
								player2.enabled = true;
								nozzle.enabled = false;
							}
						} else {
							Debug.Log ("yes");
							foreach (Door door in doors) {
								for (int i=0; i<door.panels.Length; i++) {
									if (door.panels[i]  == panel) {
										door.SetState(!door.state);
									}
								}
							}
						}
					}
				}
			}
		//}
	}
}
