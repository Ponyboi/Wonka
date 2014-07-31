/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Nozzle : MonoBehaviour {

	public int id;
	public int idSelect;
	public GameController gameController;
	public GameObject ringHolder;
	public GameObject armTop;
	public GameObject armBottom;
	public List<GameObject> arms = new List<GameObject>();
	public GameObject armPrefab;
	public List<String> names = new List<String>() {"Top", "Bottom"};

	public int initialCandyCount = 4;
	public float candySpacing = 1.5f;
	private float armHeight = 2.0f;
	private float armRadius = 1.75f;
	private int armCount = 6;

	public Transform[] candies;
	private Transform currentCandy;
	private float shootWait = 0.7f;
	private float shootWaitTime = 0.0f;
	private float armWait = 0.5f;
	private float armWaitTime = 0.0f;
	public float armSpeed = 400f;

	private Vector3 moveDirection = Vector3.zero;
	public float speedSmoothing = 10.0f;
	private float speed = 0.3f;
	private float dampening = 30.5f;

	// Use this for initialization
	void Awake() {
		//GenerateArms();
	}

	void Start () {
		gameController = GameObject.Find("_GameController").GetComponent<GameController>();
		ringHolder = GameObject.Find ("RingHolder");

		//arms = new List<GameObject>();
		//GenerateArms();
		TraverseHierarchyAdd(transform, names, arms);

		ClipGeneration();
	}
	
	// Update is called once per frame
	void Update () {

		UpdateSmoothedMovementDircetion();
	
		Vector3 movement= moveDirection;
		//Debug.Log("movement: " + movement);
		movement *= Time.deltaTime;

		transform.position = Vector3.Lerp(transform.position, transform.position + movement, 2.3f);
		if (currentCandy != null) {
			//currentCandy.transform.position = Vector3.Lerp(transform.position, transform.position + movement, 2.3f);
		}

		shootCandy();
	}

	void UpdateSmoothedMovementDircetion() {
		Vector3 forward= Vector3.right;
		forward.y = 0;
		forward = forward.normalized;
		
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right= new Vector3(forward.z, 0, -forward.x);
		
		float v= 0;
		float h= 0;
		//if (gameController.getIdSelect() == id) {
			v = Input.GetAxisRaw("Vertical2");
			h = Input.GetAxisRaw("Horizontal2");
		 if (Mathf.Abs(Input.GetAxisRaw("RightAnalog_V")) > 0.2f || Mathf.Abs(Input.GetAxisRaw("RightAnalog_H")) > 0.2f) {
			v = -Input.GetAxisRaw("RightAnalog_V");
			h = Input.GetAxisRaw("RightAnalog_H");
		}
		
		// Target direction relative to the camera
		Vector3 targetDirection = h * right + v * forward;
		float curSmooth= speedSmoothing * Time.deltaTime;
		
		// Choose target speed
		//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
		float targetSpeed= Mathf.Min(targetDirection.magnitude, 1.0f) * speed; //Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		moveDirection += targetDirection* targetSpeed;
		moveDirection -= moveDirection/dampening;
	}

	public void shootCandy() {
		//if (gameController.getIdSelect() == id) {
		if ((Input.GetMouseButtonDown(0) && shootWaitTime < Time.time) || (Input.GetButtonDown("Jump2") && shootWaitTime < Time.time)) {//(0.2f < Mathf.Abs(Input.GetAxisRaw("RightTrigger")) && shootWaitTime < Time.time)) {
			int candyType = UnityEngine.Random.Range(0,candies.Length);
			Transform candy = (Transform)Instantiate(candies[candyType]);
			currentCandy = candy;
			candy.transform.position = transform.position + Vector3.up*1;//3.5f;
			candy.rigidbody.AddForce(Vector3.up * -2000);
			shootWaitTime = Time.time + shootWait;
			armWaitTime = Time.time + armWait;

		} else if (armWaitTime > Time.time) {
			foreach (GameObject arm in arms) {
				FlexArm(arm, -armSpeed);
			}
			ringHolder.collider.enabled = false;
		} else {
			foreach (GameObject arm in arms) {
				FlexArm(arm, armSpeed);
			}
			ringHolder.collider.enabled = true;
		}
		//}
	}

	public static Transform TraverseHierarchy(Transform root, String name) {
		Transform obj = root;
		foreach (Transform child in root) {
			// Do something with child, then recurse.
			//Debug.Log(child.name);
			if (child.gameObject.name == name) {
				return child;
			} else {
				obj = TraverseHierarchy(child, name);
			}
		}
		return obj;
	}

	public void TraverseHierarchyAdd(Transform root, List<String> names, List<GameObject> armsList) {
		//Transform obj = root;
		foreach (Transform child in root) {
			// Do something with child, then recurse.
			Debug.Log(child.name);
			if (names.Contains(child.gameObject.name)) {
				armsList.Add(child.gameObject);
				Debug.Log("contains");
			} else {
				//obj = 
				TraverseHierarchyAdd(child, names, armsList);
			}
		}
		//return obj;
	}

	public void FlexArm(GameObject arm, float armSpeed) {
		JointMotor motor = arm.hingeJoint.motor;
		motor.targetVelocity = armSpeed;
		arm.hingeJoint.motor = motor;
	}

	public void ClipGeneration() {
		for (int i=0; i<initialCandyCount; i++) {
			int candyType = UnityEngine.Random.Range(0,candies.Length);
			Transform candy = (Transform)Instantiate(candies[candyType]);
			candy.transform.position = transform.position + Vector3.up*(i * candySpacing);
		}
	}
	public void GenerateArms() {
		for (int i=0; i<armCount; i++) {
			GameObject arm = (GameObject)Instantiate(Resources.Load("Prefabs/DispenserArm"));
			float x = Mathf.Cos(Mathf.Deg2Rad*(360/armCount)*i) * armRadius;
			Debug.Log(x);
			float z = Mathf.Sin(Mathf.Deg2Rad*(360/armCount)*i) * armRadius;
			Vector3 pos = new Vector3(transform.position.x + x, transform.position.y - armHeight, transform.position.z + z);
			Quaternion rot = Quaternion.AngleAxis((360/armCount)*i , arm.transform.up);
			arm.transform.position = pos;
			//arm.transform.rotation = arm.transform.rotation * rot;
			arm.transform.parent = this.transform;
		}
	}
}
