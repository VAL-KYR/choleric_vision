using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class tapeMaster : MonoBehaviour
{
	// Animation Jazz
	Animator anim;

	static int interactHash = Animator.StringToHash("interact");

	// these are named after the actual layer in unity for the state machine
	static int rNothingStateHash = Animator.StringToHash("R_Reel.nothing");
	static int rOnStateHash = Animator.StringToHash("R_Reel.onTape");
	static int rOffStateHash = Animator.StringToHash("R_Reel.offTape");
	static int rSpinStateHash = Animator.StringToHash("R_Reel.reelSpin");

    static int lNothingStateHash = Animator.StringToHash("L_Reel.nothing");
    static int lOnStateHash = Animator.StringToHash("L_Reel.onTape");
    static int lOffStateHash = Animator.StringToHash("L_Reel.offTape");
    static int lSpinStateHash = Animator.StringToHash("L_Reel.reelSpin");

    AnimatorStateInfo rTapeState;
    AnimatorStateInfo lTapeState;

    // Regluar Jazz
    public bool debug;

	public bool tapeActive = false;
	public bool tapePlaying = false;

	Renderer render;
	public GameObject ui;
	private Text uiText;
	public float interactDistance = 3.0f;
	public bool playQueue;
	public bool remoteOn = false;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();

		remoteOn = false;
		playQueue = false;
		render = gameObject.GetComponent<Renderer>();
		uiText = ui.GetComponent<Text>();

		anim.SetBool("tapeActive", tapeActive);
		anim.SetBool("tapePlaying", tapePlaying);
	}

	void Update()
	{
		rTapeState = anim.GetCurrentAnimatorStateInfo(0);
        lTapeState = anim.GetCurrentAnimatorStateInfo(1);

        //play queuing
        if (playQueue && (rTapeState.fullPathHash == rSpinStateHash && lTapeState.fullPathHash == lSpinStateHash))
		{
			gameObject.GetComponent<AudioSource>().Play();
			anim.SetBool("tapePlaying", true);
            uiText.text = "Press A to Stop Recording";
			playQueue = false;
			//remoteOn = false;
		}

		//If tape is done change playing state
		else if (!playQueue && (rTapeState.fullPathHash == rNothingStateHash || lTapeState.fullPathHash == lNothingStateHash))
		{
            uiText.text = "Press A to Start Recording";
            gameObject.GetComponent<AudioSource>().Stop();
		}

		//If the tape is done turn it off
		else if (!gameObject.GetComponent<AudioSource>().isPlaying)
		{
			anim.SetBool("tapePlaying", false);
			gameObject.GetComponent<AudioSource>().Stop();
        }

        // Fix for stubborn anim layer
        if((playQueue && (rTapeState.fullPathHash == rSpinStateHash ^ lTapeState.fullPathHash == lSpinStateHash)))
        {
            anim.SetTrigger(interactHash);
        }

        if ((!gameObject.GetComponent<AudioSource>().isPlaying && (rTapeState.fullPathHash == rSpinStateHash ^ lTapeState.fullPathHash == lSpinStateHash)))
        {
            anim.SetBool("tapePlaying", false);
        }
        //

    }

	public void lookingAtMe(float lookAtDist)
	{

		if (lookAtDist <= interactDistance)
		{
			uiText.enabled = true;
		}
		else
		{
			uiText.enabled = false;
		}

		if (Input.GetButtonDown("Action") && lookAtDist <= interactDistance)
		{
			//if the tape is spinning
			if ((rTapeState.fullPathHash == rSpinStateHash || lTapeState.fullPathHash == lSpinStateHash) && gameObject.GetComponent<AudioSource>().isPlaying)
			{
				anim.SetBool("tapePlaying", false);
				playQueue = false;
                anim.ResetTrigger(interactHash);
			}

			//if nothing is going on a queue is empty
			else if ((rTapeState.fullPathHash == rNothingStateHash && lTapeState.fullPathHash == lNothingStateHash) && !gameObject.GetComponent<AudioSource>().isPlaying)
			{
				anim.SetTrigger(interactHash);
				playQueue = true;
			}
		}

		if (debug)
			Debug.Log("Looking At Recorder " + gameObject);
	}

	public void remote()
	{
		anim.SetTrigger(interactHash);
		playQueue = true;
	}
    
}
