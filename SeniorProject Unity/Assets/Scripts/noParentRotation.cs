using UnityEngine;
using System.Collections;

public class noParentRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// make child not rotate from this parent
		Quaternion rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
		gameObject.transform.rotation = rotation;
		//
	}
}
