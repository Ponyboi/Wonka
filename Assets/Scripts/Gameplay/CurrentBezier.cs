using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurrentBezier : MonoBehaviour {

	private GameController gameController;
	public List<CameraNode> currentNodes;
	public GameObject vessel;
	public List<Bezier> myBezier;
	public GameObject handle1;
	public GameObject handle2;
	private float t = 0f;
	private int segment = 0;
	private Vector3 currentTangent;
	public CameraTransition camTrans = CameraTransition.Linear;

	public bool unifiedSpeed = true;
	private float speedScaler = 1.0f;
	public float speed = 0.005f;
	public float curveSoftness = 1.5f;

	public int particleCount = 5;
//	private List<Transform> particles;
	private GameObject[] particles;
	public float particleSpacing = 0.05f;
	public bool isMoving = false;
	public bool finishedRun = false;
	private bool firstRun = true;

	public enum CameraTransition {Linear, Sine, EaseIn, EaseOut};

	void Awake() {
		gameController = GameObject.Find("_GameController").GetComponent<GameController>();
		ToggleMeshes(transform);
	}

	void Start()
		
	{
		if (currentNodes.Count > 3) {
			currentTangent = findTangentStart(currentNodes[0].transform.position,
			                            currentNodes[1].transform.position,
			                                  currentNodes[2].transform.position, curveSoftness);
		}
		for (int i=0; i<currentNodes.Count-1; i++) {
	//		if (currentNodes[i+1] != null) {
//			Debug.Log("count: " + i);

			Vector3[] tangents = null;
			Vector3 nextTangent;

			if (i < currentNodes.Count - 2) {
				tangents = findTangent(currentNodes[i].transform.position,
				            			currentNodes[i+1].transform.position,
				                       currentNodes[i+2].transform.position, curveSoftness);
				nextTangent = tangents[0];
			} else if (i < currentNodes.Count - 1) {
				nextTangent = findTangentStart(currentNodes[i+1].transform.position,
				                       			currentNodes[i].transform.position,
				                       			currentNodes[i-1].transform.position, curveSoftness);
				//delete this
				tangents = new Vector3[2];
				tangents[0] = new Vector3(0,0,0);
				tangents[1] = new Vector3(0,0,0);
			} else {
				nextTangent = new Vector3(0,0,0);
			}

//				myBezier.Add(new Bezier( currentNodes[i].transform.position, Random.insideUnitSphere * 20f, Random.insideUnitSphere * 2f, currentNodes[i+1].transform.position));
				myBezier.Add(new Bezier(currentNodes[i].transform.position,
			                        	currentTangent,
			                        	nextTangent,
			                        	currentNodes[i+1].transform.position));

				//	}
			if (tangents[1] != null)
				currentTangent = tangents[1];
			else
				currentTangent = new Vector3(0,0,0);
		}

		//particles
		particles = new GameObject[particleCount];
		for (int i=0; i<particleCount; i++) {
//			particles.Add ((Transform) Instantiate(vessel, currentNodes[0].transform.position, Quaternion.identity));
			//particles[i] = ((Transform) Instantiate(vessel, currentNodes[0].transform.position, Quaternion.identity));
			particles[i] = vessel;

		}
		//Debug.Log("particles.Count start: " + particles.Count);
	}
	
	
	
	void Update() {
		if (isMoving) {
			MoveParticles();
		}
	}

	public void MoveParticles()  {
		//Debug.Log("particles.Count: " + particles.Count);
		for (int i=0; i<particles.Length; i++) {
			float tVal = (t + (particleSpacing * i));
			int clamp = (int)Mathf.Floor(tVal);
			int tWrap = (segment + clamp);
			if (tWrap > currentNodes.Count-2)
				tWrap -= currentNodes.Count-1;
			
			
			Vector3 vec = myBezier[tWrap].GetPointAtTime( tVal - clamp);
			//particles[i].transform.rotation = Quaternion.LookRotation(vec - particles[i].transform.position); //look at track
			if (firstRun) {
				particles[i].transform.rotation = Quaternion.LookRotation(currentNodes[segment].GetLookPosition() -  particles[i].transform.position);
				firstRun = false;
			}
			Quaternion rotation = Quaternion.LookRotation(currentNodes[segment].GetLookPosition() -  particles[i].transform.position);
			particles[i].transform.rotation = Quaternion.Lerp(particles[i].transform.rotation, rotation, currentNodes[segment].rotationSpeed);
			particles[i].transform.position = vec;
			speed = currentNodes[segment].speed;
		}
		handle1.transform.position = myBezier[segment].p1 + currentNodes[segment].transform.position;
		handle2.transform.position = myBezier[segment].p2;
		
		float averageLength = AverageLength();
		Vector3 currentSegment = (currentNodes[segment+1].transform.position - currentNodes[segment].transform.position);
		if (unifiedSpeed)
			speedScaler = averageLength/currentSegment.magnitude;
		else
			speedScaler = 1.0f;
		
		t += speed * speedScaler;
		if( t > 1f ) {
			Debug.Log("Segment: " + segment);
			if (segment >= currentNodes.Count-2) {
				segment = 0;
				finishedRun = true;
				Debug.Log("finished Run!");
				isMoving = false;
				gameController.camMode = CameraController.CameraMode.Coop;
			}else {		
				segment++;
			}
			t = 0f;
			
		}
	}

	Vector3[] findTangent(Vector3 previousNode, Vector3 presentNode, Vector3 nextNode, float power) {
		Vector3 line1 = previousNode - presentNode;
		Vector3 line2 = nextNode - presentNode;

		Vector3 midLine = ((line1/2) - (line2/2));
		Vector3 midLine2 = ((line2/2) - (line1/2));
	//	Vector3 prevTangent = ((line1/2) - (midLine/2));
		//Vector3 nextTangent = ((line2/2) - (midLine/2));

		Vector3[] tangentLines = new Vector3[2];
		//tangentLines[0] = prevTangent + presentNode;
		//tangentLines[1] = nextTangent + presentNode;
		float midLineScaler = (line1.magnitude / (line1.magnitude + line2.magnitude));
		float midLine2Scaler = (line2.magnitude / (line1.magnitude + line2.magnitude));
		tangentLines[0] = midLine * midLineScaler * power;
		tangentLines[1] = midLine2 * midLine2Scaler * power;
		return tangentLines;
	}

	Vector3 findTangentStart(Vector3 presentNode, Vector3 nextNode1, Vector3 nextNode2, float power) {
		Vector3 line1 = nextNode1 - presentNode;
		Vector3 line2 = nextNode1 - nextNode2;

		Vector3 nextTangent = ((line1) + (line2 * (line1.magnitude/3)));
		nextTangent = nextTangent.normalized * (line1.magnitude/3) * power;

		return nextTangent;
	}

	float AverageLength() {
		float average = 0.0f;
		for (int i=0; i<currentNodes.Count-2; i++) {
			Vector3 sum = currentNodes[i].transform.position - currentNodes[i+1].transform.position;
			average += sum.magnitude;
		}
		average /= currentNodes.Count-1;
		return average;
	}

	void ToggleMeshes(Transform root) {
		foreach (Transform child in root) {
			// Do something with child, then recurse.
			//Debug.Log(child.name);
			child.renderer.enabled = !child.renderer.enabled;
			ToggleMeshes(child);

		}
	}

	public void setMoving (bool isMoving) {
		this.isMoving = isMoving;
	}

	public bool GetIsFinished() {
		return finishedRun;
	}
	

	/*
	 * 	void Start()
		
	{
		if (currentNodes.Count > 2) {
			currentTangent = findTangent(currentNodes[0].transform.position,
			                            currentNodes[1].transform.position,
			                            currentNodes[2].transform.position, 0.5f, -1);
		}
		for (int i=0; i<currentNodes.Count; i++) {
	//		if (currentNodes[i+1] != null) {
			Debug.Log("count: " + i);

			Vector3 nextTangent;
			if (i < currentNodes.Count-3){
				nextTangent = findTangent(currentNodes[i+1].transform.position,
				                              currentNodes[i+2].transform.position,
				                              currentNodes[i+3].transform.position, 0.5f, 1);
			}else if (i < currentNodes.Count-2){
				nextTangent = findTangent(currentNodes[i+1].transform.position, 
				                              currentNodes[i+2].transform.position,
				                              currentNodes[i+2].transform.position, 0.5f, 1);
			}else if (i < currentNodes.Count-1){
				nextTangent = findTangent(currentNodes[i+1].transform.position, 
				                          currentNodes[i+1].transform.position,
				                          currentNodes[i+1].transform.position, 0.5f, 1);
			}else {
				nextTangent = findTangent(currentNodes[i].transform.position, 
				                          currentNodes[i].transform.position,
				                          currentNodes[i].transform.position, 0.5f, 1);
			}
//				myBezier.Add(new Bezier( currentNodes[i].transform.position, Random.insideUnitSphere * 20f, Random.insideUnitSphere * 2f, currentNodes[i+1].transform.position));
				myBezier.Add(new Bezier(currentNodes[i].transform.position,
			                        	currentTangent,
			                        	nextTangent,
			                        	currentNodes[i+1].transform.position));


			if (i < currentNodes.Count-3){
				currentTangent = findTangent(currentNodes[i+3].transform.position,
				                          currentNodes[i+2].transform.position,
				                          currentNodes[i+1].transform.position, 0.5f, -1);
			}else if (i < currentNodes.Count-2){
				currentTangent = findTangent(currentNodes[i+2].transform.position, 
				                          currentNodes[i+1].transform.position,
				                          currentNodes[i+1].transform.position, 0.5f, -1);
			}else if (i < currentNodes.Count-1){
				currentTangent = findTangent(currentNodes[i+1].transform.position, 
				                          currentNodes[i+1].transform.position,
				                          currentNodes[i+1].transform.position, 0.5f, -1);
			}else {
				currentTangent = findTangent(currentNodes[i].transform.position, 
				                          currentNodes[i].transform.position,
				                          currentNodes[i].transform.position, 0.5f, -1);
			}
				//	}
			//currentTangent = nextTangent;
		}
	}
	*/
	
}