/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script made by Aidan
**/

using UnityEngine;
using System.Collections;

public class TaffyCamera : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;



	private int camMode;
	
	// Use this for initialization
	void Start () {
		camMode = 1;

	}
	
	// Update is called once per frame
	void Update () {
		float xVal,yVal,zVal;
		Vector3 aveVect = (player1.transform.position+player2.transform.position)/2;
		switch(camMode){
		case 1:
			xVal = Mathf.Min(player1.transform.position.x,player2.transform.position.x)-10;
			yVal = aveVect.y+10;
			zVal = aveVect.z;
			transform.LookAt(aveVect);
			transform.position = new Vector3(xVal,yVal,zVal);
			if(player1.transform.position.x>-65&&player2.transform.position.x>-65){
				//camMode=2;
				//player1.GetComponent<TaffyPlayer>().switchCont();
				//player2.GetComponent<TaffyPlayer>().switchCont();
			}
			break;
		case 2:
			xVal = Mathf.Min(player1.transform.position.x,player2.transform.position.x);
			zVal = aveVect.z-10;
			yVal=aveVect.y+7;
			transform.LookAt(aveVect);
			transform.position= new Vector3(xVal,yVal,zVal);
			break;
		}
	}
	
	public void SetCamMode(int mode) {
		camMode = mode;
	}
	
	public int GetCamMode() {
		return camMode;
	}
	
}
