using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class tapeRecorder : MonoBehaviour
{
	// Animation Jazz
	Animator anim;

	static int interactHash = Animator.StringToHash("interact");

	// these are named after the actual layer in unity for the state machine
	static int nothingStateHash = Animator.StringToHash("Base Layer.nothing");
	static int onStateHash = Animator.StringToHash("Base Layer.onTape");
	static int offStateHash = Animator.StringToHash("Base Layer.offTape");
	static int spinStateHash = Animator.StringToHash("Base Layer.reelSpin");

	AnimatorStateInfo tapeState;

	// Regluar Jazz
	public bool debug;

	// Coliider method for interaction distance
	/*
	public bool interactSpace;
	*/

	public bool tapeActive = false;
	public bool tapePlaying = false;

	Renderer render;
	public GameObject ui;
	private Text uiText;
	public float interactDistance = 3.0f;
	public bool playQueue;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();

		// Coliider method for interaction distance
		/*
		interactSpace = false;
		*/

		playQueue = false;
		render = gameObject.GetComponent<Renderer>();
		uiText = ui.GetComponent<Text>();

		anim.SetBool("tapeActive", tapeActive);
		anim.SetBool("tapePlaying", tapePlaying);
	}

	void Update()
	{
		tapeState = anim.GetCurrentAnimatorStateInfo(0);

		//play queuing
		if (playQueue && tapeState.fullPathHash == spinStateHash)
		{
			gameObject.GetComponent<AudioSource>().Play();
			anim.SetBool("tapePlaying", true);
			playQueue = false;
		}

		//If tape is done change playing state
		else if (!playQueue && tapeState.fullPathHash == nothingStateHash)
		{
			gameObject.GetComponent<AudioSource>().Stop();
		}

		//If the tape is done turn it off
		else if (!gameObject.GetComponent<AudioSource>().isPlaying)
		{
			anim.SetBool("tapePlaying", false);
			gameObject.GetComponent<AudioSource>().Stop();
		}



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
			if (tapeState.fullPathHash == spinStateHash && gameObject.GetComponent<AudioSource>().isPlaying)
			{
				anim.SetBool("tapePlaying", false);
				playQueue = false;
			}

			//if nothing is going on a queue is empty
			else if (tapeState.fullPathHash == nothingStateHash && !gameObject.GetComponent<AudioSource>().isPlaying)
			{
				anim.SetTrigger(interactHash);
				playQueue = true;
			}

		}

		if (debug)
			Debug.Log("Looking At Recorder " + gameObject);
	}

	// Coliider method for interaction distance
	/*
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("GameController"))
		{
			interactSpace = true;
			if (debug)
				Debug.Log("Entered door interactSpace " + gameObject);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("GameController"))
		{
			interactSpace = false;
			if (debug)
				Debug.Log("Exited door interactSpace " + gameObject);
		}
	}
	*/
}
