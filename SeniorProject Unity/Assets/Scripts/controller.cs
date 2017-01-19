 using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class controller : MonoBehaviour
{
    public bool debug;


    [System.Serializable]
    public class PlayerSpeedGroup
    {
        public float walkSpeed;
        public float sprintSpeed;
        public float crouchSpeed;
        public float speedH = 2.0f;
        public float speedV = 2.0f;
    }
    public PlayerSpeedGroup playerSpeedGroup = new PlayerSpeedGroup();

    [System.Serializable]
    public class CamerasGroup
    {
        //Cameras and joint
        public GameObject playerVR;
        public GameObject playerNormal;
        
    }
    public CamerasGroup camerasgroup = new CamerasGroup();

    private GameObject headJoint;

    //Player Positions
    private float yLookLimit = 70;
    private Vector3 prevPlayerPos;
    private Vector3 prevPlayerRot;
    private Vector3 playerPos;
    private Vector3 oriPlayerScale;
    private float mYaw = 0.0f;
    private float mPitch = 0.0f;
    public Vector3 moveDirection = Vector3.zero;
    private GameObject ground;
    private float headDistance;
    private float crouchDiffeance;
    private float rotSpeed;
    private Rigidbody rb;

    //Player Controls
    public bool playerSprint;
    private float playerHeight;
    private float playerSpeed;
    public float gravity;
    public bool crouch = false;
    private float crouchHeight;
    public CharacterController gController;
    private CapsuleCollider contCollider;
    private Vector3 originalCamPlacement;

    //Heartbeat Variables
    public GameObject heart;
    public bool HBListening;
    public bool calibrationDone;
    static int HBListenHash = Animator.StringToHash("HBListen"); // For triggering later

    //Notebook Variables 
    private GameObject[] noteBooks;    
    public GameObject notesPage;
    public GameObject hintsPage;
    public bool notebookLook;
    Animator anim;
    // these are named after the actual layer in unity for the state machine
    static int closedStateHash = Animator.StringToHash("Base Layer.Closed");
    static int openStateHash = Animator.StringToHash("Base Layer.Opened");


    // Use this for initialization
    void Start()
	{



        //Find Gameobjects or componants
        camerasgroup.playerVR = GameObject.FindGameObjectWithTag("VRCam");
        camerasgroup.playerNormal = GameObject.FindGameObjectWithTag("NonVRCam");
        headJoint = GameObject.FindGameObjectWithTag("NeckJoint");
        ground = GameObject.FindGameObjectWithTag("playerBottom");
        contCollider = GetComponent<CapsuleCollider>();
        gController = GetComponent<CharacterController>();        
        noteBooks = GameObject.FindGameObjectsWithTag("noteBook"); // The controller identifies any notebooks in use either in VR or non-VR
        anim = GameObject.FindGameObjectWithTag("arms").GetComponent<Animator>(); // Initialize animator from placeholder arms
        rb = GetComponent<Rigidbody>();

        // Every noteook starts open with this command
        foreach (GameObject n in noteBooks)
        {
            // If the notebooks don't start open, open them at the start of the game
            if (!n.activeSelf)
                n.GetComponent<noteBook>().OpenClose();
        }

        //Set variablets
        calibrationDone = false;
        playerSpeed = playerSpeedGroup.walkSpeed;// Grab gController and it's speed states
        playerSprint = false;// Grab gController and it's speed states
        oriPlayerScale = transform.localScale;
        playerHeight = oriPlayerScale[1];
        headDistance = Vector3.Distance(ground.transform.position, headJoint.transform.position);
        crouchHeight = (oriPlayerScale[1] * 0.6f);
        crouchDiffeance = headDistance - crouchHeight;
        rotSpeed = 0.0f;

        // Setting up camera to use (VR or no VR)
        if (VRSettings.enabled)
        {
            camerasgroup.playerVR.SetActive(true);
            camerasgroup.playerNormal.SetActive(false);
        }
        else
        {
            camerasgroup.playerVR.SetActive(false);
            camerasgroup.playerNormal.SetActive(true);
        }

        
    }

	// Update is called once per frame
	void Update()
	{

        //print("First:" + transform.position);

        

        // Update Standing Position if Player is not crouched
        if (!crouch)
        {
            playerPos = transform.position;
        }

        // If player is sprinting set to sprintspeed
        if (playerSprint)
            playerSpeed = playerSpeedGroup.sprintSpeed;
        else if (crouch)
            playerSpeed = playerSpeedGroup.crouchSpeed;
        else
            playerSpeed = playerSpeedGroup.walkSpeed;



        //-------------------------------------------------------------------------------Ask Chris about this
        // Without look restraints
        mYaw += playerSpeedGroup.speedH * Input.GetAxis("Mouse X");




        // Toggle look constraints for VR
        if (!VRSettings.enabled)
        {
            // With look restraints
            if ((mPitch <= yLookLimit) && (Input.GetAxis("Mouse Y") < 0))
                mPitch -= playerSpeedGroup.speedV * Input.GetAxis("Mouse Y");
            else if ((mPitch >= -yLookLimit) && (Input.GetAxis("Mouse Y") > 0))
                mPitch -= playerSpeedGroup.speedV * Input.GetAxis("Mouse Y");
        }


        //transform.eulerAngles = new Vector3(mPitch, mYaw, 0.0f);

        if (VRSettings.enabled)
        {
            transform.eulerAngles = new Vector3(0.0f, mYaw, 0.0f);
        }
        if (!VRSettings.enabled)
        {
            transform.eulerAngles = new Vector3 (0.0f, mYaw, 0.0f);
            camerasgroup.playerNormal.transform.eulerAngles = new Vector3(mPitch, mYaw, 0.0f);
        }

        if (debug)
        {
            Debug.Log("mPitch " + mPitch);
            Debug.Log("mYaw " + mYaw);
            Debug.Log("Vertical " + Input.GetAxis("Vertical"));
            Debug.Log("Horizontal " + Input.GetAxis("Horizontal"));
            Debug.Log(moveDirection.x + "moveDirection.x");
            Debug.Log(moveDirection.y + "moveDirection.y");
            Debug.Log(moveDirection.z + "moveDirection.z");
        }


        //transform.Rotate(0, Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime, 0);
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //moveDirection = Vector3.forward * Input.GetAxis("Vertical");
        //moveDirection = Vector3.left * Input.GetAxis("Horizontal");
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.x *= playerSpeed;
        moveDirection.z *= playerSpeed;
        /*float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");*/

        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        


        //crouch state
        if (Input.GetButtonDown("Crouch") && !crouch)
		{
			if (playerSprint)
				playerSprint = false;

            headJoint.transform.position -= new Vector3(0.0f, crouchDiffeance, 0.0f);

            crouch = true;
        }

        else if(Input.GetButtonDown("Crouch") && crouch)
        {
            headJoint.transform.position += new Vector3(0.0f, crouchDiffeance, 0.0f);

            crouch = false;
        }

        /* A known bug is that crouch spamming can lead to poor updates of original standing Y pos 
			which can clip you through the ground depending on how high the player gravity is */
        // The error can probably be fixed by removing the attached rigid body's gravity

        /* Another known bug is being able to crouch onto objects higher than you */
        // This could be fixed by lowering the player as the center and height for the controller are adjusted

        moveDirection.y -= gravity * Time.deltaTime;
        gController.Move(moveDirection * Time.deltaTime);
        //transform.Translate(moveDirection[0], 0.0f, moveDirection[2]);

        //rb.AddForce(movement * playerSpeed);


        // Toggle playerSprint & cannot sprint with crouched
        if (Input.GetButtonDown("Sprint") && !crouch)
			playerSprint = true;

		if (Input.GetButtonUp("Sprint"))
			playerSprint = false;


        // TEST   -  Not needed with new code fix, but will keep just incase
        /*if (Input.GetButton("VRup") && playerVR.transform.position.y < 1.75)
        {
            playerVR.transform.position += new Vector3(0.0f, 0.01f, 0.0f);
        }

        if (Input.GetButton("VRdown") && playerVR.transform.position.y > 1.15)
        {
            playerVR.transform.position -= new Vector3(0.0f, 0.01f, 0.0f);
        }*/


        

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
                n.GetComponent<noteBook>().OpenClose();
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
                Debug.Log("VR Camera is " + camerasgroup.playerVR.activeSelf);
                Debug.Log("Normal Camera is " + camerasgroup.playerNormal.activeSelf);
            }
        }


        

        // Toggle VR Controller Camera
        if (VRSettings.enabled)
        {
            camerasgroup.playerVR.SetActive(true);
            camerasgroup.playerNormal.SetActive(false);
        }
        else
        {
            camerasgroup.playerVR.SetActive(false);
            camerasgroup.playerNormal.SetActive(true);
        }


        if (Input.GetButtonDown("Cancel"))
			Application.Quit();

        


        if (VRSettings.enabled)
        {

            float camY = camerasgroup.playerVR.transform.rotation.eulerAngles.y;

            if (Input.GetButtonDown("VRLookReset"))
            {                

                    transform.eulerAngles += new Vector3(transform.eulerAngles.x, camY, transform.eulerAngles.z);

                    UnityEngine.VR.InputTracking.Recenter();
                
            }
        }


        //print("Last:" + transform.position);


    }


}