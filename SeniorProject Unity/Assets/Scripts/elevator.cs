using UnityEngine;
using System.Collections;

public class elevator : MonoBehaviour {


    public GameObject playerC;

	public bool debug;

    public float eleSpeed;
    public float eleMaxHeight;
    public float eleMinHeight;
	public bool playerInElevator = false;
	public bool powerOn = false;

    private Vector3 playerPos;
    private Vector3 elePos;

    private float currentDis;
    private float eleXPos;
    private float eleYPos;
    private float eleZPos;

    private bool eleTop;
    private bool onEle;
    private bool eleDone;

    // Use this for initialization
    void Start () {
        elePos = transform.position;

        eleXPos = elePos[0];
        eleYPos = elePos[1];
        eleZPos = elePos[2];

        eleTop = true;
        onEle = false;
        eleDone = true;
    }
	
	// Update is called once per frame
	void Update () {

        playerPos = playerC.transform.position;
        elePos = transform.position;


        currentDis = Vector3.Distance(playerPos, elePos);

		if(debug)
        	Debug.Log("Ele Dist: " + currentDis);
		
		if (playerInElevator && powerOn)
			onEle = true;
		else
			onEle = false;

        if (eleTop == true && onEle == true)
        {
            eleYPos -= eleSpeed;
            transform.position = new Vector3(eleXPos, eleYPos, eleZPos);

            if (eleYPos <= eleMaxHeight)
            {
                eleYPos = eleMaxHeight;
                eleTop = false;
            }
        }

    }

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("GameController"))
			playerInElevator = true;
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("GameController"))
			playerInElevator = false;
	}

	public void powerSupplied()
	{
		powerOn = true;
	}

}
