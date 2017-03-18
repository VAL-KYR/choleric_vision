using UnityEngine;
using System.Collections;

public class bodyBagScare : MonoBehaviour {

	public bool flagsTripped;
	public GameObject[] flags;
	public GameObject activate;
	public int flagsReady;
	public AudioSource sound;
	public bool tripped;

	// Use this for initialization
	void Start () {
		sound = gameObject.GetComponent<AudioSource>();
		flagsReady = 0;
		tripped = false;
	}
	
	// Update is called once per frame
	void Update () {

		flagsReady = 0;

		foreach (GameObject g in flags) 
		{
			if (g.GetComponent<playerZoneFlag>().playerFlag)
			{
				flagsReady++;
			}
		}

        if(flagsReady >= flags.Length - 1)
        {
            flagsTripped = true;
        }

	}

	public void OnTriggerEnter(Collider c)
	{
		if (c.CompareTag("GameController") && flagsTripped && !tripped)
		{
			if (!activate.activeSelf)
			{
				activate.SetActive(true);
				tripped = true;
			}

		}
	}
}
