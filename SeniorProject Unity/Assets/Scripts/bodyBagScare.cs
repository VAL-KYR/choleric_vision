using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class bodyBagScare : MonoBehaviour {

    private GameObject activeFlashLight;
    public GameObject flashLight;
    public GameObject VRFlashLight;
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

        if (VRSettings.enabled)
        {
            activeFlashLight = VRFlashLight;
        }
        else
        {
            activeFlashLight = flashLight;
        }
        

	}
	
	// Update is called once per frame
	void Update () {

        if (VRSettings.enabled)
        {
            activeFlashLight = VRFlashLight;
        }
        else
        {
            activeFlashLight = flashLight;
        }

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

                activeFlashLight.GetComponent<flashLightOnOff>().flashPeriod = 2.0f;
                activeFlashLight.GetComponent<flashLightOnOff>().flashing = true;

                tripped = true;
			}

		}
	}
}
