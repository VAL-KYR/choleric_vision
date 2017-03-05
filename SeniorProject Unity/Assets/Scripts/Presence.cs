using UnityEngine;
using System.Collections;

public class Presence : MonoBehaviour {

    public float curPres = 0.5f;

    //HB Variables
    private int hbHighest;
    public int curBPM;
    public int avrBPM;
    public bool BPMBool;
    private float HBVar;
    public float hbTimerIni;
    private float hbTimerCur;

    //Walking Speed Variables
    public float xDir;
    public float zDir;
    private float speedVar;

    //Sound Sampling
    private int qSamples = 1024;
    private float refValue = 0.1f;
    private float rmsValue;
    private float dbValue;
    private float[] samples;
    
    

    // Use this for initialization
    void Start () {

        hbHighest = 0;
        hbTimerCur = 0.0f;
        HBVar = 0.0f;

        samples = new float[qSamples];

        
    }

    // Update is called once per frame
    void Update() {
        
        float speed = Mathf.Sqrt(Mathf.Pow(xDir, 2) + (Mathf.Pow(zDir, 2)));
        
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

        if (BPMBool)//if player playing with HB sensor
        {
            speedVar = (speed * 0.041f) - 0.15f;


            if (curBPM > hbHighest)
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
                if(hbHighest < avrBPM + 7)
                    curPres = 50.0f + (HBVar) * speedVar;
                else
                    curPres = (0.5f + (HBVar - (Time.deltaTime * 3)) * speedVar * 0.01f);


                hbTimerCur -= Time.deltaTime;
            }
            else
            {
                hbHighest = 0;
            }

        }
        else //if player not playing with HB sensor
        {
            speedVar = (speed * 0.069f) - 0.15f;

            curPres = 0.5f + speedVar;
        }
    }
}