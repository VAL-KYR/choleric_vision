using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class doorMaster : MonoBehaviour
{
    public GameObject lDoorSound;
    private AudioSource lDoorSounder;
    public GameObject rDoorSound;
    private AudioSource rDoorSounder;

    public AudioClip[] lockSounds;
    public AudioClip lockSound;
    public bool lockReset = false;

    public AudioClip[] openSounds;
    public AudioClip openSound;
    public bool openReset = false;

    public AudioClip[] closeSounds;
    public AudioClip closeSound;
    public bool closedReset = false;


    Animator anim;

    // For Zone Flags
    public List<GameObject> flags;
    public int lessFlags = 0;

    // For triggering later
    static int doorInteractHash = Animator.StringToHash("doorInteract");

	// these are named after the actual layer in unity for the state machine
	static int leftClosedStateHash = Animator.StringToHash("Left Door.Closed");
	static int leftOpenStateHash = Animator.StringToHash("Left Door.Opened");
    static int rightClosedStateHash = Animator.StringToHash("Right Door.Closed");
    static int rightOpenStateHash = Animator.StringToHash("Right Door.Opened");

    public bool debug;
	public bool doorLocked;
    public bool doorOpen;
	public bool interactSpace;

    public bool doorHasKey = false;

    private string lastDoorState;
    public AnimatorStateInfo doorState;

    // Use this for initialization
    void Start()
	{
        // Sound
        lDoorSounder = lDoorSound.GetComponent<AudioSource>();
        rDoorSounder = rDoorSound.GetComponent<AudioSource>();
        lockSounds = Resources.LoadAll<AudioClip>("SoundEffects/Door/Locked");
        openSounds = Resources.LoadAll<AudioClip>("SoundEffects/Door/Open");
        closeSounds = Resources.LoadAll<AudioClip>("SoundEffects/Door/Close");
        lockSound = lockSounds[Random.Range(0, lockSounds.Length)];
        openSound = openSounds[Random.Range(0, openSounds.Length)];
        closeSound = openSounds[Random.Range(0, closeSounds.Length)];

        // Animation
        anim = gameObject.GetComponent<Animator>();

        anim.SetBool("doorLocked", doorLocked);
        anim.SetBool("doorOpen", doorOpen);

        // Logic
        // Check for new keys that can unlock this door
        keySearch();
    }

	// Update is called once per frame
	void Update()
	{
        doorState = anim.GetCurrentAnimatorStateInfo(0);

        if (doorState.fullPathHash == leftClosedStateHash)
        {
            doorOpen = false;
            anim.SetBool("doorOpen", doorOpen);
        }

        else if(doorState.fullPathHash == leftOpenStateHash)
        {
            doorOpen = true;
            anim.SetBool("doorOpen", doorOpen);     
        }


        // Flags trigger forced open door
        if (flags.Count > 0 && !anim.GetBool("doorOpen") && anim.GetBool("doorLocked"))
        {

            int flagsConfirmed = 0;

            for (int t = 0; t < flags.Count; t++)
            {
                if (flags[t].GetComponent<playerZoneFlag>().playerFlag)
                {
                    flagsConfirmed++;
                }
            }

            if (flagsConfirmed >= (flags.Count - lessFlags))
            {
                unLock();
                forceOpen();

                // Needed because stubborn doors are being stubborn (animation synch error)
                //forceOpen();

                //forceOpen();
                //
            }
        }

    }

    public void OnTriggerEnter()
	{
		interactSpace = true;
		if (debug)
			Debug.Log("Entered door interactSpace " + gameObject);
	}

	public void OnTriggerExit()
	{
		interactSpace = false;
		if (debug)
			Debug.Log("Exited door interactSpace " + gameObject);
	}

	public void lookingAtMe()
	{
        // We give a GUI queue here on the door mesh
        if (interactSpace)
        {
            if (Input.GetButtonDown("Action"))
            {
                if (debug)
                    Debug.Log("sending interact to " + anim.name);

                if (doorLocked)
                {
                    if (!doorHasKey)
                    {
                        // UI queue for later that the door is broken
                        if (lDoorSound.activeSelf)
                        {
                            lDoorSounder.clip = lockSounds[Random.Range(0, lockSounds.Length)];
                            lDoorSounder.Play();
                        }
                        if (rDoorSound.activeSelf)
                        {
                            rDoorSounder.clip = lockSounds[Random.Range(0, lockSounds.Length)];
                            rDoorSounder.Play();
                        }

                    }

                    // UI queue for later that it needs a key
                    if (lDoorSound.activeSelf)
                    {
                        lDoorSounder.clip = lockSounds[Random.Range(0, lockSounds.Length)];
                        lDoorSounder.Play();
                    }
                    if (rDoorSound.activeSelf)
                    {
                        rDoorSounder.clip = lockSounds[Random.Range(0, lockSounds.Length)];
                        rDoorSounder.Play();
                    }
                }

                else
                {
                    if (doorState.fullPathHash == leftClosedStateHash)
                    {
                        if (lDoorSound.activeSelf)
                        {
                            lDoorSounder.clip = openSounds[Random.Range(0, openSounds.Length)];
                            lDoorSounder.Play();
                        }
                        if (rDoorSound.activeSelf)
                        {
                            rDoorSounder.clip = openSounds[Random.Range(0, openSounds.Length)];
                            rDoorSounder.Play();
                        }
                    }
                    else if(doorState.fullPathHash == leftOpenStateHash)
                    {
                        if (lDoorSound.activeSelf)
                        {
                            lDoorSounder.clip = closeSounds[Random.Range(0, closeSounds.Length)];
                            lDoorSounder.Play();
                        }
                        if (rDoorSound.activeSelf)
                        {
                            rDoorSounder.clip = closeSounds[Random.Range(0, closeSounds.Length)];
                            rDoorSounder.Play();
                        }
                    }
                }

                anim.SetTrigger(doorInteractHash);
            }
        }

		if (debug)
			Debug.Log("Looking At Door " + gameObject);
	}

	public void unLock()
	{
		doorLocked = false;
        anim.SetBool("doorLocked", doorLocked);
    }

    public void forceLock()
    {
        doorLocked = true;
        anim.SetBool("doorLocked", doorLocked);
    }

    public void forceOpen()
	{
        anim.SetTrigger(doorInteractHash);
    }

    public void forceClosed()
    {
        anim.SetTrigger(doorInteractHash);
    }

    public void keySearch()
    {
        // Search for Keys
        foreach (GameObject key in GameObject.FindGameObjectsWithTag("key"))
        {
            // if you've found that this door has a key don't run any more checks
            if (!doorHasKey)
            {
                // check if the key's unlock object matches this door
                if (key.GetComponent<keyUI>().lockedObject == gameObject)
                {
                    if (debug)
                        Debug.Log("I have a key " + doorHasKey + " this key " + key);
                    doorHasKey = true;
                }
            }
        }
    }

}
