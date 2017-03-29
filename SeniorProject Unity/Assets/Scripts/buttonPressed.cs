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


    // Use this for initialization
    void Start () {

        xboxPress = false;

        if (GameObject.FindGameObjectWithTag("hints"))
        {
            hintsPage = GameObject.FindGameObjectWithTag("hints");
            UIReady = true;
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
            }
            else
            {
                hintsPage.GetComponent<Image>().sprite = keys;
            }


        }

    }
}
