using UnityEngine;
using System.Collections;

public class triggerRemote : MonoBehaviour {

	public string triggerTag;
	public GameObject activateObject;
    public bool triggered = false;

	void OnTriggerEnter()
	{
		var go = GameObject.FindWithTag(triggerTag);
		if (go.GetComponent<soundTrigger>())
		{
			go.GetComponent<soundTrigger>().arm();
		}

        // activate the activate object
        if (!activateObject.activeSelf && !triggered)
        {
            activateObject.SetActive(true);
            //Debug.Log(activateObject.name + " object is " + activateObject.activeSelf);
            triggered = true;
        }	

	}
}
