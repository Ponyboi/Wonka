using UnityEngine;
using System.Collections;

public class CameraNode : MonoBehaviour {

	public GameObject lookPos;
	public float rotationSpeed;
	public float speed;
	public CurrentBezier.CameraTransition camTrans;
	
	void Start () {
		lookPos = Nozzle.TraverseHierarchy(transform, "LookPosition").gameObject;
	}

	void Update () {
		
	}

	public Vector3 GetLookPosition(){
		return lookPos.transform.position;
	}
}
