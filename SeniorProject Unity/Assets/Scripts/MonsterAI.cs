using UnityEngine;
using System.Collections;
using System;

public class MonsterAI : MonoBehaviour {

    

    [System.Serializable]
    public class mDebug
    {
        public bool monsterSpeakStates = false;
        public bool investigate = false;
        public bool investigateSound = false;
        public bool doorAction = false;
    }
    public mDebug debug = new mDebug();

    private NavMeshAgent agent;

    public float time = 0.0f;

    // destination points
    public GameObject[] patrolDestinations;
    public GameObject[] searchDestinations;
    int destinationIterator = 0;
    int x = 0;

    // Player variables
    public float playerHealth;
    public GameObject[] playerSpawns;

    // CHASE state calculation variables
    public float playerDistance;
    public bool playerFlashlight;

    // SEARCH state variables
    public float sightDistance = 3.4f;
    public float monsterSightDistance;
    public bool playerSeen = false;

    // Objects of interest
    public GameObject monsterEyes;
    public GameObject seen;
    private RaycastHit hit;
    private Vector3 cameraCenter;

    public GameObject player;
    public GameObject[] doors;
    public GameObject[] tapeRecorders;
    private GameObject investigateRecorder;
    private GameObject searchObject;

    // action operation handler
    public float actionCooldown = 2.0f;
	public float actionTime = 0.0f;

    public float searchCooldown = 8.0f;
    public float searchTime = 0.0f;

    // lights
    private GameObject[] unaturalLights;
    float[] unaturalLightIntensity;
    
    // monster details
    public string state = "patrol";
    public float presence;


    private Vector3 lastPosition;
    private Vector3 velocity;

    // Use this for initialization
    void Start () {
        // finding player
        player = GameObject.FindGameObjectWithTag("GameController");

        // objects of interest startup
        // remember to give him tape recorders
        // remmeber to give him doors
        // remmenber to give him flashlights

        // Player variables
        playerHealth = GameObject.FindGameObjectWithTag("GameController").GetComponent<controller>().playerHealth;

        time = 0.0f;
		actionTime = 0.0f;

        // search state stuff

        playerSeen = false;
        monsterSightDistance = sightDistance;

        agent = GetComponent<NavMeshAgent>();
        lastPosition = new Vector3(0, 0, 0);
        unaturalLights = GameObject.FindGameObjectsWithTag("unaturalLight");

        unaturalLightIntensity = new float[unaturalLights.Length];

        destinationIterator = 0;

        for (int x = 0; x < unaturalLights.Length; x++)
        {
            unaturalLightIntensity[x] = unaturalLights[x].GetComponent<Light>().intensity;
        }

		// states bool startup
		state = "patrol";
        presence = player.GetComponent<Presence>().curPres;
        playerFlashlight = player.GetComponent<Presence>().playerFlashlight;
    }
	
	// Update is called once per frame
	void Update () {

        /// Universal time to sample
		time += Time.deltaTime;
        ///

        /// Player variables Update
        playerHealth = GameObject.FindGameObjectWithTag("GameController").GetComponent<controller>().playerHealth;
        if (playerFlashlight)
        {
            monsterSightDistance = sightDistance;
        }
        else
        {
            monsterSightDistance = sightDistance/2;
        }
        /// 


        /// CHASE state variables update
        playerFlashlight = player.GetComponent<Presence>().playerFlashlight;
        presence = player.GetComponent<Presence>().curPres;
        playerDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        ///

        /// State runner - chooses appropriate function for state, or instances another instant action function (like attack)
        if (state == "patrol")
        {
            Patrol();
        }
        else if (state == "search")
        {
            Search();
        }
        else if (state == "chase")
        {
            Chase();
        }
		else if (state == "investigate")
		{
			Investigate();
		}
        else if (state == "investigateSound")
        {
            InvestigateSound();
        }
        ///

        /// State trigger logic - this has the rules for enabling and disabling behaviours
        foreach (GameObject r in tapeRecorders)
        {
			if (r.GetComponent<AudioSource>().isPlaying && r != null && searchObject == null)
            {
                if(debug.investigate)
				    Debug.Log("searching for recorder " + r + " is now recorder " + investigateRecorder);

				investigateRecorder = r;
				state = "investigate"; // instance to determine source of sound and where to go
            }

        }

        // check for the player if the monster should start a search
        if(state != "chase")
        {
            PlayerAwarenessCheck();
        }
        /// 


        // Generic Monster behaviours
        DimLights();
        MonsterAnimate();
        Looking();
        CalculateRanges();
        //

    }

    /// This will control what animations the monster is doing, and the sounds are handled by the monsterAnimation script
    public void MonsterAnimate()
    {
        if(state == "patrol")
        {
            // Idle
            gameObject.GetComponent<monsterAnimator>().idle = true;
            gameObject.GetComponent<monsterAnimator>().search = false;
            gameObject.GetComponent<monsterAnimator>().chase = false;
        }
        else if (state == "search")
        {
            // Search
            gameObject.GetComponent<monsterAnimator>().idle = false;
            gameObject.GetComponent<monsterAnimator>().search = true;
            gameObject.GetComponent<monsterAnimator>().chase = false;
        }
        else if (state == "chase")
        {
            // Chase
            gameObject.GetComponent<monsterAnimator>().idle = false;
            gameObject.GetComponent<monsterAnimator>().search = false;
            gameObject.GetComponent<monsterAnimator>().chase = true;
        }
        else if (state == "investigate")
        {
            // change to same as last state mayber?
            // Idle
            gameObject.GetComponent<monsterAnimator>().idle = true;
            gameObject.GetComponent<monsterAnimator>().search = false;
            gameObject.GetComponent<monsterAnimator>().chase = false;
        }
        else if (state == "investigateSound")
        {
            // Idle
            gameObject.GetComponent<monsterAnimator>().idle = true;
            gameObject.GetComponent<monsterAnimator>().search = false;
            gameObject.GetComponent<monsterAnimator>().chase = false;
        }

    }

    /// This is the monster's default state for travelling
    public void Patrol()
    {
        // Patrol destination loop
        if (destinationIterator >= patrolDestinations.Length)
        {
            destinationIterator = 0;
        }

        if (!patrolDestinations[destinationIterator].GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
        {
            agent.SetDestination(patrolDestinations[destinationIterator].transform.position);
        }
        else
        {
            destinationIterator++;
        }
        //    
    }

    /// This function is a search, meaning it uses the monster's head to raycast to target and see if they should persue them
    public void Search()
    {
        searchTime += Time.deltaTime;

        // visual confirmation (trip a local bool)
        
        // just in case
        MonsterAnimate();
        //Looking();

        // later this will be changed to a visual confirmation, distance check, and presence check if statement maybe
        // those parameters will make a chageEngage local bool


        /// Engage a chase within distance

        // this should be replaced with a laststate return later
        if (debug.monsterSpeakStates)
            Debug.Log("WHERE ARE YOU?");

        if (playerDistance <= sightDistance && playerSeen)
        {
            if(debug.monsterSpeakStates)
                Debug.Log("FOUND YOU!");

            state = "chase";
            searchTime = 0;
        }
        /// Ignore/Miss player
        else
        {
            if (searchTime > searchCooldown)
            {
                if (debug.monsterSpeakStates)
                    Debug.Log("Guess it was nothing");

                state = "patrol";
                searchTime = 0;
            }

        }

    }

    /// This is a chase which is sustained so long as the player is in range or their presence is still high (no visual confirmation so he can chase around corners)
    public void Chase()
	{
        // action is a cooldown type so calculate time while in state
        actionTime += Time.deltaTime;

        /// chasing the player
        // set the playerPursuit distance as a variable[make later], and presence pursuit variable[make later], also make it so that if he can still see you then keep chasing
        if (playerDistance < 5.0f || presence > 3.4f)
        {
            if (debug.monsterSpeakStates)
                Debug.Log("HERE'S JOHNNY!!!");

            // travel to player
            agent.SetDestination(player.transform.position);

            // Attack the player within a certain distance
            if(playerDistance < 1.0f && actionTime > actionCooldown)
            {
                Attack();
                actionTime = 0.0f;
            }
        }
        /// lost the player
        else
        {
            if (debug.monsterSpeakStates)
                Debug.Log("WHERE DID YOU GO???");

            state = "patrol";
        }

        // maybe set new speeds for the agent too?
       
    }

    /// This is a chase which is sustained so long as the player is in range or their presence is still high (no visual confirmation so he can chase around corners)
    public void ScriptedChase()
    {
        // action is a cooldown type so calculate time while in state
        actionTime += Time.deltaTime;

        /// chasing the player
        // set the playerPursuit distance as a variable[make later], and presence pursuit variable[make later], also make it so that if he can still see you then keep chasing
       
        if (debug.monsterSpeakStates)
            Debug.Log("HERE'S JOHNNY!!!");

        // travel to player
        agent.SetDestination(player.transform.position);

        // Attack the player within a certain distance
        if (playerDistance < 1.0f && actionTime > actionCooldown)
        {
            Attack();
            actionTime = 0.0f;
        }
       
    }

    public void Attack()
	{

        if (debug.monsterSpeakStates)
            Debug.Log("DIE");

        gameObject.GetComponent<monsterAnimator>().attack = true;

        // damage player
        // for damage to hit arm must pass through player ???
        DamagePlayer();
    }

    public void DamagePlayer()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<controller>().playerHealth = playerHealth - 35.0f;

        if(playerHealth <= 0.0f)
        {
            PlayerDeath();
        }
        else
        {
            PlayerKO();
        }

    }

    public void PlayerKO()
    {
        int monsterDestination = UnityEngine.Random.Range(0, patrolDestinations.Length);

        gameObject.transform.position = patrolDestinations[monsterDestination].transform.position;
        // idk if this is necessary
        agent.SetDestination(patrolDestinations[monsterDestination].transform.position);


        int playerDestination = UnityEngine.Random.Range(0, playerSpawns.Length);
        
        player.transform.position = playerSpawns[playerDestination].transform.position;
        
        // fade to black on controller
        // spawn monster somwhere else
        // spawn player somewhere else (CREATE PLAYER SPAWN POINTS)
        // once fade done, and player moved fade to normal again
    }

    public void PlayerDeath()
    {
        state = "patrol";

        if(debug.monsterSpeakStates)
            Debug.Log(" Y O U   D I E D ");

        // Change scene to death screen 
        // GAME OVER
    }

    // an instance function to look at the tape playing's closest search point
    public void Investigate()
    {
		if (investigateRecorder != null)
		{
			float[] searchDistances = new float[searchDestinations.Length];

            if(debug.investigate)
			    Debug.Log("new distances instanced " + searchDistances);

			for (int s = 0; s < searchDestinations.Length; s++)
			{
				searchDistances[s] = Vector3.Distance(investigateRecorder.transform.position, searchDestinations[s].transform.position);
			}

            if(debug.investigate)
			    Debug.Log("new distances assigned " + searchDistances);

			float toDistance = Mathf.Min(searchDistances);

            if(debug.investigate)
			    Debug.Log("todistance is " + toDistance);

			for (int s = 0; s < searchDestinations.Length; s++)
			{
				if (searchDistances[s] == toDistance)
				{
					searchObject = searchDestinations[s];
                    if(debug.investigate)
					    Debug.Log("now investigating sound at " + searchObject);

					state = "investigateSound";
				}
			}
		}
        


    }

    /// Monster checks to see if it knows the player is there, if it confirms it will do a search
    public void PlayerAwarenessCheck()
    {
        // The only variables it checks is it's search starting distance[make later] checked against player distance, and then presence vs search starting presence[make later]
        if (presence > 0.34f && playerDistance < 5.0f)
        {
            if (debug.monsterSpeakStates)
                Debug.Log("What was that?");

            state = "search";
        }
    }

    // later this will be used to calculate state engagement ranges based on player presence and/or flashlight
    public void CalculateRanges()
    {

    }

    /// THe function looks for the player and trips a boolean that the player has been seen
    public void Looking()
    {
        //playerSeen = monsterEyes.GetComponent<monsterEyes>().playerSeen;
        
        
        cameraCenter = monsterEyes.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, monsterEyes.GetComponent<Camera>().nearClipPlane));

        
        // raycast from camera
        if (Physics.Raycast(cameraCenter, monsterEyes.transform.forward, out hit, 1000))
        {
            // the object seen is what the raycast hit
            seen = hit.transform.gameObject;

            Debug.Log("seen object " + seen);

            // see if the seen object is the player
            if (seen.CompareTag("GameController") && state == "search")
            {
                if (Vector3.Distance(hit.point, gameObject.transform.position) <= sightDistance)
                {
                    playerSeen = true;
                }
            }

            Debug.Log("seen player " + playerSeen);

        }
        
    }

    /// Monster moves to the trigger sound room
    public void InvestigateSound()
    {
		// go to searchDestination as searchObject
		if (!searchObject.GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
		{
            if(debug.investigateSound)
			    Debug.Log("travelling to " + searchObject);

			velocity = (transform.position - lastPosition) / Time.deltaTime;
			lastPosition = transform.position;

			// if running into a door
			if (velocity.magnitude < 0.01f)
			{
				OpenNearbyDoor();
			}

			agent.SetDestination(searchObject.transform.position);
		}
		else
		{
			actionTime += Time.deltaTime;

			if(actionTime > actionCooldown)
			{
				investigateRecorder.GetComponent<tapeMaster>().forceStop();
			}

            if (debug.investigateSound)
                Debug.Log("turned off " + investigateRecorder + " " + investigateRecorder.GetComponent<AudioSource>().isPlaying);

			// Exit to patrol or search state when music is turned off
			if (!investigateRecorder.GetComponent<AudioSource>().isPlaying) 
			{
				// set investigateRecorder = null
				investigateRecorder = null;
				searchObject = null;

				// execute search with stateSearch
				//stateSearch = true;
				state = "search";
			}

		}
    }

    public void OnDrawGizmos()
    {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(cameraCenter, hit.point);
    }

    /// Extra actions the monster can do - this one is to prevent getting stuck on a door
	public void OpenNearbyDoor()
	{
		actionTime += Time.deltaTime;

		float[] doorDistances = new float[doors.Length];

        if (debug.doorAction)
            Debug.Log("new door distances instanced " + doorDistances);

		for (int s = 0; s < doors.Length; s++)
		{
			doorDistances[s] = Vector3.Distance(gameObject.transform.position, doors[s].transform.position);
		}

        if (debug.doorAction)
            Debug.Log("new door distances assigned " + doorDistances);

		float toDistance = Mathf.Min(doorDistances);

        if (debug.doorAction)
            Debug.Log("to door distance is " + toDistance);

		for (int s = 0; s < doors.Length; s++)
		{
			// open the door but also only do so if sound is not playing
			if (doorDistances[s] == toDistance && actionTime > actionCooldown)
			{
				doors[s].GetComponent<doorMaster>().unLock();
				doors[s].GetComponent<doorMaster>().forceOpen();

				actionTime = 0.0f;
			}
		}
	}

    ///  the monster does this all the time, basically he dims eletrical power
    public void DimLights()
    {
        // Lights Dimming
        for (int j = 0; j < unaturalLights.Length; j++)
        {
            float distanceToLight = (unaturalLights[j].transform.position - transform.position).magnitude;

            if (distanceToLight <= 10.0f)
            {
                unaturalLights[j].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[j].GetComponent<Light>().intensity, 0.0f, (distanceToLight / 10.0f) * Time.deltaTime);
            }

            else
            {
                unaturalLights[j].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[j].GetComponent<Light>().intensity, unaturalLightIntensity[j], (distanceToLight / 60.0f) * Time.deltaTime);
            }
        }
        //
    }








    ///  old code we may never need
    public void MonsterLightReset()
    {
        // light reset code
        /*
        if (destinationIterator >= patrolDestinations.Length)
        {
            if(debug)
                Debug.Log("movement end");

            for (int y = 0; y < unaturalLights.Length; y++)
            {
                unaturalLights[y].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[y].GetComponent<Light>().intensity, unaturalLightIntensity[y], (20.0f) * Time.deltaTime);
            }


            int lightsNormal = 0;

            for (int y = 0; y < unaturalLights.Length; y++)
            {
                if(unaturalLights[y].GetComponent<Light>().intensity == unaturalLightIntensity[y]){
                    lightsNormal++;
                }
            }
            if(debug)
                Debug.Log("lightsnorml " + lightsNormal);

            if (lightsNormal >= unaturalLights.Length)
            {
                gameObject.SetActive(false);

                if(debug)
                    Debug.Log("monster poof");
            }

            monsterGone = true;

        }
        
        else
        {
            // Patrol destination loop
        }
        */
        //
    }

}
