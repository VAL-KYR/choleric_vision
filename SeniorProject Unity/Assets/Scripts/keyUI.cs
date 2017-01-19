using UnityEngine;
using System.Collections;

public class keyUI : MonoBehaviour {
    // Key audio compenent for all key sounds
    AudioSource keySource;

    // Key unlock sounds
    public AudioClip[] unlockSounds;
    public AudioClip unlockSound;
    private bool unlockSounded = false;

    // Key pickup sounds
    public AudioClip pickupSound;
    private bool pickedUp = false;

    public bool debug;
    public bool startDead;
    public bool keyGrabbed;

    public GameObject playerC;
    public GameObject KeyGO;
    public GameObject lockedObject;

    public float keyGrabbingDistance;
    public float doorDistance;

    private Vector3 playerPos;
    private Vector3 keyPos;
    private Vector3 doorPos;
    private float currentDis;

	public bool triggerOnPickup;
	public bool triggerOnUse;
	public string triggerTag;
	private bool triggerEvent;

	public bool enableOrDisableObject = false;
	public GameObject activeObject;



    // Use this for initialization
    void Start () {
        // key sound gen
        keySource = gameObject.GetComponent<AudioSource>();
        unlockSounds = Resources.LoadAll<AudioClip>("SoundEffects/Key/Unlock");
        unlockSound = unlockSounds[Random.Range(0, unlockSounds.Length)];

        unlockSounded = false;
        // This is the code for playing a sound for an event
        // switch clip
        // play
        /*
        keySource.clip = unlockSound;
        keySource.Play();
        */

        keySource = gameObject.GetComponent<AudioSource>();

        // key logic
        triggerEvent = false;
        keyGrabbed = false;
        doorPos = lockedObject.transform.position;

        playerC = GameObject.FindGameObjectWithTag("GameController");

    }
	
	// Update is called once per frame
	void Update () {
        
        // Start with key unspawned
        if (startDead)
        {
            gameObject.SetActive(false);
            startDead = false;
        }
            

        keyPos = KeyGO.transform.position;

		if(debug)
        	Debug.Log(gameObject + " distance: " + currentDis);

        if (keyGrabbed == false)
        {

            playerPos = playerC.transform.position;

            currentDis = Vector3.Distance(playerPos, keyPos);

            

			if ((currentDis < keyGrabbingDistance) && (Input.GetButtonDown("Action")) && lookingAtMe(true))
            {
                transform.position = new Vector3(100, 100, 100);
                keyGrabbed = true;

                keySource.clip = pickupSound;
                keySource.Play();

                // Flip Active state of despawnObject
                if (enableOrDisableObject)
				{
					if(!activeObject.activeSelf)
						activeObject.SetActive(true);
					if (activeObject.activeSelf)
						activeObject.SetActive(false);
				}

				// Trigger on key pickup 
				if (triggerOnPickup)
				{
					var go = GameObject.FindWithTag(triggerTag);
					go.GetComponent<soundTrigger>().arm();
				}
            }
        }

        else if (keyGrabbed == true)
        {
            playerPos = playerC.transform.position;


            currentDis = Vector3.Distance(playerPos, doorPos);

            if ((currentDis < doorDistance) && (Input.GetButtonDown("Action")))
            {
				if (debug)
					Debug.Log("Unlocked " + lockedObject);

				// This unlocks the door
				// I've created a door tag for later so we can choose what the key does and filter what it unlocks by tag
				if (lockedObject.CompareTag("door"))
					lockedObject.GetComponent<doorMaster>().unLock();

				if (lockedObject.CompareTag("documentCabinet"))
					lockedObject.GetComponent<documentCabinet>().unLock();

                // Playing Sound on object unlock
                if (!unlockSounded)
                {
                    keySource.clip = unlockSound;
                    keySource.Play();
                    unlockSounded = true;
                }

				// Trigger on key use
				if (triggerOnUse)
				{
					var go = GameObject.FindWithTag(triggerTag);
					go.GetComponent<soundTrigger>().arm();
				}

            }
        }
    }

	public bool lookingAtMe(bool lookingAt)
	{
		return lookingAt;
	}

}
