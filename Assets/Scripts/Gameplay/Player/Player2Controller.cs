/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;

public class Player2Controller : MonoBehaviour {
	public GameController gameController;
	public PlayerController player1;
	public PlayerController player2;
	public Nozzle nozzle;
	public GameObject[] panels;
	public Door[] doors;
	private bool isRiding = false;
	public float proxThresh;


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
		if (GameObject.Find ("Doors") != null) {
						doors = GameObject.Find ("Doors").GetComponentsInChildren<Door> ();
				}
	}
	
	// Update is called once per frame
	void Update () {
		if (gameController.getIdSelect() == this.GetComponent<PlayerController>().id) {
			if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("PlayerAbility")) {
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
								door.SetState(true);
							}
						}
					}
				}
			}
		}
	}
}
