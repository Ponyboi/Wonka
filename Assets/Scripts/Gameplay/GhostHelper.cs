using UnityEngine;
using System.Collections;

public class GhostHelper : MonoBehaviour {
	public float alphaVal = 1.0f;
	public float velocity = 0.0f;
	private Color color = new Color(1,1,1,1);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		color = renderer.material.color;
		color.a = alphaVal;
		renderer.material.color = color;
	}
}
