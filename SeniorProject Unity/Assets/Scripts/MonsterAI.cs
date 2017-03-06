using UnityEngine;
using System.Collections;
using System;

public class MonsterAI : MonoBehaviour {

    public NavMeshAgent agent;

    public bool debugMonsterSpeakStates = false;
    public bool debugInvestigate = false;
    public bool debugInvestigateSound = false;
    public bool debugDoorAction = false;

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

    // Objects of interest
    public GameObject player;
    public GameObject[] doors;
    public GameObject[] tapeRecorders;
    public GameObject investigateRecorder;
    public GameObject searchObject;

    // action operation handler
    public float actionCooldown = 2.0f;
	public float actionTime = 0.0f;

    // lights
    public GameObject[] unaturalLights;
    float[] unaturalLightIntensity;
    
    // monster details
    public bool monsterGone = false;
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

        agent = GetComponent<NavMeshAgent>();
        lastPosition = new Vector3(0, 0, 0);
        unaturalLights = GameObject.FindGameObjectsWithTag("unaturalLight");

        unaturalLightIntensity = new float[unaturalLights.Length];

        destinationIterator = 0;

        for (int x = 0; x < unaturalLights.Length; x++)
        {
            unaturalLightIntensity[x] = unaturalLights[x].GetComponent<Light>().intensity;
        }

        monsterGone = false;

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
                if(debugInvestigate)
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
        // visual confirmation (trip a local bool)

        // later this will be changed to a visual confirmation, distance check, and presence check if statement
        // those parameters will make a chageEngage local bool
        
        
        /// Engage a chase within distance
        if(playerDistance < 2.0f)
        {
            if(debugMonsterSpeakStates)
                Debug.Log("FOUND YOU!");

            state = "chase";
        }
        /// Ignore/Miss player
        else
        {
            // this should be replaced with a laststate return later
            if (debugMonsterSpeakStates)
                Debug.Log("WHERE ARE YOU?");

            state = "patrol";
        }
    }

    /// This is a chase which is sustained so long as the player is in range or their presence is still high (no visual confirmation so he can chase around corners)
    public void Chase()
	{
        // action is a cooldown type so calculate time while in state
        actionTime += Time.deltaTime;

        /// chasing the player
        // set the playerPursuit distance as a variable[make later], and presence pursuit variable[make later]
        if (playerDistance < 4.0f || presence > 0.4f)
        {
            if (debugMonsterSpeakStates)
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
            if (debugMonsterSpeakStates)
                Debug.Log("WHERE DID YOU GO???");

            state = "patrol";
        }

        // maybe set new speeds for the agent too?
        

        // if presence is still high enough
        /// AND visual contact (use raycast method)
        /// AND close enough to player (scales to double if flashlight is on) - Access the boolean GameObject.FindGameObjectWithTag("GameController").GetComponent<Presence>().playerFlashlight;
        // continue to chase player
        // play chase animation
        // // if player within attack range
        // // call instance of Attack() [use cooldown too]
        // else
        // go to patrol
    }

	public void Attack()
	{

        if (debugMonsterSpeakStates)
            Debug.Log("DIE");

        gameObject.GetComponent<monsterAnimator>().attack = true;

        // damage player
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

        // if player health is less than or equal to 0
        //-- PlayerDeath();
        // else just knock out the player
        //-- PlayerKO();
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

        if(debugMonsterSpeakStates)
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

            if(debugInvestigate)
			    Debug.Log("new distances instanced " + searchDistances);

			for (int s = 0; s < searchDestinations.Length; s++)
			{
				searchDistances[s] = Vector3.Distance(investigateRecorder.transform.position, searchDestinations[s].transform.position);
			}

            if(debugInvestigate)
			    Debug.Log("new distances assigned " + searchDistances);

			float toDistance = Mathf.Min(searchDistances);

            if(debugInvestigate)
			    Debug.Log("todistance is " + toDistance);

			for (int s = 0; s < searchDestinations.Length; s++)
			{
				if (searchDistances[s] == toDistance)
				{
					searchObject = searchDestinations[s];
                    if(debugInvestigate)
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
        if (playerFlashlight && presence > 0.5f && playerDistance < 3.0f)
        {
            if (debugMonsterSpeakStates)
                Debug.Log("What was that?");

            state = "search";
        }
    }

    // later this will be used to calculate state engagement ranges based on player presence and/or flashlight
    public void CalculateRanges()
    {

    }

    /// Monster moves to the trigger sound room
    public void InvestigateSound()
    {
		// go to searchDestination as searchObject
		if (!searchObject.GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
		{
            if(debugInvestigateSound)
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

            if (debugInvestigateSound)
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

    /// Extra actions the monster can do - this one is to prevent getting stuck on a door
	public void OpenNearbyDoor()
	{
		actionTime += Time.deltaTime;

		float[] doorDistances = new float[doors.Length];

        if (debugDoorAction)
            Debug.Log("new door distances instanced " + doorDistances);

		for (int s = 0; s < doors.Length; s++)
		{
			doorDistances[s] = Vector3.Distance(gameObject.transform.position, doors[s].transform.position);
		}

        if (debugDoorAction)
            Debug.Log("new door distances assigned " + doorDistances);

		float toDistance = Mathf.Min(doorDistances);

        if (debugDoorAction)
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
