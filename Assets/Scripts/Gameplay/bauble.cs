using UnityEngine;
using System.Collections;

public class bauble : MonoBehaviour {
	private Vector3 direction;
	private float speed;

	private Vector3 normal;
	// Use this for initialization
	void Start () {
		direction = new Vector3(Random.Range (-1.0f, 1.0f),Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f));
		speed = transform.parent.GetComponent<crayMachine> ().ballSpeed;

	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (direction * speed * Time.deltaTime);

	}
	void OnCollisionEnter(Collision col){
		Vector3 normal = col.contacts [0].normal;
		direction = Vector3.Reflect (direction, normal);
	}


}
