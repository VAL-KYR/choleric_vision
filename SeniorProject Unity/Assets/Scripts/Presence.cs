using UnityEngine;
using System.Collections;

public class Presence : MonoBehaviour {

    public bool debug = false;

    public float curPres = 0.5f;

    //HB Variables
    private int hbHighest;
    private int curBPM;
    private int avrBPM;
    public bool BPMBool;
    private float HBVar;
    public float hbTimerIni;
    private float hbTimerCur;

    //Walking Speed Variables
    private float speedVar;
    public float time;

    //Sound Sampling
    private int qSamples = 1024;
    private float refValue = 0.1f;
    private float rmsValue;
    private float dbValue;
    private float[] samples;

    private Vector3 velocity;
    private Vector3 lastPosition;

    [System.Serializable]
    public class presenceTweak
    {
        public float HBVarFactor = 0.6f;
        public float crouchFactor = 0.1f;
        public bool playerCrouch = false;
        public bool playerFlashlight = false;
    }
    public presenceTweak presence = new presenceTweak();

    //public bool playerCrouch = false;
    //public bool playerFlashlight = false;

    public GameObject[] flashLights;
    

    // Use this for initialization
    void Start () {

        hbHighest = 0;
        hbTimerCur = 0.0f;
        HBVar = 0.0f;

        samples = new float[qSamples];

		// update player flashlight status
		foreach (GameObject f in flashLights)
		{
			if (f.activeSelf)
			{
                presence.playerFlashlight = f.GetComponent<flashLightOnOff>().flashlight.isActiveAndEnabled;
			}

		}

        curBPM = GetComponent<heartBeat>().curBeatPerMin;
        avrBPM = GetComponent<heartBeat>().avBeatPerMin;

        presence.playerCrouch = GameObject.FindGameObjectWithTag("GameController").GetComponent<controller>().crouch;
        lastPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update() {

        // update player crouch status
        presence.playerCrouch = GameObject.FindGameObjectWithTag("GameController").GetComponent<controller>().crouch;

        // update player flashlight status
        foreach (GameObject f in flashLights)
        {
            if (f.activeSelf)
            {
                presence.playerFlashlight = f.GetComponent<flashLightOnOff>().flashlight.isActiveAndEnabled;
            }
            
        }

        curBPM = GetComponent<heartBeat>().curBeatPerMin;
        avrBPM = GetComponent<heartBeat>().avBeatPerMin;


        // velocity and speed calculations
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        float speed = velocity.magnitude;

        /*int i;
        float sum = 0f;
        for (i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average
        dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min

        print(dbValue);
        */
        time += Time.deltaTime;

        if (BPMBool)//if player playing with HB sensor
        {
            HBVar = ((float)curBPM / (float)avrBPM) - 1.0f;  // 0 to 1 factor

            if (time > 0.1f)
            {
                if (presence.playerCrouch)
                {
                    speedVar = ((speed * 0.069f) - 0.15f) - presence.crouchFactor;
                    time = 0;
                }
                else
                {
                    speedVar = (speed * 0.069f) - 0.15f;
                    time = 0;
                }

                curPres = (0.5f + speedVar) + (HBVar * presence.HBVarFactor);
            }
            
            

            /*
            if (debug)
            {
                Debug.Log("Using heartrate for presence");
            }

            speedVar = (speed * 0.041f) - 0.15f;


            if (curBPM + 10 > hbHighest)
            {
                hbHighest = curBPM;
                hbTimerCur = hbTimerIni;

                if (curBPM < avrBPM + 7)
                    HBVar = 0.0f;
                else
                {
                    HBVar = (curBPM * 10) * (avrBPM * 0.0001f);
                }
                
                
            }

            
            if(hbTimerCur > 0.0f)
            {
                // HB Presence calculation
                if(hbHighest < avrBPM + 7)
                {
                    // crouching reduces presence
                    if (playerCrouch)
                    {
                        curPres = (0.5f + (HBVar) * speedVar) - presence.crouchFactor;
                    }
                    else
                    {
                        curPres = 0.5f + (HBVar) * speedVar;
                    }
                }
                    
                else
                {
                    // crouching reduces presence
                    if (playerCrouch)
                    {
                        curPres = (0.5f + (HBVar - (Time.deltaTime * 3)) * speedVar * 0.01f) - presence.crouchFactor;
                    }
                    else
                    {
                        curPres = (0.5f + (HBVar - (Time.deltaTime * 3)) * speedVar * 0.01f);
                    }
                }
                    


                hbTimerCur -= Time.deltaTime;
            }
            else
            {
                hbHighest = 0;
            }
            */

        }
        else //if player not playing with HB sensor
        {
            if (debug)
            {
                Debug.Log("Not using heartrate for presence");
            }

            // crouching reduces presence
            if (presence.playerCrouch)
            {
                speedVar = ((speed * 0.069f) - 0.15f) - presence.crouchFactor;
            }
            else
            {
                speedVar = (speed * 0.069f) - 0.15f;
            }
            

            // No HB Presence calculation
            curPres = 0.5f + speedVar;
        }
    }
}