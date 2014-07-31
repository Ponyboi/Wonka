using UnityEngine;
using System.Collections;

public class TextureScaling : MonoBehaviour {
	public float scaleX;
	public float scaleZ;
	public float scaleAll = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		scaleX = transform.localScale.z * scaleAll;
		scaleZ = transform.localScale.x * scaleAll;
		//renderer.material.SetTextureScale ("_MainTex", new Vector2(1/scaleX, 1/scaleZ));

		renderer.material.SetTextureScale ("_BumpMap", new Vector2(1/scaleX, 1/scaleZ));
		renderer.material.SetTextureScale ("_BumpMap2", new Vector2(1/scaleX, 1/scaleZ));
		renderer.material.SetTextureScale ("_FlowMap", new Vector2(1/scaleX, 1/scaleZ));
		renderer.material.SetTextureScale ("_NoiseMap", new Vector2(1/scaleX, 1/scaleZ));
		renderer.material.SetTextureScale ("_Cube", new Vector2(1/scaleZ, 1/scaleX));
	}
}
