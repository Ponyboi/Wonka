using UnityEngine;
using System.Collections;

public class LaffyTaffy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
//			TaffyPlayer player = col.gameObject.GetComponent<TaffyPlayer>();
				//player.verticalSpeed = player.GetBounceSpeed();
//				player.verticalSpeed = player.jumpHeight*1.5f;
			//}
		}
		else if(col.gameObject.rigidbody!=null){
			Vector3 curVel = col.rigidbody.velocity;
			col.rigidbody.velocity = new Vector3(curVel.x,15,curVel.z);
		}
	}
	void OnTriggerStay(Collider col){
		if(col.gameObject.tag == "Player"){
//			TaffyPlayer player = col.gameObject.GetComponent<TaffyPlayer>();
			//if(player.IsGrounded()){
//				player.verticalSpeed = player.jumpHeight*1.5f;
			//}
		}
		else if(col.gameObject.rigidbody!=null&& col.rigidbody.velocity.y<0){
			col.rigidbody.AddForce(new Vector3(0,150,0));
		}
	}
}
