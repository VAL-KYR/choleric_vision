using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using UnityEngine.UI;

public class heartBeat : MonoBehaviour {

    public bool debug; 

    public int curBeatPerMin;
    public string curBeatPerMinString;

    public int numOfHeartAvg;
    public int[] OfHeartAvgCount;
    private int numOfHeartAvgDone;

    public int avBeatPerMin;

    SerialPort sp = new SerialPort("COM7", 9600);

    private int BPMavg;

    private int calState;
    // 0 = Calibration not done
    // 1 = Calibrating
    // 2 = Done


    public bool cali;


    // Use this for initialization
    void Start () {

        calState = 0;

        numOfHeartAvgDone = 0; 
        

        avBeatPerMin = 0;

        sp.Open();
        sp.ReadTimeout = 1;

        cali = false;
    }
	
	// Update is called once per frame
	void Update () {

        if(debug)
            Debug.Log("Calibration State: " + calState);

        if(calState == 0)
        {
            cali = GetComponent<controller>().calibrationDone;

            if(cali == false)
            {

            }
            else if (cali == true)
            {
                calState = 1;
            }
        }
        else if (calState == 1)
        {
            if (numOfHeartAvgDone < numOfHeartAvg)
            {

                if (sp.IsOpen)
                {
                    try
                    {

                        curBeatPerMinString = sp.ReadLine();


                        int OfHeartAvgCountTemp = int.Parse(curBeatPerMinString);


                        if (OfHeartAvgCountTemp <= 50 || OfHeartAvgCountTemp >= 110)
                        {

                        }
                        else
                        {
                            OfHeartAvgCount[numOfHeartAvgDone] = OfHeartAvgCountTemp;

                            numOfHeartAvgDone++;
                        }
                        
                    }
                    catch (System.Exception)
                    {

                    }
                }
            }
            else if (numOfHeartAvgDone == numOfHeartAvg)
            {
                for (int i = 0; i < numOfHeartAvg; i++)
                {
                    avBeatPerMin = avBeatPerMin + OfHeartAvgCount[i];
                }

                avBeatPerMin = avBeatPerMin / numOfHeartAvg;


                calState = 2;
                GetComponent<Presence>().BPMBool = true;
                GetComponent<Presence>().avrBPM = avBeatPerMin;
            }
        }
        else if (calState == 2)
        {
            if (sp.IsOpen)
            {
                try
                {

                    curBeatPerMinString = sp.ReadLine();

                    int curBeatPerMinTemp = int.Parse(curBeatPerMinString);


                    if (curBeatPerMinTemp > 50)
                    {
                        curBeatPerMin = curBeatPerMinTemp;
                        GetComponent<Presence>().curBPM = curBeatPerMin;
                    }
                        

                    print(curBeatPerMin);
                }
                catch (System.Exception)
                {

                }
            }
        }
        
        
    }

    void updateHBText(int curBeatPerMin)
    {
        string HBdisplay = curBeatPerMin.ToString();

    }

    
    // All serial ports connect
    // Open(s) not in current context error?
    /*
    public bool Connect()
    {
        sp.ReadTimeout = 1;
        sp.WriteTimeout = 1;
        if (debug)
            Debug.Log("Start connecting Serial Device");

        bool connect = false;
        //find the COM and connect

        foreach (string s in SerialPort.GetPortNames())
        {
            connect = Open(s);
            if (connect)
            {
                if (debug)
                    Debug.Log("First Connected COM device on Port: " + s);
                break;
            }
        }
        if (!connect)
            Debug.LogWarning("All available COM ports tried");
        return connect;
    }
    */
}
