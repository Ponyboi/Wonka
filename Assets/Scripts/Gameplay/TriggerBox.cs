using UnityEngine;
using System.Collections;

public class TriggerBox : MonoBehaviour {
	public GameController gameController;

	public CameraController.CameraMode camMode = CameraController.CameraMode.Coop;
	public int camShotIndex;
	public bool isCameraTrigger = false;

	public int levelIndex;
	public bool isLevelSwitcher = false;

	public bool destroyOnTrigger;

	// Use this for initialization
	void Start () {
		gameController = GameObject.Find("_GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag =="Player" || col.gameObject.layer == 12) {
			if (isCameraTrigger) {
				gameController.camMode = camMode;
				gameController.camShotIndex = camShotIndex;
			}
			if (isLevelSwitcher) {
				Application.LoadLevel(levelIndex);
			}

			if (destroyOnTrigger) {
				Destroy(this.gameObject);
			}
		}

	}
}
