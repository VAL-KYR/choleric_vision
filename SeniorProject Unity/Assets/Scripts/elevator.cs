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

        if (eleTop == true && onEle == true && !soundDone.GetComponent<AudioSource>().isPlaying)
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
        if (!bellSounded)
        {
            elevatorSource.clip = bellSound;
            elevatorSource.Play();
            bellSounded = true;
        }

    }

}
