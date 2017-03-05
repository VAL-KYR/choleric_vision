using UnityEngine;
using System.Collections;

public class bodyBagScare : MonoBehaviour {

	public bool flagsTripped;
	public GameObject[] flags;
	public GameObject activate;
	public int flagsReady;

	// Use this for initialization
	void Start () {
		flagsTripped = false;
		flagsReady = 0;
	}
	
	// Update is called once per frame
	void Update () {

		foreach (GameObject g in flags) 
		{
			if (g.GetComponent<playerZoneFlag>().playerFlag)
			{
				flagsReady++;
			}
		}

	}

	public void OnTriggerEnter(Collider c)
	{
		if (c.CompareTag("GameController") && flagsReady >= flags.Length - 1)
		{
			if (!activate.activeSelf)
			{
				activate.SetActive(true);
			}

		}
	}
}
