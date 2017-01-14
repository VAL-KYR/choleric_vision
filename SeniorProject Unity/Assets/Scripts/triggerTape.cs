using UnityEngine;
using System.Collections;

public class triggerTape : MonoBehaviour {

	public GameObject tapeRecorder;
	public bool triggered = false;

	// Use this for initialization
	void Start () {
		triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter()
	{
		if (!triggered)
		{
			tapeRecorder.GetComponent<tapeRecorder>().remote();
			triggered = true;
		}

	}
}
