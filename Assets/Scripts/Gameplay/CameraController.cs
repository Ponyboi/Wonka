/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public PlayerController[] players;
	public CurrentBezier introTrack; //temp as these controls should be in this script
	private Vector3 offset;
	public float distanceAway;
	public float distanceUp;
	public float cameraTilt;
	public float cameraGrowth;
	public float smooth;
	public float rotateSpeed;
	private Vector3 upVector;
	private Vector3 forwardVector;
	private float angle;
	GameObject playerRotation;
	GameObject cameraRotation;

	public GameObject[] cameras; 

	public enum CameraMode {Coop, Nozzle, Intro, Fixed};
	public CameraMode camMode = CameraMode.Coop;
	public int camIndex = 0;

	// Use this for initialization
	void Start () {
		playerRotation = new GameObject("_rotation");
		cameraRotation = new GameObject("_CamRotation");
//		upVector = player.transform.up;
//		forwardVector  = player.transform.forward;
//		offset = player.transform.position + 
//			upVector * (distanceUp + player.GetSize()) - 
//			forwardVector * (distanceAway  + player.GetSize());
	}
	
	// Update is called once per frame
	void Update () {
		switch(camMode){
		case CameraMode.Nozzle:
			Vector3 camPos = new Vector3(players[1].transform.position.x - distanceAway, players[1].transform.position.y + distanceUp*2.1f, 0);
			cameras[camIndex].transform.position = Vector3.Lerp(cameras[camIndex].transform.position, camPos, Time.deltaTime * smooth);

			cameras[camIndex].transform.rotation = Quaternion.LookRotation(Vector3.right*5 + Vector3.down*4); //players[1].transform.position + Vector3.right*4
			break;
		case CameraMode.Coop:
			Vector3 player = (players[1].transform.position + players[2].transform.position)/2;
			offset = player + 
				playerRotation.transform.up * (distanceUp) - 
					playerRotation.transform.forward * (distanceAway);

			playerRotation.transform.position = player;
			cameraRotation.transform.position = offset;
			cameraRotation.transform.parent = playerRotation.transform;

			if (0 < Mathf.Abs(Input.GetAxisRaw("RightAnalog_H"))) {
				angle += Input.GetAxisRaw("RightAnalog_H") * rotateSpeed * 0.3f;
			} else {
				angle += Input.GetAxis("Mouse X") * rotateSpeed;
			}
				
			playerRotation.transform.rotation = Quaternion.AngleAxis(angle , playerRotation.transform.up);
			//Debug.DrawRay(playerRotation.transform.position, playerRotation.transform.forward * 10, Color.blue);

			cameras[camIndex].transform.position = Vector3.Lerp(cameras[camIndex].transform.position, cameraRotation.transform.position, Time.deltaTime * smooth);

			Vector3 velocity = new Vector3(0.8f, 0.8f, 0.8f);//Vector3.one;
			
			//transform.position = Vector3.SmoothDamp(transform.position, cameraRotation.transform.position, ref velocity, smooth);

			Vector3 relativePos = player - cameras[camIndex].transform.position;
			Quaternion rotation = Quaternion.LookRotation(relativePos + (Vector3.up * cameraTilt));
			cameras[camIndex].transform.rotation = rotation;
	//		transform.position = offset + player.transform.position;

			break;
		case CameraMode.Intro:
			if (!introTrack.GetIsFinished()) {
				introTrack.setMoving(true);
			}
			break;

		case CameraMode.Fixed:
			Vector3 playerFixed = (players[1].transform.position + players[2].transform.position)/2;
			Vector3 relativePosFixed = playerFixed - cameras[camIndex].transform.position;
			Quaternion rotationFixed = Quaternion.LookRotation(relativePosFixed + (Vector3.up * cameraTilt));
			cameras[camIndex].transform.rotation = rotationFixed;
			break;
		}

		if (cameras[camIndex].camera.enabled != enabled) {
//			Debug.Log("select cam");
			SelectCamera(camIndex);
		}
	}

	public void SelectCamera (int index) {
		for (int i=0; i<cameras.Length; i++) {
			// Activate the selected camera
			if (i == index){
				cameras[i].camera.enabled = true;
				cameras[i].GetComponent<AudioListener>().enabled = true;
				// Deactivate all other cameras
			}else{
				cameras[i].camera.enabled = false;
				cameras[i].GetComponent<AudioListener>().enabled = false;
			}
			
		}
	}

	public void SetCamMode(CameraMode mode) {
		camMode = mode;
	}

	public CameraMode GetCamMode() {
		return camMode;
	}

	public void SetCamIndex(int index) {
			camIndex = index;
	}
	
	public int GetCamIndex() {
		return camIndex;
	}

	public GameObject GetCurrentCamera() {
		return cameras[camIndex].gameObject;
	}
}
