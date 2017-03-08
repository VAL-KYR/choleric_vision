﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class flashLightOnOff : MonoBehaviour
{

    public Light flashlight;

	public bool debug = false;

    public float lightIntensity;


    // Use this for initialization
    void Start()
    {
        flashlight = gameObject.GetComponent<Light>();

        flashlight.intensity = lightIntensity;

		flashlight.enabled = false;
    }

	// Update is called once per frame
	void Update()
	{
		if (debug)
			Debug.Log(lightIntensity);

		if (Input.GetButtonDown("FlashLight"))
		{
			if(flashlight.isActiveAndEnabled)
				flashlight.enabled = false;
			else
				flashlight.enabled = true;
		}



    }

};
