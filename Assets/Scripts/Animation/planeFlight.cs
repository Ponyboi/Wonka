using UnityEngine;
using System.Collections;

public class planeFlight : MonoBehaviour {

	public float rotateSpeed;
	public float speed;
	private Transform theChild;

	// Use this for initialization
	void Start () {
		theChild = transform.GetChild (0);
		theChild.Rotate (Vector3.right * -rotateSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward*Time.deltaTime*speed);
		transform.Rotate (Vector3.up * Time.deltaTime*rotateSpeed);
	}
}
