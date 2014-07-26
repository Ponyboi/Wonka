
/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public bool state = false;
	public float speed = 0.2f;
	public Vector3 openPos;
	public Vector3 closePos;

	// Use this for initialization
	void Start () {
		foreach  (Transform pos in transform) {
			if (pos.name == "openPos") {
				openPos = pos.position;// + transform.position;
			}
			if (pos.name == "closePos") {
				closePos = pos.position;// + transform.position;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (state) {
			transform.position = Vector3.MoveTowards(transform.position, openPos, speed);
		} else {
			transform.position = Vector3.MoveTowards(transform.position, closePos, speed);
		}
	}

	public void SetState(bool state) {
		if (state) {
			this.state = state;
		} else {
			this.state = state;
		}
	}
}
