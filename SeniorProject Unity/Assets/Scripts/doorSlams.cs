using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class doorSlams : MonoBehaviour {
    public AudioClip scareSound;

    private GameObject activeFlashLight;
    public GameObject flashLight;
    public GameObject VRFlashLight;

    public GameObject[] doors;
    public bool triggerDoors = false;
    public bool reTrigger = false;

   
    public float slamTime = 10.0f;
    public float time = 0.0f;

    public float delay = 1.0f;
    public float delayTime = 0.0f;

	// Use this for initialization
	void Start () {
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


        if (triggerDoors && gameObject.GetComponent<AudioSource>().isPlaying)
        {
            time += Time.deltaTime;

            delayTime += Time.deltaTime;

            if (delayTime >= delay)
            {
                foreach (GameObject d in doors)
                {
                    float action = Random.value;

                    if (action >= 0.5f)
                    {
                        if (!d.GetComponent<doorMaster>().doorOpen)
                        {
                            d.GetComponent<doorMaster>().forceOpen();
                        }
                        else
                        {
                            d.GetComponent<doorMaster>().forceClosed();
                        }
                    }

                }

                // reset the delay
                delayTime = 0.0f;
            }
            
        }
        else
        {
            if (reTrigger)
            {
                triggerDoors = false;
                time = 0.0f;
            }

            
        }
        
	}

    public void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController") && !triggerDoors)
        {
            triggerDoors = true;

            gameObject.GetComponent<AudioSource>().clip = scareSound;
            gameObject.GetComponent<AudioSource>().Play();
            activeFlashLight.GetComponent<flashLightOnOff>().flashPeriod = (scareSound.length - slamTime);
            activeFlashLight.GetComponent<flashLightOnOff>().flashing = true;
        }
    }
}
