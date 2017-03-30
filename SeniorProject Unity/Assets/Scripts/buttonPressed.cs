using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class buttonPressed : MonoBehaviour {

    public static bool xboxPress;
    private GameObject hintsPage;
    public Sprite xbox;
    public Sprite keys;
    public bool xboxPressed;
    public bool UIReady;
    private Vector3 startScale = new Vector3(0, 0, 0);
    private Vector3 startPos = new Vector3(0, 0, 0);


    // Use this for initialization
    void Start () {

        xboxPress = false;

        if (GameObject.FindGameObjectWithTag("hints"))
        {
            hintsPage = GameObject.FindGameObjectWithTag("hints");
            UIReady = true;

            if (startScale == new Vector3(0, 0, 0))
            {
                startScale = hintsPage.transform.localScale;
            }

            if (startPos == new Vector3(0, 0, 0))
            {
                startPos = hintsPage.transform.localPosition;
            }
        }
        else
        {
            UIReady = false;
        }

        

    }

    // Update is called once per frame
    void Update() {

        // searching for page at runtime
        if (!hintsPage)
        {
            if (GameObject.FindGameObjectWithTag("hints"))
            {
                hintsPage = GameObject.FindGameObjectWithTag("hints");
                UIReady = true;

                if (startScale == new Vector3(0, 0, 0))
                {
                    startScale = hintsPage.transform.localScale;
                }

                if (startPos == new Vector3(0, 0, 0))
                {
                    startPos = hintsPage.transform.localPosition;
                }
            }
            else if (!GameObject.FindGameObjectWithTag("hints"))
            {
                UIReady = false;
            }

            
        }

        xboxPressed = xboxPress;

        if (Input.anyKeyDown)
        {
            for (int i = 0; i < 20; i++)
            {
                xboxPress = false;

                if (Input.GetKeyDown("joystick 1 button " + i))
                {
                    //print("XBOX Pressed");
                    xboxPress = true;
                }
                if (xboxPress)
                    break;
            }
            if (!xboxPress)
            {
                //print("Keyboard Pressed");
            }

        }

        // when page is found
        if (UIReady)
        {

            // Switch UI
            if (xboxPress)
            {
                hintsPage.GetComponent<Image>().sprite = xbox;
                hintsPage.transform.localScale = startScale;
                hintsPage.transform.localPosition = startPos;
            }
            else
            {
                hintsPage.GetComponent<Image>().sprite = keys;
                hintsPage.transform.localScale = startScale * 0.61f;
                hintsPage.transform.localPosition = new Vector3(startPos.x + (startPos.y * 1.2f), startPos.y, startPos.z);
            }


        }

    }
}
