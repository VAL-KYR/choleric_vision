using UnityEngine;
using System.Collections;

public class HBDataScript : MonoBehaviour {

	public bool debug = false;
    public static int HBAvg;
    public static bool hbBool;
	public static string comNum;

    public static bool orHBBool;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (debug)
			Debug.Log("HBDataCom Recieved through " + comNum);
	}

	public void GetInput(string comString)
	{
		comNum = comString;
		//Debug.Log("You entered " + comNum);
	}
}
