/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class Y_Camera : MonoBehaviour {

	public Transform target;
	public float distance;
	public float heightDistance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(target.position.x, target.position.y + heightDistance, target.position.z - distance);
		transform.eulerAngles = new Vector3(transform.rotation.x + 38.0f, transform.rotation.y, transform.rotation.z);

	}


}
