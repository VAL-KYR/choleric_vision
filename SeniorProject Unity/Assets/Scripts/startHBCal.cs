using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using UnityEngine.UI;

public class startHBCal : MonoBehaviour {

    public int curBeatPerMin;
    public string curBeatPerMinString;

    public int numOfHeartAvg;
    public int[] OfHeartAvgCount;
    private int numOfHeartAvgDone;



    private float q1;
    private float q3;

    bool q1M;
    bool q3M;

    private int q1Num;
    private int q3Num;


	public bool readyCom = false;
	public GameObject comEntry;

    SerialPort sp;

    private int BPMavg;

    private int calState;
    // 0 = Calibration not done
    // 1 = Calibrating
    // 2 = Done

    public int avBeatPerMin;

    public bool cali;

    //public String comPort;

    public GameObject[] hbObj;
    public int hbObjNum;

    public GameObject loadBar;

    private float barX;
    private float perBerBeat;

    public GameObject[] hbState;

    int outlier;

    private bool askCal;

    private int sel;
    public GameObject[] askCalOB;
    public int askCalOBNum;

    public GameObject[] yesNo;
    public int yesNoNum;

    private bool prevPressed;

    public AudioClip selectS;
    public AudioClip UpDownS;

    private AudioSource audioMenu;


    // Use this for initialization
    void Start () {

        calState = 0;

        numOfHeartAvgDone = 0;


        avBeatPerMin = 0;

		// returns null because duh startup order
		//sp = new SerialPort(HBDataScript.comNum, 9600);
        //sp.Open();
        //sp.ReadTimeout = 1;

        cali = false;

        for (int i = 0; i < hbObjNum; i++)
            hbObj[i].SetActive(false);

        for(int i = 0; i < askCalOBNum; i++)
            askCalOB[i].SetActive(false);

        

        for (int i = 0; i < 4; i++)
            hbState[i].SetActive(false);  

        barX = loadBar.transform.localScale.x;
        loadBar.transform.localScale -= new Vector3 (barX, 0.0f, 0.0f);

        perBerBeat = barX / numOfHeartAvg ;

        askCal = false;
        sel = 0;

        prevPressed = false;

        audioMenu = GetComponent<AudioSource>();
    }

	// Update is called once per frame
	void Update () {

		//sp = new SerialPort(HBDataScript.comNum, 9600);
		//Debug.Log("Serial # " + sp.PortName);
		//Debug.Log("Serial Reading " + sp.ReadLine());

        if (calState == 0)
        {
            cali = GetComponent<startUp>().calibrationStart;


            if (cali == false)
            {

            }

            else if (cali == true)
            {

				if (readyCom)
				{
					sp = new SerialPort(HBDataScript.comNum, 9600);

					if (!sp.IsOpen)
					{
						sp.Open();
						sp.ReadTimeout = 1;

						//Debug.Log("Serial # " + sp.PortName);
						//Debug.Log("Serial Reading " + sp.ReadLine());

						calState = 1;
					}

				}

			}
            
        }
        else if (calState == 1)
        {
            for (int i = 0; i < hbObjNum; i++)
                hbObj[i].SetActive(true);

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
                            
                            loadBar.transform.localScale += new Vector3(perBerBeat, 0.0f, 0.0f);
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

                HBDataScript.HBAvg = avBeatPerMin;

                outlier = calOutliers(numOfHeartAvg, OfHeartAvgCount, avBeatPerMin);

                calState = 2;
            }
        }
        else if (calState == 2)
        {
            for (int i = 0; i < hbObjNum; i++)
                hbObj[i].SetActive(false);


            if (outlier >= 0 && outlier < 3)
                hbState[0].SetActive(true);
            else if (outlier >= 3 && outlier < 4)
                hbState[1].SetActive(true);
            else if (outlier >= 4 && outlier < 6)
                hbState[2].SetActive(true);
            else if (outlier >= 6)
                hbState[3].SetActive(true);

            askCal = true;



            for (int i = 0; i < askCalOBNum; i++)
                askCalOB[i].SetActive(true);
             
        }
        else if (calState == 3)
        {
			// Turn off ComEntry UI
			comEntry.SetActive(false);
        }

        if (askCal)
        {
            for (int i = 0; i < yesNoNum; i++)
            {
                if (sel == i)
                    yesNo[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                else
                    yesNo[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }

            if (Input.GetAxis("Vertical") < 0.0f && !prevPressed)
            {
                prevPressed = true;
                sel += 1;
                audioMenu.clip = UpDownS;
                audioMenu.Play();
            }

            if (Input.GetAxis("Vertical") > 0.0f && !prevPressed)
            {
                prevPressed = true;
                sel -= 1;
                audioMenu.clip = UpDownS;
                audioMenu.Play();
            }
            if (Input.GetAxis("Vertical") == 0.0)
            {
                prevPressed = false;
            }


            if (sel < 0)
                sel = yesNoNum - 1;
            if (sel > yesNoNum - 1)
                sel = 0;

            if (Input.GetButtonDown("Action") || Input.GetButtonDown("Submit"))
            {
                audioMenu.clip = selectS;
                audioMenu.Play();

                if (sel == 0)
                {
                    GetComponent<startUp>().startGame = true;


                    if (outlier >= 0 && outlier < 3)
                        hbState[0].SetActive(false);
                    else if (outlier >= 3 && outlier < 4)
                        hbState[1].SetActive(false);
                    else if (outlier >= 4 && outlier < 6)
                        hbState[2].SetActive(false);
                    else if (outlier >= 6)
                        hbState[3].SetActive(false);

                    for (int i = 0; i < askCalOBNum; i++)
                        askCalOB[i].SetActive(false);

                    calState = 3;

                    askCal = false;

                    sp.Close();
                }
                if (sel == 1)
                {

                    calState = 1;

                    for (int i = 0; i < askCalOBNum; i++)
                        askCalOB[i].SetActive(false);

                    numOfHeartAvgDone = 0;

                    loadBar.transform.localScale -= new Vector3(barX, 0.0f, 0.0f);

                    avBeatPerMin = 0;
                    sel = 0;

                    if (outlier >= 0 && outlier < 3)
                        hbState[0].SetActive(false);
                    else if (outlier >= 3 && outlier < 4)
                        hbState[1].SetActive(false);
                    else if (outlier >= 4 && outlier < 6)
                        hbState[2].SetActive(false);
                    else if (outlier >= 6)
                        hbState[3].SetActive(false);
                }
            }
        }

    }

    int calOutliers(int numOfHeartAvg, int[] OfHeartAvgCount, int avBeatPerMin)
    {
        Array.Sort(OfHeartAvgCount);

        q1 = numOfHeartAvg * 0.25f;
        q3 = numOfHeartAvg * 0.75f;

        int newQ1 = (int)q1;
        int newQ3 = (int)q3;
        

        int q1Num = OfHeartAvgCount[newQ1];

        int q3Num = OfHeartAvgCount[newQ3];


        float IQR = (q3Num - q1Num);

        float out1 = q1Num - (1.5f * IQR);
        int newOut1 = (int)out1;

        float out3 = q3Num + (1.5f * IQR);
        int newOut3 = (int)out3;

        print(newOut1 + " " + newOut3);

        int outNum = 0;

        for(int i = 0; i < numOfHeartAvg; i++)
            if (OfHeartAvgCount[i] < newOut1 || OfHeartAvgCount[i] > newOut3)
                outNum++;
        
        if(OfHeartAvgCount[(numOfHeartAvg - 1)] - OfHeartAvgCount[0] >= 40)
        {
            outNum = 10;
        }

        return (outNum);

    }
    
}
