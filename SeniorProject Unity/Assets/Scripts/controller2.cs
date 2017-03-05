using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class controller2 : MonoBehaviour
{
    public bool debug;

    Animator anim;

    // For triggering later
    static int HBListenHash = Animator.StringToHash("HBListen");

    // these are named after the actual layer in unity for the state machine
    static int closedStateHash = Animator.StringToHash("Base Layer.Closed");
    static int openStateHash = Animator.StringToHash("Base Layer.Opened");

    public GameObject playerVR;
    public GameObject playerNormal;
    private GameObject[] noteBooks;
    public GameObject heart;
    public GameObject notesPage;
    public GameObject hintsPage;

    public bool notebookLook;
    public bool HBListening;
	public bool playerSprint;
	public float walkSpeed;
	public float sprintSpeed;
    public float crouchSpeed;
    private float playerSpeed;

    public bool calibrationDone;


	public float gravity = 8.9f;

	public float speedH = 2.0f;
	public float speedV = 2.0f;
	public float yLookLimit = 70;

	private Vector3 playerPos;
	public bool crouch = false;
	public float crouchHeight = 1.0f;

	private float mYaw = 0.0f;
	private float mPitch = 0.0f;
	public Vector3 moveDirection = Vector3.zero;

	public CharacterController gController;
    

	// Use this for initialization
	void Start()
	{

        // Animation states for HBListening 
        // Initialize animator from placeholder arms
        anim = GameObject.FindGameObjectWithTag("arms").GetComponent<Animator>();

        // The controller identifies any notebooks in use either in VR or non-VR
        noteBooks = GameObject.FindGameObjectsWithTag("noteBook");

        // Every noteook starts open with this command
        foreach (GameObject n in noteBooks)
		{
            // If the notebooks don't start open, open them at the start of the game
            if(!n.activeSelf)
			    n.GetComponent<noteBook>().Open();
		}

        // Grab gController and it's speed states
        gController = GetComponent<CharacterController>();
		playerSpeed = walkSpeed;
		playerSprint = false;

        // Setting up camera to use (VR or no VR)
        if (VRSettings.enabled)
        {
            playerVR.SetActive(true);
            playerNormal.SetActive(false);
        }
        else
        {
            playerVR.SetActive(false);
            playerNormal.SetActive(true);
        }

        calibrationDone = false;
    }

	// Update is called once per frame
	void Update()
	{

        // Update Standing Position if Player is not crouched
        if (!crouch)
		{
			playerPos = transform.position;
		}

        // If player is sprinting set to sprintspeed
        if (playerSprint)
            playerSpeed = sprintSpeed;
        else if (crouch)
            playerSpeed = crouchSpeed;
        else
            playerSpeed = walkSpeed;




        // Without look restraints
        mYaw += speedH * Input.GetAxis("Mouse X");


        // Toggle look constraints for VR
        if(!VRSettings.enabled)
        {
            // With look restraints
            if ((mPitch <= yLookLimit) && (Input.GetAxis("Mouse Y") < 0))
                mPitch -= speedV * Input.GetAxis("Mouse Y");
            else if ((mPitch >= -yLookLimit) && (Input.GetAxis("Mouse Y") > 0))
                mPitch -= speedV * Input.GetAxis("Mouse Y");
        }

        transform.eulerAngles = new Vector3(mPitch, mYaw, 0.0f);

        if (debug)
        {
            Debug.Log("mPitch " + mPitch);
            Debug.Log("mYaw " + mYaw);
        }

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.x *= playerSpeed;
        moveDirection.z *= playerSpeed;

		// Alternate crouch state if you don't want the player to go from crouch to sprint
		//if (Input.GetButtonDown("Crouch") && !crouch && !playerSprint)

		//crouch state
		if (Input.GetButtonDown("Crouch") && !crouch)
		{
			if (playerSprint)
				playerSprint = false;
			
			//transform.position = new Vector3(transform.position[0], playerPos[1], transform.position[2]);
			gController.center = new Vector3(gController.center[0], -1 + crouchHeight, gController.center[2]);
			gController.height = 2.5f - crouchHeight;
			crouch = true;
		}

		//standing state
		else if(Input.GetButtonDown("Crouch") && crouch)
		{
			//transform.position = new Vector3(transform.position[0], playerPos[1], transform.position[2]);
			gController.center = new Vector3(gController.center[0], -1, gController.center[2]);
			gController.height = 2.5f;
			crouch = false;
		}

		/* A known bug is that crouch spamming can lead to poor updates of original standing Y pos 
			which can clip you through the ground depending on how high the player gravity is */
		// The error can probably be fixed by removing the attached rigid body's gravity

		/* Another known bug is being able to crouch onto objects higher than you */
		// This could be fixed by lowering the player as the center and height for the controller are adjusted

		//if(!gController.isGrounded)

		moveDirection.y -= gravity * Time.deltaTime;
		gController.Move(moveDirection * Time.deltaTime);


		// Toggle playerSprint & cannot sprint with crouched
		if (Input.GetButtonDown("Sprint") && !crouch)
			playerSprint = true;

		if (Input.GetButtonUp("Sprint"))
			playerSprint = false;

        // TEST
        if (Input.GetButton("VRup") && playerVR.transform.position.y < 1.75)
        {
            playerVR.transform.position += new Vector3(0.0f, 0.01f, 0.0f);
        }

        if (Input.GetButton("VRdown") && playerVR.transform.position.y > 1.15)
        {
            playerVR.transform.position -= new Vector3(0.0f, 0.01f, 0.0f);
        }

        // Heart Beat Listening
        if (Input.GetButton("HBListen"))
        {
            calibrationDone = true;

      
            // Use this variable to access in calibration
            HBListening = true;
            heart.GetComponent<heartBeatThump>().heartListening = true;

            // Remove the notes for HBListening
            notesPage.SetActive(false);
            hintsPage.SetActive(false);

            // If the player is holding the HBListen key the animation is triggered
            anim.SetBool("HBListen", true);
        }

        // NOT Heart Beat Listening
        else
        {
            // Use this variable to access in calibration
            HBListening = false;
            heart.GetComponent<heartBeatThump>().heartListening = false;

            // Remove the hints for HBListening
            notesPage.SetActive(true);
            hintsPage.SetActive(true);

            // If the player is holding the HBListen key the animation is triggered
            anim.SetBool("HBListen", false);
        }

        // Toggle Journal (Alpha)
        if (Input.GetButtonUp("NoteBook"))
        {
            noteBooks = GameObject.FindGameObjectsWithTag("noteBook");

            foreach (GameObject n in noteBooks)
            {
                n.GetComponent<noteBook>().Open();
            }      
        }
           

        // Toggle VR
        if (Input.GetButtonDown("VRToggle"))
        {
            VRSettings.enabled = !VRSettings.enabled;

            // Debug VR State
            if (debug)
            {
                Debug.Log("VR is " + VRSettings.enabled);
                Debug.Log("VR Camera is " + playerVR.activeSelf);
                Debug.Log("Normal Camera is " + playerNormal.activeSelf);
            }
        }


        // Toggle VR Controller Camera
        if (VRSettings.enabled)
        {
            playerVR.SetActive(true);
            playerNormal.SetActive(false);
        }
        else
        {
            playerVR.SetActive(false);
            playerNormal.SetActive(true);
        }


        if (Input.GetButtonDown("Cancel"))
			Application.Quit();
	}

}