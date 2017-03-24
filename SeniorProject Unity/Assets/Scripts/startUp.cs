using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class startUp : MonoBehaviour
{

    //Check this option to export game with the heartbeat option available. Uncheck will start the game without
    public bool withHeartBeat;
    public bool OriginalWithHeartBeat;

    [System.Serializable]
    public class ss
    {
        public GameObject logo;
        public float djTimer;
        public float djTimerFade;
        public float StartDjTimerFade;

    }
    public ss splashScreen = new ss();

    [System.Serializable]
    public class sts
    {
        public GameObject startScreenOBJ;
        public GameObject gameLogoOBJ;
        public Vector3 gameLogoStartPos;
        public float gameLogoStartSca;
        public Vector3 gameLogoEndPos;
        public float gameLogoEndSca;
        public float gameLogofade;
        public float gameIntroTimer;
        public float gameFadeOutTimer;
    }
    public sts startScreen = new sts();


    public GameObject[] selObj;
    public int selObjNum;

    public GameObject[] selOptions;
    public int selOptNum;

    public GameObject[] hbObj;
    public int hbObjNum;

    public GameObject[] selHbOptions;
    public int selBhOptNum;

    private Color logoCol;
    private float timer;
    private bool fadeIn;
    private float startFade;
    private bool startTrans;
    private bool begining;
    private bool beg;


    private bool sel;
    public bool startGame;
    private bool selHB;
    private bool HBCal;

    private float startFade2;

    private int fadeState;

    public Camera cam;
    private float camColor;

    private int curSelOpt;

    public GameObject audioNarration;
    public float narrationStart;
    public float narrationTimer;

    public bool calibrationStart;

    private GameObject HBData;

    private float tempVal;
    private SpriteRenderer sred;
    private Color sredCol;
    private float fadeScale;

    private float musicVol;

    private float curScale;
    private float curPos;

    private bool prevPressed;

    public AudioClip selectS;
    public AudioClip UpDownS;

    private AudioSource audioMenu;

    // Use this for initialization
    void Start()
    {
        
        HBData = GameObject.FindGameObjectWithTag("hbData");

        cam.clearFlags = CameraClearFlags.SolidColor;

        startScreen.startScreenOBJ.SetActive(false);
        startScreen.gameLogoOBJ.SetActive(false);

        timer = 0.0f;

        logoCol = splashScreen.logo.GetComponent<SpriteRenderer>().color;
        splashScreen.logo.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.0f);


        fadeIn = false;
        startTrans = false;
        begining = beg = true;
        sel = startGame = HBCal = false;
        curSelOpt = 0;


        startFade = splashScreen.djTimer - splashScreen.djTimerFade;
        startFade2 = startScreen.gameIntroTimer - startScreen.gameFadeOutTimer;


        for (int i = 0; i < selObjNum; i++)
            selObj[i].SetActive(false);

        for (int i = 0; i < hbObjNum; i++)
            hbObj[i].SetActive(false);

        audioNarration.SetActive(false);

        calibrationStart = false;

        HBDataScript.orHBBool = OriginalWithHeartBeat;

        withHeartBeat = OriginalWithHeartBeat;

        if (!OriginalWithHeartBeat)
        {
            for (int i = 0; i < GetComponent<startHBCal>().hbObjNum; i++)
                GetComponent<startHBCal>().hbObj[i].SetActive(false);
            for (int i = 0; i < 4; i++)
                GetComponent<startHBCal>().hbState[i].SetActive(false);
            for (int i = 0; i < GetComponent<startHBCal>().askCalOBNum; i++)
                GetComponent<startHBCal>().askCalOB[i].SetActive(false);
            for (int i = 0; i < GetComponent<startHBCal>().yesNoNum; i++)
                GetComponent<startHBCal>().yesNo[i].SetActive(false);
            GetComponent<startHBCal>().enabled = false;
        }

        camColor = 1.0f;

        sred = splashScreen.logo.GetComponent<SpriteRenderer>();
        sredCol = sred.color;

        sredCol.a = 0.0f;

        musicVol = 0.7f;
        curScale = startScreen.gameLogoStartSca;
        curPos = startScreen.gameLogoStartPos.y;

        prevPressed = false;

        audioMenu = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {


        if (timer < splashScreen.djTimer && begining)
        {


            if (timer < splashScreen.StartDjTimerFade && !fadeIn)
            {
                //float t = Mathf.PingPong(Time.time, splashScreen.StartDjTimerFade) / splashScreen.StartDjTimerFade;

                fadeScale = Time.deltaTime / splashScreen.StartDjTimerFade;

                sredCol.a += fadeScale;
                splashScreen.logo.GetComponent<SpriteRenderer>().color = sredCol;

                //splashScreen.logo.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 0.0f), new Color(1f, 1f, 1f, 1.0f), t);
            }
            else if (timer > splashScreen.StartDjTimerFade && !fadeIn)
            {
                fadeIn = true;
            }
            else if (timer >= startFade)
            {
                //float t = Mathf.PingPong(Time.time, splashScreen.djTimerFade) / splashScreen.djTimerFade;

                fadeScale = Time.deltaTime / splashScreen.djTimerFade;

                sredCol.a -= fadeScale;
                splashScreen.logo.GetComponent<SpriteRenderer>().color = sredCol;

                //splashScreen.logo.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 1.0f), new Color(1f, 1f, 1f, 0.0f), t);
            }


            timer += Time.deltaTime;

            if (timer > splashScreen.djTimer)
            {
                startTrans = true;
                timer = 0.0f;
                begining = false;
            }


        }
        else if (timer < startScreen.gameIntroTimer && !begining && beg)
        {

            if (startTrans)
            {
                //float t = Mathf.PingPong(Time.time, startScreen.gameLogofade) / startScreen.gameLogofade;

                fadeScale = Time.deltaTime / startScreen.gameLogofade;

                splashScreen.logo.SetActive(false);

                camColor -= fadeScale;

                startScreen.gameLogoOBJ.SetActive(true);
                cam.backgroundColor = new Color(camColor, camColor, camColor, 1.0f);



                if (timer > startScreen.gameLogofade)
                    startTrans = false;

                startScreen.gameLogoOBJ.GetComponent<AudioSource>().volume = musicVol;
            }

            else if (timer >= startFade2 && !startTrans)
            {
                //float t = Mathf.PingPong(Time.time, startScreen.gameFadeOutTimer) / startScreen.gameFadeOutTimer;

                Vector3 newScale = new Vector3(startScreen.gameLogoEndSca, startScreen.gameLogoEndSca, startScreen.gameLogoEndSca);
                Vector3 newScale2 = new Vector3(startScreen.gameLogoEndSca, startScreen.gameLogoEndSca, startScreen.gameLogoEndSca);

                float scaleFac = Math.Abs(startScreen.gameLogoEndSca - startScreen.gameLogoStartSca);

                float musicScale = 1.0f - 0.7f;
                fadeScale = Time.deltaTime / startScreen.gameFadeOutTimer * musicScale;

                musicVol += fadeScale;

                startScreen.gameLogoOBJ.GetComponent<AudioSource>().volume += musicVol;

                fadeScale = Time.deltaTime / startScreen.gameFadeOutTimer * scaleFac;

                curScale += fadeScale;

                startScreen.gameLogoOBJ.transform.localScale = new Vector3(curScale, curScale, curScale);

                float PosFac = Math.Abs(startScreen.gameLogoEndPos.y - startScreen.gameLogoStartPos.y);
                fadeScale = Time.deltaTime / startScreen.gameFadeOutTimer * PosFac;

                //curPos += fadeScale;

                startScreen.gameLogoOBJ.transform.position += new Vector3(0.0f, fadeScale, 0.0f);


                startScreen.startScreenOBJ.SetActive(true);

                for (int i = 0; i < selObjNum; i++)
                    selObj[i].SetActive(true);

                for (int i = 0; i < selObjNum; i++)
                {
                    //selObj[i].GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 1.0f), new Color(1f, 1f, 1f, 0.0f), t);
                }
            }


            timer += Time.deltaTime;

            if (timer > startScreen.gameIntroTimer)
            {
                beg = false;
                sel = true;
            }



        }


        if (selHB)
        {
            for (int i = 0; i < selBhOptNum; i++)
            {
                if (curSelOpt == i)
                    selHbOptions[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                else
                    selHbOptions[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }

            if (Input.GetAxis("Vertical") < 0.0f && !prevPressed)
            {
                prevPressed = true;
                curSelOpt += 1;
                audioMenu.clip = UpDownS;
                audioMenu.Play();
            }
            if (Input.GetAxis("Vertical") > 0.0f && !prevPressed)
            {
                curSelOpt -= 1;
                prevPressed = true;
                audioMenu.clip = UpDownS;
                audioMenu.Play();
            }
            if (Input.GetAxis("Vertical") == 0.0)
            {
                prevPressed = false;
            }


            if (curSelOpt < 0)
                curSelOpt = selBhOptNum - 1;
            if (curSelOpt > selBhOptNum - 1)
                curSelOpt = 0;

            if (Input.GetButtonDown("Action") || Input.GetButtonDown("Submit"))
                {
                audioMenu.clip = selectS;
                audioMenu.Play();

                if (curSelOpt == 0)
                    {

                        withHeartBeat = true;
                        calibrationStart = true;
                        selHB = false;

                        HBDataScript.hbBool = withHeartBeat; 

                        for (int i = 0; i < hbObjNum; i++)
                            hbObj[i].SetActive(false);

                        startScreen.startScreenOBJ.SetActive(false);

                        startScreen.gameLogoOBJ.SetActive(false);

                    }
                    if (curSelOpt == 1)
                    {
                        startGame = true;
                        selHB = false;
                        withHeartBeat = false;

                        HBDataScript.hbBool = withHeartBeat;

                        for (int i = 0; i < hbObjNum; i++)
                                hbObj[i].SetActive(false);

                            startScreen.startScreenOBJ.SetActive(false);

                            startScreen.gameLogoOBJ.SetActive(false);

                            timer = 0.0f;
                        }
                }
            if (Input.GetButtonDown("Back"))
            {
                audioMenu.clip = selectS;
                audioMenu.Play();

                selHB = false;

                sel = true;

                for (int i = 0; i < selObjNum; i++)
                    selObj[i].SetActive(true);

                for (int i = 0; i < hbObjNum; i++)
                    hbObj[i].SetActive(false);
            }


        }



        if (sel && !beg)
        {
            for (int i = 0; i < selOptNum; i++)
            {
                if (curSelOpt == i)
                    selOptions[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                else
                    selOptions[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }

            if (Input.GetAxis("Vertical") < 0.0f && !prevPressed)
            {
                curSelOpt += 1;
                prevPressed = true;
                audioMenu.clip = UpDownS;
                audioMenu.Play();
            }
            if (Input.GetAxis("Vertical") > 0.0f && !prevPressed)
            {
                curSelOpt -= 1;
                prevPressed = true;
                audioMenu.clip = UpDownS;
                audioMenu.Play();
            }
            if (Input.GetAxis("Vertical") == 0.0)
            {
                prevPressed = false;
            }


            if (curSelOpt < 0)
                curSelOpt = selOptNum - 1;
            if (curSelOpt > selOptNum - 1)
                curSelOpt = 0;

            if (Input.GetButtonDown("Action") || Input.GetButtonDown("Submit"))
            {
                audioMenu.clip = selectS;
                audioMenu.Play();

                if (curSelOpt == 0 && !withHeartBeat)
                {
                    sel = false;
                    startGame = true;

                    for (int i = 0; i < selObjNum; i++)
                        selObj[i].SetActive(false);

                    startScreen.startScreenOBJ.SetActive(false);

                    startScreen.gameLogoOBJ.SetActive(false);

                    timer = 0.0f;

                    withHeartBeat = false;

                    HBDataScript.hbBool = withHeartBeat;
                }
                if (curSelOpt == 0 && withHeartBeat)
                {
                    sel = false;
                    selHB = true;

                    for (int i = 0; i < selObjNum; i++)
                        selObj[i].SetActive(false);

                    for (int i = 0; i < hbObjNum; i++)
                        hbObj[i].SetActive(true);

                    //startScreen.startScreenOBJ.SetActive(false);

                    //startScreen.gameLogoOBJ.SetActive(false);

                    timer = 0.0f;
                }
                if (curSelOpt == 1)
                {
                    Application.Quit();
                }
                if (curSelOpt == 2)
                {
                    SceneManager.LoadScene("credits", LoadSceneMode.Single);
                }
            }
        }


        if (startGame)
        {
            if (timer > narrationStart && !audioNarration.activeSelf)
            {
                audioNarration.SetActive(true);
            }

            timer += Time.deltaTime;

            if(timer > narrationTimer)
                SceneManager.LoadScene("Beta_1.2.8", LoadSceneMode.Single);

            //yield return new WaitForSeconds(5.0f);
        }

        
    }
}
