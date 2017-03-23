using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class credits : MonoBehaviour {


    public GameObject[] creditOB;

    public int num;

    public float onScreenTimer;

    public float fade;

    public float timer;


    public float blankTime;

    public float endTimer;

    private int curItem;

    private bool itmOnScreen;


    private float fadeAway;

    private float fadeScale;

    private int fadeState;

    private SpriteRenderer sred;
    private Color sredCol;

    private bool endCred;


    // Use this for initialization
    void Start () {

        for(int i = 0; i < num; i++)
            creditOB[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

        timer = 0.0f;


        itmOnScreen = false;

        fadeAway = onScreenTimer - fade;

        curItem = 0;

        sred = creditOB[curItem].GetComponent<SpriteRenderer>();
        sredCol = sred.color;

        endCred = false;
    }
	
	// Update is called once per frame
	void Update () {

        if(!endCred)
        {
            if (fadeState == 0)
            {
                creditOB[curItem].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                timer += Time.deltaTime;

                if (timer >= blankTime)
                {
                    fadeState++;
                    timer = 0.0f;
                }
            }
            else if (fadeState == 1)
            {
                fadeScale = Time.deltaTime / fade;

                sredCol.a += fadeScale;

                creditOB[curItem].GetComponent<SpriteRenderer>().color = sredCol;

                if (sredCol.a >= 1.0f)
                {
                    fadeState++;

                }

                timer += Time.deltaTime;
            }
            else if (fadeState == 2)
            {


                if (timer >= fadeAway)
                {
                    fadeState++;

                }

                timer += Time.deltaTime;
            }
            else if (fadeState == 3)
            {
                fadeScale = Time.deltaTime / fade;

                sredCol.a -= fadeScale;

                if (curItem == num - 1)
                {

                }
                else
                {
                    creditOB[curItem].GetComponent<SpriteRenderer>().color = sredCol;
                }


                if (timer >= onScreenTimer)
                {
                    fadeState = 0;
                    timer = 0.0f;

                    sredCol.a = 0.0f;

                    creditOB[curItem].GetComponent<SpriteRenderer>().color = sredCol;


                    curItem++;

                    if(curItem >= num)
                    {
                        endCred = true;
                    }
                    else
                    {
                        sred = creditOB[curItem].GetComponent<SpriteRenderer>();
                        sredCol = sred.color;
                    }

                    
                }

                timer += Time.deltaTime;
            }
        }
        if (endCred)
        {
            timer += Time.deltaTime;

            if (timer >= endTimer)
            {
                SceneManager.LoadScene("startScreen", LoadSceneMode.Single);
            }

        }



            if (Input.GetButtonDown("Action"))
        {
            SceneManager.LoadScene("startScreen", LoadSceneMode.Single);
        }

    }
}
