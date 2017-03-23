using UnityEngine;
using System.Collections;

public class inputHandler : MonoBehaviour {

	static public string comNum;

    public GameObject comUI;

    public void Start()
    {
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
