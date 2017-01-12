using UnityEngine;
using System.Collections;

public class triggerRemote : MonoBehaviour {

	public string triggerTag;

	void OnTriggerEnter()
	{
		var go = GameObject.FindWithTag(triggerTag);
		go.GetComponent<soundTrigger>().arm();
	}
}
