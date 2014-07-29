using UnityEngine;
using System.Collections;

public class EnemyGrab : MonoBehaviour {
	public PlayerController player;
	public EnemySight enemySight;
	public bool isGrabbing;
	public float grabDist = 2.0f;
	public GameObject dumpPos;

	// Use this for initialization
	void Start () {

		if (GameObject.FindWithTag(Tags.player) != null) {
			player = GameObject.FindWithTag(Tags.player).GetComponent<PlayerController>();
		}

		PlayerController[] players = GameObject.FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
		foreach(PlayerController player in players) {
			if (player.id == 1)
				this.player = player;
		}

		enemySight = Nozzle.TraverseHierarchy(transform, "Head").GetComponent<EnemySight>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((player.transform.position - transform.position).magnitude < grabDist && 
		    (dumpPos.transform.position - transform.position).magnitude > grabDist) {
			if (player.isGrabbed == false) {
				player.isGrabbed = true;
				player.transform.parent = transform;
				enemySight.CoolDown(15);
				isGrabbing = true;
			}
		} else if ((dumpPos.transform.position - transform.position).magnitude < grabDist) {
			if (isGrabbing == true) {
				player.isGrabbed = false;
				player.transform.parent = null;
				enemySight.CoolDown(10);
			}
			isGrabbing = false;
		} else {
			isGrabbing =  false;
		}
	}
}
