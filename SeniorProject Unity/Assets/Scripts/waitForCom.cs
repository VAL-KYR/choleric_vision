using UnityEngine;
using System.Collections;

public class waitForCom : MonoBehaviour {

	public GameObject HBStartRun;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (HBDataScript.comNum != null)
		{
			if (!HBStartRun.GetComponent<startHBCal>().readyCom)
			{
				HBStartRun.GetComponent<startHBCal>().readyCom = true;
			}
		}
	}
}
