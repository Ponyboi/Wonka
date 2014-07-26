/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class ActivateCollider : MonoBehaviour {

	public SwitchCamera camera;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find("Y_Cameras").GetComponent<SwitchCamera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(camera.currentCamera != 2){
			gameObject.collider.enabled = false;
		}
		else{
			gameObject.collider.enabled = true;

		}
	}
}
