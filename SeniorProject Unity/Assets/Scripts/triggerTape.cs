using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class triggerTape : MonoBehaviour {

	public GameObject tapeRecorder;
	public bool triggered = false;

    public bool armed = false;

    // For Zone Flags
    public List<GameObject> flags;
    public int lessFlags = 0;

    // Use this for initialization
    void Start () {
		triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
        // Flags trigger forced open door
        if (flags.Count > 0)
        {

            int flagsConfirmed = 0;

            for (int t = 0; t < flags.Count; t++)
            {
                if (flags[t].GetComponent<playerZoneFlag>().playerFlag)
                {
                    flagsConfirmed++;
                }
            }

            if (flagsConfirmed >= (flags.Count - lessFlags))
            {
                armed = true;
            }
        }
        //

    }

    void OnTriggerEnter()
	{
		if (!triggered && armed)
		{
			tapeRecorder.GetComponent<tapeRecorder>().remote();
			triggered = true;
		}

	}
}
