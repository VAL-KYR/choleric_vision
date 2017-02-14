using UnityEngine;
using System.Collections;

public class triggerLookAt : MonoBehaviour {

	public GameObject rootObject;

	// Use this for initialization
	public void lookTrigger() {
		if (rootObject.CompareTag("door"))
			rootObject.GetComponent<doorMaster>().lookingAtMe();
		else if (rootObject.CompareTag("documentCabinet"))
			rootObject.GetComponent<documentCabinet>().lookingAtMe();
	}

}
