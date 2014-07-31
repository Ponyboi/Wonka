using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public float radius;
	public SpawnType spawnerType;
	public float count;
	public Transform type;
	public bool timerOn;
	public float timerInterval;
	private float timeCount;
	public List<Transform> typeList;

	public enum SpawnType {Box, Circle};

	// Use this for initialization
	void Start () {
		SpawnNow();
		timeCount = timerInterval;
	}
	
	// Update is called once per frame
	void Update () {
		float currentTime = Time.time;
		if (timerOn) {
			if (currentTime > timeCount){
				SpawnNow();
				timeCount = Time.time + timerInterval;
				Debug.Log ("Time: " + Time.time + " timeCount: " + timeCount);
			}
		}
	}

	void SpawnNow() {
		for (int i=0; i<count; i++) {
			Transform item;
			int typeIndex = Random.Range(0, typeList.Count);
			if (spawnerType == SpawnType.Box) {
				float x = Random.Range(-radius, radius);
				float z = Random.Range(-radius, radius);
				Vector3 genPosition = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
				item = (Transform)Instantiate(typeList[typeIndex]);
				item.transform.position = genPosition;
				//item.transform.parent = GameObject.Find("_Spawned").transform;
			} else {
				float x = Random.Range(-radius, radius);
				float z = Random.Range(-radius, radius);
				Vector3 genPosition = new Vector3(x, transform.position.y, z);
				genPosition = Vector3.ClampMagnitude(genPosition, radius);
				item = (Transform)Instantiate(typeList[typeIndex]);
				item.transform.position = genPosition;
				//item.transform.parent = GameObject.Find("_Spawned").transform;
			}
		}
	}
}
