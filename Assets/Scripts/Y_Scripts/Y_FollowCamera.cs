/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
using UnityEngine;
using System.Collections;

public class Y_FollowCamera : MonoBehaviour {

	public GameObject target;
	Vector3 offset;
	public float turnAngle = 90.0f;

	void Start() {
		offset = target.transform.position - transform.position;
	}
	
	void LateUpdate() {




		float desiredAngle = target.transform.eulerAngles.y + turnAngle;
		Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
		transform.position = target.transform.position - (rotation * offset);
		transform.LookAt(target.transform);
	}


	
	
}
