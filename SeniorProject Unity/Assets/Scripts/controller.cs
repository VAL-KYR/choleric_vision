using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class controller : MonoBehaviour
{
	CursorLockMode wantedMode;

    private float quitTime = 0.0f;
    public bool debug;
    public float playerHealth = 100.0f;

    [System.Serializable]
    public class PlayerSpawns
    {
        public GameObject[] spawns;
        public int spawnChoose;
    }
    public PlayerSpawns playerSpawn = new PlayerSpawns();

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
    public class PlayerLean
    {
        public GameObject leanObject;
        public float maxRight = -40.0f;
        public float maxLeft = 40.0f;
        public float leanSpeed = 1.0f;
        public float leanReturnSpeed = 3.0f;
        public float lEase = 0;
        public float rEase = 0;
		public bool keyboard = true;
		public bool controller = false;
    }
    public PlayerLean playerLean = new PlayerLean();

    [System.Serializable]
    public class PlayerSpeech
    {
        public AudioSource voice;
        public AudioClip[] hitSounds;
        public AudioClip[] deathSounds;
        //public AudioClip hitSound;
    }
    public PlayerSpeech playerSpeech = new PlayerSpeech();

    [System.Serializable]
    public class CamerasGroup
    {
        //Cameras and joint
        public GameObject playerVR;
        public GameObject playerNormal;
        
    }
    public CamerasGroup camerasgroup = new CamerasGroup();

    private GameObject headJoint;

    // Player Death Effects
    GameObject vrCam;
    GameObject nonVrCam;

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
    public float playerMaxSpeed;
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
    public GameObject noteBookGO;
    // these are named after the actual layer in unity for the state machine
    static int closedStateHash = Animator.StringToHash("Base Layer.Closed");
    static int openStateHash = Animator.StringToHash("Base Layer.Opened");

    public bool holdObj;

    /// HEAD NOTEBOOK ADJUSTMENT CODE (ERICA)
    private bool vrBodCal;
    /// HEAD NOTEBOOK ADJUSTMENT CODE (ERICA)

    // Use this for initialization
    void Start()
	{

		// lcok cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = (CursorLockMode.Locked != wantedMode);

        // Player Death Effects
        if (!VRSettings.enabled)
        {
            nonVrCam = GameObject.FindGameObjectWithTag("NonVRCam");
        }
        else
        {
            vrCam = GameObject.FindGameObjectWithTag("VRCam");
        }

        /// Place the player at a spawn of choice ///
        playerSpawn.spawns = GameObject.FindGameObjectsWithTag("PlayerSpawns");
        gameObject.transform.position = playerSpawn.spawns[playerSpawn.spawnChoose].transform.position;

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
        playerSpeech.voice = GetComponent<AudioSource>();


        /// loading sounds for player
        playerSpeech.hitSounds = Resources.LoadAll<AudioClip>("Player/HitSounds");
        playerSpeech.deathSounds = Resources.LoadAll<AudioClip>("Player/DeathSounds");
        //playerSpeech.hitSound

        playerMaxSpeed = playerSpeedGroup.sprintSpeed;

        /// HEAD NOTEBOOK ADJUSTMENT CODE (ERICA)

        if (VRSettings.enabled)
        {
            vrBodCal = false;
        }
        else
        {
            nonVrCam.GetComponent<Camera>().fieldOfView = 70.0f;
        }
        
        /// HEAD NOTEBOOK ADJUSTMENT CODE (ERICA)

        // Every noteook starts open with this command
        foreach (GameObject n in noteBooks)
        {
            // If the notebooks don't start open, open them at the start of the game
            if (!n.activeSelf)
                n.GetComponent<noteBook>().Open();
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
        /// HEAD NOTEBOOK ADJUSTMENT CODE (ERICA)       
        if (VRSettings.enabled && !vrBodCal)
        {

            vrBodCal = true;

            //headJoint.transform.position += new Vector3(0.0f, 0.0f, -0.5f);
            Debug.Log("head join pos " + headJoint.transform.position);

        }
        /// HEAD NOTEBOOK ADJUSTMENT CODE (ERICA)

        if (!VRSettings.enabled)
        {
            if (!nonVrCam)
                nonVrCam = GameObject.FindGameObjectWithTag("NonVRCam");

            nonVrCam.GetComponent<Camera>().fieldOfView = 70.0f;
        }
        else
        {
            if (!vrCam)
                vrCam = GameObject.FindGameObjectWithTag("VRCam");
        }

        // Player Damage Effects
        Effects();

        // Update Standing Position if Player is not crouched
        if (!crouch)
        {
            playerPos = transform.position;
        }

        /// If player is sprinting set to sprintspeed
        if(playerHealth >= 100)
        {
            if (playerSprint)
            {
                playerSpeed = playerSpeedGroup.sprintSpeed;
                playerMaxSpeed = playerSpeedGroup.sprintSpeed;
            }
            else if (crouch)
                playerSpeed = playerSpeedGroup.crouchSpeed;
            else
                playerSpeed = playerSpeedGroup.walkSpeed;

            /// BLEED
            nonVrCam.GetComponent<BleedBehavior>().minBloodAmount = 0.0f;
        }
        else if (playerHealth <= 50)
        {
            if (playerSprint)
            {
                playerSpeed = playerSpeedGroup.sprintSpeed / 1.4f;
                playerMaxSpeed = playerSpeedGroup.sprintSpeed / 1.4f;
            }
            else if (crouch)
                playerSpeed = playerSpeedGroup.crouchSpeed / 1.4f;
            else
                playerSpeed = playerSpeedGroup.walkSpeed / 1.4f;

            /// BLEED
            nonVrCam.GetComponent<BleedBehavior>().minBloodAmount = 0.25f;
        }
        else if (playerHealth <= 0)
        {
            if (playerSprint)
            {
                playerSpeed = playerSpeedGroup.sprintSpeed / 1.7f;
                playerMaxSpeed = playerSpeedGroup.sprintSpeed / 1.7f;
            }
            else if (crouch)
                playerSpeed = playerSpeedGroup.crouchSpeed / 1.7f;
            else
                playerSpeed = playerSpeedGroup.walkSpeed / 1.7f;

            /// BLEED
            nonVrCam.GetComponent<BleedBehavior>().minBloodAmount = 0.5f;
        }
        ///

        if (!holdObj)
        {

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

            if (VRSettings.enabled)
            {
                transform.eulerAngles = new Vector3(0.0f, mYaw, 0.0f);
            }
            if (!VRSettings.enabled)
            {
                transform.eulerAngles = new Vector3(0.0f, mYaw, 0.0f);
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

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= playerSpeed;
            moveDirection.z *= playerSpeed;

            //crouch state
            if (Input.GetButton("Crouch") && !crouch)
            {
                if (playerSprint)
                    playerSprint = false;


                headJoint.transform.position -= new Vector3(0.0f, crouchDiffeance, 0.0f);
                noteBookGO.transform.position -= new Vector3(0.0f, crouchDiffeance, 0.0f);

                crouch = true;
            }

            else if (!Input.GetButton("Crouch") && crouch)
            {
                headJoint.transform.position += new Vector3(0.0f, crouchDiffeance, 0.0f);
                noteBookGO.transform.position += new Vector3(0.0f, crouchDiffeance, 0.0f);

                crouch = false;
            }
        }
        else
        {
            moveDirection = new Vector3(0.0f, 0.0f, 0.0f);

            noteBooks = GameObject.FindGameObjectsWithTag("noteBook");

                foreach (GameObject n in noteBooks)
                {
                    if (n.GetComponent<noteBook>().bookOpen)
                    {
                        n.GetComponent<noteBook>().Close();
                }
            }
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
        if (Input.GetButton("VRup") && headJoint.transform.position.y < 1.75)
        {
            headJoint.transform.position += new Vector3(0.0f, 0.01f, 0.0f);
        }

        if (Input.GetButton("VRdown") && headJoint.transform.position.y > 1.15)
        {
            headJoint.transform.position -= new Vector3(0.0f, 0.01f, 0.0f);
        }

        // NON VR LEANING
        if (!VRSettings.enabled)
        {
            
            if (Input.GetAxis("LLean") > 0)
            {
				playerLean.keyboard = false;
				playerLean.controller = true;

                float newLLeanAngle = Input.GetAxis("LLean") * playerLean.maxLeft;
                playerLean.leanObject.transform.localRotation = Quaternion.Lerp(playerLean.leanObject.transform.localRotation, Quaternion.Euler(0, 0, newLLeanAngle), playerLean.leanSpeed * Time.deltaTime);
                playerLean.lEase = playerLean.leanObject.transform.localRotation.eulerAngles.z;
                //Debug.Log("LLean " + Input.GetAxis("LLean"));
            }
            else if (Input.GetAxis("RLean") < 0)
            {
				playerLean.keyboard = false;
				playerLean.controller = true;

                float newRLeanAngle = Input.GetAxis("RLean") * playerLean.maxRight;
                playerLean.leanObject.transform.localRotation = Quaternion.Lerp(playerLean.leanObject.transform.localRotation, Quaternion.Euler(0, 0, -1 * newRLeanAngle), playerLean.leanSpeed * Time.deltaTime);
                playerLean.rEase = -1 * (playerLean.leanObject.transform.localRotation.eulerAngles.z - 360);
                //Debug.Log("RLean " + Input.GetAxis("RLean"));
            }
			else if (playerLean.controller && Input.GetAxis("RLean") == 0 && Input.GetAxis("LLean") == 0)
			{
				playerLean.leanObject.transform.localRotation = Quaternion.Lerp(playerLean.leanObject.transform.localRotation, Quaternion.Euler(0.0f, 0, 0), playerLean.leanReturnSpeed * Time.deltaTime);
			}
            

			if (Input.GetButton("MKLLean"))
			{
				playerLean.keyboard = true;
				playerLean.controller = false;

				float newLLeanAngle = Input.GetAxis("MKLLean") * playerLean.maxLeft;
				playerLean.leanObject.transform.localRotation = Quaternion.Lerp(playerLean.leanObject.transform.localRotation, Quaternion.Euler(0, 0, newLLeanAngle), playerLean.leanSpeed * Time.deltaTime);
				playerLean.lEase = playerLean.leanObject.transform.localRotation.eulerAngles.z;
				//Debug.Log("LLean " + Input.GetAxis("LLean"));
			}
			else if (Input.GetButton("MKRLean"))
			{
				playerLean.keyboard = true;
				playerLean.controller = false;

				float newRLeanAngle = Input.GetAxis("MKRLean") * playerLean.maxRight;
				playerLean.leanObject.transform.localRotation = Quaternion.Lerp(playerLean.leanObject.transform.localRotation, Quaternion.Euler(0, 0, newRLeanAngle), playerLean.leanSpeed * Time.deltaTime);
				playerLean.rEase = -1 * (playerLean.leanObject.transform.localRotation.eulerAngles.z - 360);
				//Debug.Log("RLean " + Input.GetAxis("RLean"));
			}
			else if (playerLean.keyboard && !Input.GetButtonDown("MKRLean") && !Input.GetButtonDown("MKLLean"))
			{
				playerLean.leanObject.transform.localRotation = Quaternion.Lerp(playerLean.leanObject.transform.localRotation, Quaternion.Euler(0.0f, 0, 0), playerLean.leanReturnSpeed * Time.deltaTime);
			}

        }

        // Heart Beat Listening
        if (Input.GetButton("HBListen"))
        {
            calibrationDone = true;

            // Use this variable to access in calibration
            HBListening = true;
            heart.GetComponent<heartBeatThump>().heartListening = true;

            // Remove the notes for HBListening
            //notesPage.SetActive(false);
            //hintsPage.SetActive(false);

            
            noteBooks = GameObject.FindGameObjectsWithTag("noteBook");

            foreach (GameObject n in noteBooks)
            {
                if (n.GetComponent<noteBook>().bookOpen)
                {
                    n.GetComponent<noteBook>().Close();
                }
            }

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
            //notesPage.SetActive(true);
            //hintsPage.SetActive(true);

            
            noteBooks = GameObject.FindGameObjectsWithTag("noteBook");

            foreach (GameObject n in noteBooks)
            {
                if (n.GetComponent<noteBook>().bookOpen)
                {
                    n.GetComponent<noteBook>().Open();
                }
                else
                {
                    n.GetComponent<noteBook>().Close();
                }
                
            }
            

            // If the player is holding the HBListen key the animation is triggered
            anim.SetBool("HBListen", false);
        }


        

        // Toggle Journal (Alpha)
        if (Input.GetButtonDown("NoteBook"))
        {
            noteBooks = GameObject.FindGameObjectsWithTag("noteBook");

            foreach (GameObject n in noteBooks)
            {
                if (!n.GetComponent<noteBook>().bookOpen)
                {
                    n.GetComponent<noteBook>().Open();
                }
                else
                {
                    n.GetComponent<noteBook>().Close();
                }
                
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


        if (Input.GetButton("Cancel"))
        {
            quitTime += Time.deltaTime;

            if(quitTime > 4.0f)
            {
                Application.Quit();

                if (UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }
            
        }
        else
        {
            quitTime = 0.0f;
        }
			

        


        if (VRSettings.enabled)
        {

            float camY = camerasgroup.playerVR.transform.rotation.eulerAngles.y;

            if (Input.GetButtonDown("VRLookReset"))
            {                

                    transform.eulerAngles += new Vector3(transform.eulerAngles.x, camY, transform.eulerAngles.z);

                    UnityEngine.VR.InputTracking.Recenter();
                
            }
        }


    }

    public void Effects()
    {
        float playerEffectScale = -1.0f * ((playerHealth / 100) - 1.0f);

        if (!VRSettings.enabled)
        {
            nonVrCam.GetComponent<ColorCurvesManager>().Factor = Mathf.Lerp(0.0f, 1.0f, playerEffectScale);
        }
        else
        {
            vrCam.GetComponent<ColorCurvesManager>().Factor = Mathf.Lerp(0.0f, 1.0f, playerEffectScale);

        }
    }

    public void KOVoice()
    {
        playerSpeech.voice.clip = playerSpeech.hitSounds[Random.Range(0, playerSpeech.hitSounds.Length)];
        playerSpeech.voice.Play();
    }

    public void DeathVoice()
    {
        playerSpeech.voice.clip = playerSpeech.deathSounds[Random.Range(0, playerSpeech.deathSounds.Length)];
        playerSpeech.voice.Play();
    }

}