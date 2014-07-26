using UnityEngine;
using System.Collections;

public class WonkaBar : MonoBehaviour {

	public float rotSpeed;
	public float bobSpeed;
	public float bobAmp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float pos = Mathf.Sin (Time.time * bobSpeed) * bobAmp;
		Quaternion rot = Quaternion.AngleAxis((Time.time * rotSpeed) , Vector3.up);
		transform.position = new Vector3(transform.position.x, transform.position.y + pos, transform.position.z);
		transform.rotation = transform.rotation * rot;
	}
}
