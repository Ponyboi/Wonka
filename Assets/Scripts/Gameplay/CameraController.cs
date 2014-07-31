/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {
	
	public PlayerController[] players;
	public PlayerController player1;
	public PlayerController player2;
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
	
	//Camera Ghosting
	public bool geometryGhosting = false;
	public List<GameObject> ghostList;
	public List<GameObject> ghostOut;
	public LayerMask levelGeoMask;
	public float opacity = 0.5f;
	private float rayDist = 1000.0f;
	public float coolDown = 1.0f;
	public float warmUp = 0.5f;
	public float yOffset = 2;

	//Cameras
	public GameObject[] cameras; 
	public enum CameraMode {Solo, Coop, Nozzle, Intro, Fixed};
	public CameraMode camMode = CameraMode.Coop;
	public int playerFocus = 1;
	public int camIndex = 0;
	
	// Use this for initialization
	void Start () {
		playerRotation = new GameObject("_rotation");
		cameraRotation = new GameObject("_CamRotation");
		players = GameObject.FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
		foreach(PlayerController player in players) {
			if (player.id == 1)
				player1 = player;
			if (player.id == 2)
				player2 = player;
		}
		
		ghostList = new List<GameObject>();
		ghostOut = new List<GameObject>();
		
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
			cameraTilt = -2;
			Vector3 camPos = new Vector3(player1.transform.position.x - distanceAway, player1.transform.position.y + distanceUp*1.3f, player1.transform.position.z/3);
			cameras[camIndex].transform.position = Vector3.Lerp(cameras[camIndex].transform.position, camPos, Time.deltaTime * smooth);
			
			cameras[camIndex].transform.rotation = Quaternion.LookRotation(Vector3.right*5 + Vector3.up*cameraTilt); //players[1].transform.position + Vector3.right*4
			break;
		case CameraMode.Solo:
			SurroundCamera(playerFocus);
			break;
		case CameraMode.Coop:
			SurroundCamera(-1);
			break;
		case CameraMode.Intro:
			if (!introTrack.GetIsFinished()) {
				introTrack.setMoving(true);
			}
			break;
			
		case CameraMode.Fixed:
			Vector3 playerFixed = (players[0].transform.position + players[1].transform.position)/2;
			Vector3 relativePosFixed = playerFixed - cameras[camIndex].transform.position;
			Quaternion rotationFixed = Quaternion.LookRotation(relativePosFixed + (Vector3.up * cameraTilt));
			cameras[camIndex].transform.rotation = rotationFixed;
			break;
		}
		
		if (cameras[camIndex].camera.enabled != enabled) {
			Debug.Log("select cam");
			SelectCamera(camIndex);
		}
		
		if (geometryGhosting) {
			GeometryGhosting();
		}
		
	}
	public void SurroundCamera(int id){
		Vector3 player = new Vector3(0,0,0);
		if (id == -1){
			player = (player1.transform.position + player2.transform.position)/2;
		} else if(id == 1) {
			player = player1.transform.position;
		} else {
			player = player2.transform.position;
		}
		float distMod = (player1.transform.position - player2.transform.position).magnitude;
		offset = player + 
			playerRotation.transform.up * (distanceUp + (distMod/4)) - 
				playerRotation.transform.forward * (distanceAway + (distMod/2));
		
		playerRotation.transform.position = player;
		cameraRotation.transform.position = offset;
		cameraRotation.transform.parent = playerRotation.transform;
		
		if (0 < Mathf.Abs(Input.GetAxisRaw("RightAnalog_H"))) {
			//angle += Input.GetAxisRaw("RightAnalog_H") * rotateSpeed * 0.3f;
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
	
	public void GeometryGhosting() {
		List<GameObject> raycastGeo = new List<GameObject>();
		RaycastHit player1Hit = new RaycastHit();
		RaycastHit player2Hit = new RaycastHit();
		
		Debug.DrawRay(cameras[camIndex].transform.position, (player1.transform.position + Vector3.up * yOffset) - cameras[camIndex].transform.position, Color.red);
		Debug.DrawRay(cameras[camIndex].transform.position, (player2.transform.position + Vector3.up * yOffset) - cameras[camIndex].transform.position, Color.red);
		//	Physics.Raycast(cameras[camIndex].transform.position, player2.transform.position - cameras[camIndex].transform.position, out player2Hit, rayDist, levelGeoMask);

		//Vector3 player1NewHeight = new Vector3(player1.transform.position.x, player1.transform.position.y + yOffset, player1.transform.position.z)
		if (Physics.Raycast(cameras[camIndex].transform.position, (player1.transform.position + Vector3.up * yOffset) - cameras[camIndex].transform.position, out player1Hit, rayDist, levelGeoMask)){
			if (player1Hit.transform.gameObject.tag != "Player"){
				if (!player1Hit.transform.gameObject.GetComponent<GhostHelper>())
					player1Hit.transform.gameObject.AddComponent<GhostHelper>();
				raycastGeo.Add(player1Hit.transform.gameObject);
			}
		}	
		if (Physics.Raycast(cameras[camIndex].transform.position, (player2.transform.position + Vector3.up * yOffset) - cameras[camIndex].transform.position, out player2Hit, rayDist, levelGeoMask)){
			if (player2Hit.transform.gameObject.tag != "Player"){
				if (player2Hit.transform.gameObject.GetComponent<GhostHelper>() == null)
					player2Hit.transform.gameObject.AddComponent<GhostHelper>();
				raycastGeo.Add(player2Hit.transform.gameObject);
			}
		}
		List<GameObject> tempList = new List<GameObject>();
		foreach(GameObject obj in ghostList) {
			if(!raycastGeo.Contains(obj)){
				tempList.Add(obj);
				ghostOut.Add(obj);
			}
		}
		//clean ghostList
		foreach(GameObject obj in tempList) {
			if (ghostList.Contains(obj)) {
				ghostList.Remove(obj);
			}
		}
		ghostList = raycastGeo;
		//ghost active geometry
		foreach(GameObject obj in ghostList) {
			GhostHelper objHelper = obj.GetComponent<GhostHelper>();
			objHelper.alphaVal = Mathf.SmoothDamp(objHelper.alphaVal, opacity, ref objHelper.velocity, warmUp);
		}
		//Fade in old geometry
		tempList.Clear();
		foreach(GameObject obj in ghostOut) {
			GhostHelper objHelper = obj.GetComponent<GhostHelper>();
			objHelper.alphaVal = Mathf.SmoothDamp(objHelper.alphaVal, 1, ref objHelper.velocity, coolDown);
			//Debug.Log (obj.name);
			if (obj.GetComponent<GhostHelper>().alphaVal > 0.99f) {
				tempList.Add(obj);
			}
		}
		foreach(GameObject obj in tempList) {
			if (ghostOut.Contains(obj)) {
				ghostOut.Remove(obj);
				//Destroy(obj.GetComponent<GhostHelper>());
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
