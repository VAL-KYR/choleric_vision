using UnityEngine;
using System.Collections;

public class triggerRemote : MonoBehaviour {

	public string triggerTag;
	public GameObject activateObject;

	void OnTriggerEnter()
	{
		var go = GameObject.FindWithTag(triggerTag);
		if (go.GetComponent<soundTrigger>())
		{
			go.GetComponent<soundTrigger>().arm();
		}

		// activate the activate object
		if (!activateObject.activeSelf)
		{
			activateObject.SetActive(true);
		}
		else 
		{
			activateObject.SetActive(false);
		}

	}
}
