using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class noteBook : MonoBehaviour {

    Animator anim;

    // For triggering later
    static int bookOpenHash = Animator.StringToHash("OpenClosed");

    // these are named after the actual layer in unity for the state machine
    static int closedStateHash = Animator.StringToHash("Base Layer.Closed");
    static int openStateHash = Animator.StringToHash("Base Layer.Opened");


    public GameObject notes;
    public GameObject hints;

    private Text notesText;
    private Text hintsText;
    public int notesOverflow = 2;
    public int hintsOverflow = 2;

    public bool bookOpen = true;

    public List<string> currentNotes = new List<string>();
    public List<string> currentHints = new List<string>();

    // Use this for initialization
    void Start () {

        bookOpen = true;

        // Initialize animator from placeholder arms
        anim = GameObject.FindGameObjectWithTag("arms").GetComponent<Animator>();

        // Initialize state of animation based on notes object active state 
        anim.SetBool("OpenClosed", bookOpen);

        // Initialize text from files specified
        notesText = notes.GetComponent<Text>();
        hintsText = hints.GetComponent<Text>();

        // Starting text for notes and hints
        currentNotes.Add("Notes: ");
        currentHints.Add("Controls: ");
		currentHints.Add("Hold (RB) to listen to your heartbeat.");
        currentHints.Add("Hit (X) to open/close your Journal.");
        currentHints.Add("Hit (Y) to turn your flashlight on/off.");
        currentHints.Add("Hit (A) to interact with objects.");
        currentHints.Add("Hit (B) to toggle crouch/standing.");
        currentHints.Add("Press in the (left stick) to run");
        currentHints.Add("(RTrigger/LTrigger) to lean right and left");
        if (hintsText)
        {
            hintsText.text = string.Join("\n", currentHints.ToArray());
        }
       
    }
	
	// Update is called once per frame
	void Update () {


        /*
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick 1 button " + i) || Input.GetAxis("LLean") > 0 || Input.GetAxis("RLean") < 0)
            {
                Debug.Log("joystick 1 button " + i);
                Debug.Log("joystick used");
            }
        }
        /////////////////////////////////////////////// FIX LATER ///////////////////////////////////////////////////////////////////////////////////////////
        */

        // If the notes text objects are active then animate the arms to open or close
        anim.SetBool("OpenClosed", bookOpen);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Opened")  && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
        {
            notes.SetActive(true);
            hints.SetActive(true);
        }

    }

    // add text from tutorial triggers
    public void AddTutText(string note, string hint)
    {
		// Add the new note
        currentNotes.Add(note);
        notesText.text = string.Join("\n", currentNotes.ToArray());

		// Remove null notes
		int noteCountN = currentNotes.Count;

		for (int s = 0; s < noteCountN; s++)
		{
			if (currentNotes[s] == null)
			{
				currentNotes.RemoveAt(s);
				noteCountN--;
			}
		}

		// Remove Note Overflow
		if (currentNotes.Count > notesOverflow)
		{
			currentNotes.RemoveAt(1);
		}


        // Add new hint
        /*
		currentHints.Add(hint);
        hintsText.text = string.Join("\n", currentHints.ToArray());

		// Remove null hints
		int hintCountN = currentHints.Count;

        
		for (int s = 0; s < hintCountN; s++) 
		{
			if (currentHints[s] == null)
			{
				currentHints.RemoveAt(s);
				hintCountN--;
			}
		}
        

		// Remove Hint Overflow
		if (currentHints.Count > hintsOverflow)
		{
			currentHints.RemoveAt(1);
		}
        */
    }

    // remove text command
    public void RemoveTutText(int index, string type)
    {
        if (type == "note")
            currentNotes.RemoveAt(index);
        //if (type == "hint")
            //currentHints.RemoveAt(index);
    }

    // open or close noteBook command
    // Animation queues go here
    public void Open()
    {
        bookOpen = true;
       
        
    }

    public void Close()
    {
        bookOpen = false;
        notes.SetActive(false);
        hints.SetActive(false);
    }
}
