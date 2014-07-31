using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {

	private float playersIn;

	// Use this for initialization
	void Start () {
		playersIn = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (playersIn >= 2) {
			Application.LoadLevel("FactoryEntrance");
				}
	
	}
	void OnTriggerEnter(Collider col){
		if (col.tag == "Player") {
			playersIn+=1;
				}
		}
	void OnTriggerExit(Collider col){
		if (col.tag == "Player") {
			playersIn-=1;
		}
	}
}
