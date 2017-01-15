using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class doorAnimator : MonoBehaviour
{

	public Animator anim;

    // For Zone Flags
    public List<GameObject> flags;
    public int lessFlags = 0;

    // For triggering later
    static int doorInteractHash = Animator.StringToHash("doorInteract");

	// these are named after the actual layer in unity for the state machine
	static int closedStateHash = Animator.StringToHash("Base Layer.Closed");
	static int openStateHash = Animator.StringToHash("Base Layer.Opened");

	public bool debug;
	public bool doorLocked;
	public bool doorOpen;
	public bool interactSpace;

    private bool doorHasKey = false;

    private string lastDoorState;
    public AnimatorStateInfo doorState;

    // UI
    Renderer render;

    public GameObject ui;
    public GameObject uiAlt;
    private Text uiText;
    private Text uiTextAlt;


    // Use this for initialization
    void Start()
	{
		anim = GetComponent<Animator>();

        render = gameObject.GetComponent<Renderer>();
        uiText = ui.GetComponent<Text>();
        uiTextAlt = uiAlt.GetComponent<Text>();

        anim.SetBool("doorLocked", doorLocked);
		anim.SetBool("doorOpen", doorOpen);

        // Check for new keys that can unlock this door
        keySearch();
    }

	// Update is called once per frame
	void Update()
	{
        // accessing doorstate
        doorState = anim.GetCurrentAnimatorStateInfo(0);

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
            }
        }
        //

        // UI State
        if (!doorLocked)
        {
            if (!doorOpen)
            {
                uiText.text = "Press A to open door";
                uiTextAlt.text = "Press A to open door";
            }
            else
            {
                uiText.text = "Press A to close door";
                uiTextAlt.text = "Press A to close door";
            }

            // closed and open states for animation
            if (doorState.fullPathHash == closedStateHash)
            {
                if (debug)
                    Debug.Log(gameObject + " State is closed");

                doorOpen = false;
                anim.SetBool("doorOpen", doorOpen);
            }
        }

        else
        {
            // Door is locked and can be opened
            if (doorHasKey)
            {
                uiText.text = "Door locked";
                uiTextAlt.text = "Door locked";
            }

            // Door cannot be opened
            else
            {
                uiText.text = "Lock Broken";
                uiTextAlt.text = "Lock Broken";
            }
        }
        

		// We remove UI if the player walks out of the trigger
		if (!interactSpace)
		{
			uiText.enabled = false;
			uiTextAlt.enabled = false;
		}

        
        if (doorState.fullPathHash == openStateHash)
		{
			if (debug)
				Debug.Log(gameObject + " State is open");

			doorOpen = true;
			anim.SetBool("doorOpen", doorOpen);
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

	public void lookingAtMe(float lookAtDist)
	{
        // We give a GUI queue here on the door mesh
        if (interactSpace)
        {
            if (debug)
                Debug.Log("Text Queue " + uiText + " " + uiTextAlt);
            uiText.enabled = true;
            uiTextAlt.enabled = true;

            if (Input.GetButtonDown("Action"))
            {
                // Action either unlocks
                // This should work when we fix the rapid fire action issues
                anim.SetBool("doorLocked", doorLocked);

                // If the doorLocked SetBool earlier changed to false then this Interact should unlock the door
                anim.SetTrigger(doorInteractHash);
            }
        }

        else
        {
            uiText.enabled = false;
            uiTextAlt.enabled = false;
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
		doorOpen = true;
		anim.SetBool("doorOpen", doorOpen);
    }

    public void forceClosed()
    {
        anim.SetTrigger(doorInteractHash);
        doorOpen = false;
        anim.SetBool("doorOpen", doorOpen);
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
                    doorHasKey = true;
                }
            }
        }
    }
}
