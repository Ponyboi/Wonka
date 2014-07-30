using UnityEngine;
using System.Collections;

public class TriggerBox : MonoBehaviour {
	public GameController gameController;

	public bool isCameraTrigger = false;
	public CameraController.CameraMode camMode = CameraController.CameraMode.Coop;
	public int camShotIndex;

	public bool isLevelSwitcher = false;
	public int levelIndex;

	public bool isTransporter;
	public GameObject resetPos;

	public bool destroyOnTrigger;

	// Use this for initialization
	void Start () {
		gameController = GameObject.Find("_GameController").GetComponent<GameController>();
		resetPos = Nozzle.TraverseHierarchy(transform, "ResetPos").gameObject;
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
			if (isTransporter) {
				col.transform.position = resetPos.transform.position;
			}
			if (destroyOnTrigger) {
				Destroy(this.gameObject);
			}
		}

	}
}
