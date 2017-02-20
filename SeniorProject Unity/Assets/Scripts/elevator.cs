using UnityEngine;
using System.Collections;

public class elevator : MonoBehaviour {
    // Key audio compenent for all key sounds
    AudioSource elevatorSource;

    // elevator ready sounds
    public AudioClip[] bellSounds;
    public AudioClip bellSound;
	public GameObject soundDone;
    private bool bellSounded = false;

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


	public GameObject[] upperDoors;
	//public GameObject upperGate;
	//public GameObject lowerGate;

    public GameObject rUpperGate;
    public GameObject lUpperGate;

    public GameObject rLowerGate;
    public GameObject lLowerGate;


    //public GameObject uGateStart;
	//public GameObject uGateEnd;

    public GameObject uRGateStart;
    public GameObject uLGateStart;
    public GameObject uRGateEnd;
    public GameObject uLGateEnd;


    //public GameObject lGateStart;
	//public GameObject lGateEnd;

    public GameObject lRGateStart;
    public GameObject lLGateStart;
    public GameObject lRGateEnd;
    public GameObject lLGateEnd;

    private bool uGateMove = false;
	private bool uGateMoveBack = false;
	private bool lGateMove = false;
	private bool lGateMoveBack = false;
	private bool uGateState = false;
	private bool lGateState = false;
    private bool eleTop;
    private bool onEle;
    private bool eleDone;

    // Use this for initialization
    void Start () {
        // bell sound gen
        elevatorSource = gameObject.GetComponent<AudioSource>();
        bellSounds = Resources.LoadAll<AudioClip>("SoundEffects/Elevator/Bell");
        bellSound = bellSounds[Random.Range(0, bellSounds.Length)];


        // This is the code for playing a sound for an event
        // switch clip
        // play
        /*
        elevatorSource.clip = bellSound;
        elevatorSource.Play();
        */

        // Elevator logic
        elePos = transform.position;

        eleXPos = elePos[0];
        eleYPos = elePos[1];
        eleZPos = elePos[2];

        eleTop = true;
        onEle = false;
        eleDone = true;
    }

	// Update is called once per frame
	void Update()
	{

		playerPos = playerC.transform.position;
		elePos = transform.position;


		currentDis = Vector3.Distance(playerPos, elePos);

		if (debug)
			Debug.Log("Ele Dist: " + currentDis);



		if (playerInElevator && powerOn)
		{
			onEle = true;
		}
		else
			onEle = false;

		if (eleTop && powerOn && !playerInElevator)
		{
			uGateMoveBack = false;
			uGateMove = true;
		}
		else if (eleTop && powerOn && playerInElevator) 
		{
			uGateMove = false;
			uGateMoveBack = true;
		}

		if (lGateMove)
		{
            //lowerGate.transform.position = Vector3.Slerp(lowerGate.transform.position, lGateEnd.transform.position, 0.5f * Time.deltaTime);
            lLowerGate.transform.position = Vector3.Slerp(lLowerGate.transform.position, lLGateEnd.transform.position, 0.5f * Time.deltaTime);
            rLowerGate.transform.position = Vector3.Slerp(rLowerGate.transform.position, lRGateEnd.transform.position, 0.5f * Time.deltaTime);
        }

		if (uGateMove)
		{
			//upperGate.transform.position = Vector3.Slerp(upperGate.transform.position, uGateEnd.transform.position, 0.5f * Time.deltaTime);
            lUpperGate.transform.position = Vector3.Lerp(lUpperGate.transform.position, uLGateEnd.transform.position, 0.5f * Time.deltaTime);
            rUpperGate.transform.position = Vector3.Lerp(rUpperGate.transform.position, uRGateEnd.transform.position, 0.5f * Time.deltaTime);
            uGateState = true;
		}

		if (uGateMoveBack)
		{
			//upperGate.transform.position = Vector3.Slerp(upperGate.transform.position, uGateStart.transform.position, 0.5f * Time.deltaTime);
            lUpperGate.transform.position = Vector3.Lerp(lUpperGate.transform.position, uLGateStart.transform.position, 0.5f * Time.deltaTime);
            rUpperGate.transform.position = Vector3.Lerp(rUpperGate.transform.position, uRGateStart.transform.position, 0.5f * Time.deltaTime);
            uGateState = false;
		}

		if (eleTop && onEle && !soundDone.GetComponent<AudioSource>().isPlaying && !uGateState)
		{
			eleYPos -= eleSpeed;
			transform.position = new Vector3(eleXPos, eleYPos, eleZPos);

			if (eleYPos <= eleMaxHeight)
			{
				eleYPos = eleMaxHeight;
				eleTop = false;

				if (transform.position.y < -10.5f)
				{
					lGateMove = true;
				}

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
        if (!bellSounded)
        {
            elevatorSource.clip = bellSound;
            elevatorSource.Play();
            bellSounded = true;
        }

    }

}
