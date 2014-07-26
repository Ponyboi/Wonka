using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour {
	public Vector3 resetPosition;
	public Vector3 position;

	// Use this for initialization
	void Start () {
		resetPosition = new Vector3(0,0,0);
		position = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
