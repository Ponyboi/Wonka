/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {

	public GameObject[] cameras;
	public GameObject currentCam;
	public int currentCamera = 0;

	// Use this for initialization
	void Start () {
		currentCam = GameObject.FindGameObjectWithTag("MainCamera");
		//changeCamera(0);
	}
	
	// Update is called once per frame
	void Update () {
		changeCamera(currentCamera);
		currentCam = cameras[currentCamera].gameObject;
	}

	public void changeCamera(int current){
		for(int i = 0; i < cameras.Length; i++){
			if(i != current){
				cameras[i].camera.active = false;
			}
			else if(i == current){
				cameras[i].camera.active = true;
			}

		}

	}





}
