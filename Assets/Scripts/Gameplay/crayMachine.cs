/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class crayMachine : MonoBehaviour {

	private GameObject accordion;
	private float accScale;
	private GameObject gear;
	private GameObject lever;
	public float rotateSpd;

	private Transform pole;
	private Vector3 poleStart;
	private Vector3 poleEnd;
	private float poleSpeed;
	private Vector3 goal;

	private float nAccSpeed;
	private float curSpeed;
	private float nRotateSpd;
	private float curRot;

	public float accSpeed;
	public float ballSpeed;

	// Use this for initialization
	void Start () {
		lever = transform.FindChild ("lever").gameObject;
		accordion = transform.FindChild ("accordion").gameObject;
		gear = transform.FindChild ("gear").gameObject;
		accScale = accordion.transform.localScale.y;

		nAccSpeed = -accSpeed;
		nRotateSpd = -rotateSpd;
		curSpeed = accSpeed;
		curRot = rotateSpd;


		pole = transform.FindChild ("pole");
		poleStart = pole.position;
		poleEnd = pole.up *1.2f + poleStart;
		poleSpeed = 4;
		goal = poleEnd;
	}
	
	// Update is called once per frame
	void Update () {
		gear.transform.Rotate (Vector3.up * Time.deltaTime*50);
		lever.transform.Rotate (Vector3.right * Time.deltaTime * curRot);
		accordion.transform.localScale += new Vector3(0f,curSpeed*Time.deltaTime,0f);
		float nowScale = accordion.transform.localScale.y;
		if(nowScale<=accScale*.5){ 
			curSpeed=nAccSpeed;
			curRot = nRotateSpd;
		}
		if(nowScale>=accScale){
			curSpeed=accSpeed;
			curRot = rotateSpd;
		}

		pole.position = Vector3.Lerp (pole.position, goal, Time.deltaTime*poleSpeed);
		if(pole.position ==goal){
			if(goal==poleEnd)
				goal=poleStart;
			else
				goal=poleEnd;
		}


	}
}
