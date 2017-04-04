using UnityEngine;
using System.Collections;

public class inputHandler : MonoBehaviour {

    CursorLockMode wantedMode;

    static public string comNum;

    public GameObject comUI;

    public void Start()
    {
        // lock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        comUI.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetButtonDown("COMtoggle"))
        {
            if (comUI.activeSelf)
            {
                comUI.SetActive(false);
            }
            else
            {
                comUI.SetActive(true);
            }
        }
    }

	// Use this for initialization
	public void GetInput(string comString)
	{
		comNum = comString;
		Debug.Log("You entered " + comNum);
	}

}
