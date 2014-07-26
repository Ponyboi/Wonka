using UnityEngine;
using System.Collections;

public class Chase : MonoBehaviour {

	//target to follow, move speed, look speed
	public GameObject target;
	public float speed;
	public float lookSpeed;

	//distance to target and magnitude of distance
	private Vector3 dist;
	private float magDist;

	//target's current and prev location
	//the difference in the locations, and 
	//speed determined by that information
	private Transform curLoc;
	private Vector3 pastLoc;
	private Vector3 tarDif;
	private float tarSpd;
	//where to move torward
	private Vector3 targetLoc;


	void Start () {
		curLoc = target.transform;
		pastLoc = target.transform.position;
	}

	void Update () {
		curLoc = target.transform;
		//print (curLoc.position);
		print (pastLoc);
		tarDif = curLoc.position - pastLoc;
		tarSpd = tarDif.magnitude /Time.deltaTime;
		dist = curLoc.position - transform.position;
		if (tarSpd == 0) {
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation
			                                      (curLoc.position - transform.position),lookSpeed*Time.deltaTime);
				}
		else{
			//time taken to reach target
			float difTime = dist.magnitude/speed;
			//where the target would be after that amount of time
			targetLoc = curLoc.position + (curLoc.forward * (difTime*tarSpd));
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation
			                                      (targetLoc - transform.position),lookSpeed*Time.deltaTime);
		}
		transform.position += transform.forward * speed * Time.deltaTime;
		pastLoc = curLoc.position;
	}
}
