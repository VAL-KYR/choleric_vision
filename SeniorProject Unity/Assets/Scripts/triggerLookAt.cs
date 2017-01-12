using UnityEngine;
using System.Collections;

public class triggerLookAt : MonoBehaviour {

	public GameObject rootObject;

	// Use this for initialization
	public void lookTrigger(float lookAtDist) {
		if(rootObject.CompareTag("door"))
			rootObject.GetComponent<doorAnimator>().lookingAtMe(lookAtDist);
		else if (rootObject.CompareTag("documentCabinet"))
			rootObject.GetComponent<documentCabinet>().lookingAtMe(lookAtDist);
	}

}
