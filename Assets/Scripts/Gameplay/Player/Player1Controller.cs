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
	private GameController gameController;

	// Use this for initialization
	void Start () {
		player2 = GameObject.Find("Player2").GetComponent<PlayerController>();
		gameController = GameObject.Find("_GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	if (gameController.getIdSelect() == this.GetComponent<PlayerController>().id) {
			if (Input.GetKeyDown(KeyCode.E) ||  Input.GetButtonDown("PlayerAbility")) {
				if ((player2.transform.position - transform.position).magnitude < proxThresh) {
					if (isRiding) {
						player2.transform.parent = null;
						player2.GetComponent<CharacterController>().enabled = true;
						isRiding = false;
					} else {
						player2.transform.position = transform.position + transform.up - transform.forward;
						player2.transform.parent = transform;
						player2.GetComponent<CharacterController>().enabled = false;
						isRiding = true;
					}
				}
			}
		}
	}
}
