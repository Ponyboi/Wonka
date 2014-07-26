/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by me
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public CameraController camController;
	public CameraController.CameraMode camMode;
	public int camShotIndex;
	public float cameraZoom;
	public GameObject[] players;
	public int id;
	public int idSelect = 1;
	public Transform pointerModel;
	private Transform pointer;

	//Camera Mode Enum

	// Use this for initialization
	void Start () {
		pointer = Instantiate(pointerModel) as Transform;
		camController = GameObject.Find("_CameraController").GetComponent<CameraController>();
		Physics.IgnoreLayerCollision(10, 11); //wall and dispenser

		//Vanish Tools
		GameObject[] layerObjects = FindGameObjectsWithLayer(Tags.vanishLayer);
		foreach (GameObject obj in layerObjects) {
			obj.renderer.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q) ||  Input.GetButtonDown("PlayerSwitch")) {
			if (idSelect == 1) {
				idSelect = 2;
			} else {
				idSelect = 1;
			}
		}
		if (idSelect == 1) {
			pointer.transform.position = players[idSelect].transform.position + (Vector3.up * 2.5f);
		} else {
			pointer.transform.position = players[idSelect].transform.position + Vector3.up * 2;
		}

		if (camMode != camController.GetCamMode()) {
			camController.SetCamMode(camMode);
		}
		if (camShotIndex != camController.GetCamIndex()) {
			camController.SetCamIndex(camShotIndex);
		}
	}

	public int getIdSelect() {
		return idSelect;
	}

	public GameObject[] FindGameObjectsWithLayer (int layer) {
		GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		List<GameObject> goList = new List<GameObject>();
		for (int i = 0; i < goArray.Length; i++) {
			if (goArray[i].layer == layer) {
				goList.Add(goArray[i]);
			}
		}
		if (goList.Count == 0) {
			return null;
		}
		return goList.ToArray();
	}
	
}
