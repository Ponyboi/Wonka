/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lava : MonoBehaviour {

	public List<PlayerController> players;
	public GameObject resetPos;
	public GameObject riverJump;
	public float sinkTime = 1.2f;
	public float sinkDist = 2f;
	public float sinkPos = 0;
	public float jumpStruggle = 0.1f;
	private float vel;
	public float enterTime = 0.0f;
	public float escapeTime = 0.0f;
	public float escapeTimeout = 0.3f;
	public float struggleTime = 3;
	private float initY;
	public float initYSink = -1.5f;
	public float struggleTimeRise = 0.2f;

	// Use this for initialization
	void Awake () {
		resetPos = Nozzle.TraverseHierarchy(transform, "ResetPos").gameObject;
		riverJump = Nozzle.TraverseHierarchy(transform, "RiverJump").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		PlayerController playerRemove = null;
		foreach(PlayerController player in players){
			if (player != null) {
				sinkPos = Mathf.SmoothDamp(sinkPos, -sinkDist, ref vel, sinkTime);
				if (Input.GetButtonDown("Jump")) {
					//sinkPos += jumpStruggle;
					struggleTimeRise = Time.time + 0.03f;
				}
				if (Time.time < struggleTimeRise) {
					sinkPos = Mathf.SmoothDamp(sinkPos, sinkDist+0.5f, ref vel, sinkTime);
				}
				player.transform.position = new Vector3(player.transform.position.x, initY + sinkPos, player.transform.position.z);
				if (sinkPos + sinkDist < 0.1f || Time.time > enterTime + struggleTime){
					player.transform.position = resetPos.transform.position;
				}
			} else {
				sinkPos = Mathf.SmoothDamp(sinkPos, 0, ref vel, sinkTime);
			}
//			if (player.transform.position.y > initY - 0.1f){
//				playerRemove = player;
//				player.isDrowning = false;
//				player.isGravity = true;
//				player.speedMod = 1f;
//				escapeTime = Time.time;
//				riverJump.SetActive(true);
//			}
		}
		if (players.Count == 0) {
			sinkPos = 0;
		}
		players.Remove(playerRemove);
	}

	void OnTriggerEnter(Collider col) {
//		if (col.gameObject.tag == "Player") {
//			col.transform.position = resetPos.transform.position;
//		}

		if (col.gameObject.tag == "Player") {
			PlayerController player = col.GetComponent<PlayerController>();
			players.Add(player);
			//if (Time.time > escapeTime + escapeTimeout){
				player.isDrowning = true;
				player.isGravity = false;
				player.speedMod = 0.1f;
				player.verticalSpeed = 0;
				initY = player.transform.position.y + initYSink;
				enterTime = Time.time;
			//}
		}


	}
//	void OnTriggerStay(Collider col){
//		if (col.gameObject.tag == "Player") {
//			PlayerController player = col.GetComponent<PlayerController>();
//
//			if (Time.time > escapeTime + escapeTimeout){
//				players.Add(player);
////				player.isDrowning = true;
////				player.isGravity = false;
////				player.speedMod = 0.1f;
////				player.verticalSpeed = 0;
////				initY = player.transform.position.y + initYSink;
////				enterTime = Time.time;
//			}
//		}
//	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player") {
			PlayerController player = col.GetComponent<PlayerController>();
			players.Remove(player);
			player.isDrowning = false;
			player.isGravity = true;
			player.speedMod = 1f;
		}
	}
}
