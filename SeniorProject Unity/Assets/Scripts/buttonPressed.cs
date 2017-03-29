using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class buttonPressed : MonoBehaviour {

    static public bool xboxPress;
    private GameObject hintsPage;
    public Sprite xbox;
    public Sprite keys;
    public bool xboxPressed;


    // Use this for initialization
    void Start () {
        xboxPress = false;
        hintsPage = GameObject.FindGameObjectWithTag("hints");
    }
	
	// Update is called once per frame
	void Update () {

        xboxPressed = xboxPress;

        if(Input.anyKeyDown)
        {
            for (int i = 0; i < 20; i++)
            {
                xboxPress = false;

                if (Input.GetKeyDown("joystick 1 button " + i))
                {
                    print("XBOX Pressed");
                    xboxPress = true;
                }
                if (xboxPress)
                    break;
            }
            if (!xboxPress)
                print("Keyboard Pressed");
        }



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
