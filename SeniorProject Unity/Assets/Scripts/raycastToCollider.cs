using UnityEngine;
using System.Collections;

public class raycastToCollider : MonoBehaviour {

	public Camera raycaster;
	public GameObject castObject;

	private Vector3 cameraCenter;
	private RaycastHit hit;
	private GameObject hitObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		cameraCenter = raycaster.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, raycaster.nearClipPlane));

		if (Physics.Raycast(cameraCenter, raycaster.transform.forward, out hit, 1000))
		{
			// the object seen is what the raycast hit
			hitObject = hit.transform.gameObject;

		}

		castObject.transform.position = hit.transform.position;
	
	}
}
