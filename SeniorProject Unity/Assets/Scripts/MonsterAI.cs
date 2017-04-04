using UnityEngine;
using System.Collections;
using System;

public class MonsterAI : MonoBehaviour {


    // Monster Debug Management
    [System.Serializable]
    public class mDebug
    { 
        public bool monsterSpeakStates = false;
        public bool pureStates = false;
        public bool investigate = false;
        public bool investigateSound = false;
        public bool monsterSight = false;
        public bool doorAction = false;
        public bool fade = false;
        public bool gizmos = false;
    }
    public mDebug debug = new mDebug();

    // get agent
    private NavMeshAgent agent;

    // Init universal time
    private float time = 0.0f;

    // destination points &  objects of interest
    public GameObject deathRoom;
    public GameObject[] patrolDestinations;
    public GameObject[] searchDestinations;
    public GameObject[] doors;
    public GameObject[] tapeRecorders;
    int destinationIterator = 0;
    int x = 0;

    // Monster Agent Management
    [System.Serializable]
    public class agentManagement
    {
        public float speed = 2.5f;
        public float angularSpeed = 120.0f;
        public float acceleration = 8.0f;
        public float stoppingDistance = 0.0f;
        public float stuckDoorOpen = 0.01f;
        public bool stuckOnOpenDoor = false;
        public float doorStuckTime = 0.0f;
        public float doorGhostCooldown = 3.0f;
        public float doorReachDistance = 1.0f;
        public GameObject stuckDoor;
    }
    public agentManagement agentManager = new agentManagement();

    // Monster Agent Management
    [System.Serializable]
    public class monsterBalance
    {
        public bool pacify = false;
        public GameObject[] hands;

        public float evasionDistance = 12.0f;
        public float attackDistance = 1.0f;
        public float sightDistance = 11.0f;
        public float hearPlayerDistance = 4.0f;

        public float closeCallDistance = 1.3f;
        public float closeCallPresence = 0.25f;

        public float presenceEvasion = 0.35f;
        public float presenceSearchInterrupt = 0.69f;
        public float presenceAwareToSearch = 0.47f;
        public float distanceAwareToSearch = 8.0f;

        public float actionCooldown = 2.0f;
        public float searchLength = 8.0f;
        public float speedNerf = 0.22f;
    }
    public monsterBalance monsterBalancer = new monsterBalance();

    // Player class of variables
    [System.Serializable]
    public class playerManagement
    {
        // Player variables
        public bool won = false;
        public float health;
        public float maxSpeed;
        public GameObject[] spawns;
        public GameObject[] winZones;
        public GameObject headMaster;
        public GameObject clothes;
        
        public Material fader;
        public bool KO = false;
        public bool death = false;
        public bool fading = false;
        public float fadeRate = 1.0f;
        public float fadeColor;

        // CHASE state calculation variables
        public float distanceAway;
        public bool flashlight;
    }
    public playerManagement playerManager = new playerManagement();

    // Changing Runtime variables (Search & other states affect them)
    public float monsterSightDistance;
    public float monsterEvasionDistance;
    public bool playerSeen = false;

    // Objects of interest
    private GameObject[] monsterEyes;

    private GameObject seen;
    private RaycastHit hit;
    private Vector3 rayStart;
    private Vector3 rayMove;
    public GameObject cabinet;

    public GameObject player;
    private GameObject investigateRecorder;
    private GameObject searchObject;
    private GameObject patrolObject;

    // action operation handler
    public float actionTime = 0.0f;
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
    void Start() {
        // finding player
        player = GameObject.FindGameObjectWithTag("GameController");
        deathRoom = GameObject.FindGameObjectWithTag("DeathPoint");
        monsterBalancer.hands = GameObject.FindGameObjectsWithTag("monsterHands");
        playerManager.headMaster = GameObject.FindGameObjectWithTag("headMaster");
        playerManager.clothes = GameObject.FindGameObjectWithTag("Clothes");

        


        // Find faders
        if (!playerManager.fader)
        {
            playerManager.fader = GameObject.FindGameObjectWithTag("playerViewFader").GetComponent<Renderer>().material;
        }

        playerManager.fadeColor = 0.0f;

        playerManager.fader.SetColor("_Color", new Color(0, 0, 0, playerManager.fadeColor));

        /*
        if (playerManager.faders.Length < 1)
        {
            GameObject[] tempRend = GameObject.FindGameObjectsWithTag("playerViewFader");
            Debug.Log("materials find");

            for (int i = 0; i < tempRend.Length; i++)
            {
                Debug.Log("materials" + tempRend[i].GetComponent<Renderer>().material);
                playerManager.faders[i] = tempRend[i].GetComponent<Renderer>().material;
            }
        }  
        */
        //


        // objects of interest startup
        // remember to give him tape recorders
        // remmeber to give him doors
        // remmenber to give him flashlights

        playerManager.winZones = GameObject.FindGameObjectsWithTag("winTrigger");

        // Player variables
        playerManager.health = GameObject.FindGameObjectWithTag("GameController").GetComponent<controller>().playerHealth;
        playerManager.won = false;
        playerManager.maxSpeed = player.GetComponent<controller>().playerMaxSpeed;

        // time must be universal
        time = 0.0f;
        actionTime = 0.0f;
        searchTime = 0.0f;

        // search state stuff
        monsterEyes = GameObject.FindGameObjectsWithTag("MonsterEyes");
        playerSeen = false;
        monsterSightDistance = monsterBalancer.sightDistance;
        monsterEvasionDistance = monsterBalancer.evasionDistance;

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
        playerManager.flashlight = player.GetComponent<Presence>().presence.playerFlashlight;
    }

    // Update is called once per frame
    void Update() {

        // Find faders and do fade events
        if (!playerManager.fader)
        {
            playerManager.fader = GameObject.FindGameObjectWithTag("playerViewFader").GetComponent<Renderer>().material;
        }
        else if(playerManager.fader)
        {
            FadeEvent();
        }


        // calculate player's maximum possible speed
        playerManager.maxSpeed = player.GetComponent<controller>().playerMaxSpeed;

        // Check for player win state
        foreach (GameObject g in playerManager.winZones)
        {
            if (g.GetComponent<triggerWin>().playerWin)
            {
                playerManager.won = true;
            }
        }

        // if the player has won switch snapshot to quiet [do later]
        if (playerManager.won)
        {
            PatrolReturn();
        }

        // calculate monster velocity for door opening
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        if (debug.doorAction)
        {
            Debug.Log("velocity of monster" + velocity.magnitude);
        }


        /// Universal time to sample
		time += Time.deltaTime;
        ///

        if (debug.pureStates)
        {
            Debug.Log("state " + state);
        }


        /// Player variables Update
        playerManager.health = GameObject.FindGameObjectWithTag("GameController").GetComponent<controller>().playerHealth;

        if (playerManager.flashlight)
        {
            monsterSightDistance = monsterBalancer.sightDistance;
            monsterEvasionDistance = monsterBalancer.evasionDistance;
        }
        else
        {
            monsterSightDistance = monsterBalancer.sightDistance / 2;
            monsterEvasionDistance = monsterBalancer.evasionDistance / 2.2f;
        }
        /// 

        /// Updating Agent Parameters SPEED THE MONSTER TO PLAYER SPEED IF IN CHASE MODES
        if (state == "chase" || state == "scriptedChase") {
            agent.speed = playerManager.maxSpeed - monsterBalancer.speedNerf; // just slightly slower than fastest speed of player
        }
        else
        {
            agent.speed = agentManager.speed - monsterBalancer.speedNerf; // just slightly slower than fastest speed of player
        }
        //agent.speed = agentManager.speed;
        agent.angularSpeed = agentManager.angularSpeed;
        agent.acceleration = agentManager.acceleration;
        agent.stoppingDistance = agentManager.stoppingDistance;
        ///

        /// CHASE state variables update
        playerManager.flashlight = player.GetComponent<Presence>().presence.playerFlashlight;
        presence = player.GetComponent<Presence>().curPres;
        playerManager.distanceAway = Vector3.Distance(gameObject.transform.position, player.transform.position);
        ///

        if (cabinet.GetComponent<documentCabinet>().cabinetOpen && !playerManager.won)
        {
            state = "scriptedChase";
        }

        /// Behaviours - chooses appropriate function for state, or instances another instant action function (like attack)
        if (!monsterBalancer.pacify)
        {
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
            else if (state == "scriptedChase")
            {
                ScriptedChase();
            }
        }
        else
        {
            if (state == "patrol")
            {
                Patrol();
            }
        }
        
        ///

        /// State trigger logic - this has the rules for enabling and disabling behaviours
        foreach (GameObject r in tapeRecorders)
        {
            if (r.GetComponent<AudioSource>().isPlaying && r != null && searchObject == null)
            {
                if (debug.investigate)
                    Debug.Log("searching for recorder " + r + " is now recorder " + investigateRecorder);

                investigateRecorder = r;
                state = "investigate"; // instance to determine source of sound and where to go
            }

        }

        // check for the player if the monster should start a search
        if ((state == "patrol" || state == "search" || state == "investigate" || state == "investigateSound") && (!playerManager.death && !playerManager.KO))
        {
            PlayerAwarenessCheck();
        }
        /// 

        // unstuck from door
        if (agentManager.stuckOnOpenDoor)
        {
            RemoveDoorCollider(agentManager.stuckDoor);
        }
        //


        // Generic Monster behaviours
        CalculateRanges();
        DimLights();
        MonsterAnimate();
        Looking();
        Listening();
        //

        if (debug.monsterSpeakStates)
        {
            Debug.Log("playerManager.distanceAway " + playerManager.distanceAway);
        }

    }




    ////////// ------ BEHAVIOURS ------ //////////
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

    public void PatrolReturn()
    {
        // return to nearest patrol point
        float[] patrolDistances = new float[patrolDestinations.Length];

        if (debug.investigate)
            Debug.Log("new distances instanced " + patrolDistances);

        // create list of distances
        for (int s = 0; s < patrolDestinations.Length; s++)
        {
            patrolDistances[s] = Vector3.Distance(gameObject.transform.position, patrolDestinations[s].transform.position);
        }

        if (debug.investigate)
            Debug.Log("new distances assigned " + patrolDistances);

        // find the closest one
        float toDistance = Mathf.Min(patrolDistances);

        if (debug.investigate)
            Debug.Log("todistance is " + toDistance);

        // remember closest point
        for (int s = 0; s < patrolDestinations.Length; s++)
        {
            if (patrolDistances[s] == toDistance)
            {
                //patrolObject = patrolDestinations[s];
                destinationIterator = s;
                if (debug.investigate)
                    Debug.Log("now investigating sound at " + patrolObject);

                state = "patrol";
            }
        }
    }



    // This function is a search, meaning it uses the monster's head to raycast to target and see if they should persue them
    public void Search()
    {
        searchTime += Time.deltaTime;

        // Slow monster while searching
        agent.speed = 1.0f;

        // later this will be changed to a visual confirmation, distance check, and presence check if statement maybe
        // those parameters will make a chageEngage local bool


        // Engage a chase within distance

        // this should be replaced with a laststate return later
        if (debug.monsterSpeakStates)
            Debug.Log("WHERE ARE YOU?");

        // OR THE PRESENCE CAN BE REALLY HIGH AND JUST RAISE ALL HELL
        //if ((playerManager.distanceAway <= monsterSightDistance && playerSeen) || (presence > 3.8f && playerManager.distanceAway < 4.0f))
        if ((playerSeen && (presence > monsterBalancer.presenceEvasion)) || (presence > monsterBalancer.presenceSearchInterrupt && playerManager.distanceAway < monsterBalancer.hearPlayerDistance))
        {
            if (debug.monsterSpeakStates)
                Debug.Log("FOUND YOU!");

            if (debug.monsterSpeakStates && playerSeen)
            {
                Debug.Log("I SEE YOU!");
            }
            else if (debug.monsterSpeakStates && !playerSeen)
            {
                Debug.Log("I HEAR YOU!");
            }

            state = "chase";
            searchTime = 0;
        }
        /// Ignore/Miss player
        else
        {
            if (searchTime > monsterBalancer.searchLength)
            {
                if (debug.monsterSpeakStates)
                    Debug.Log("Guess it was nothing");

                playerSeen = false;

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // state = "patrol";
                PatrolReturn();

                searchTime = 0;
            }

        }

    }

    // This is a chase which is sustained so long as the player is in range or their presence is still high (no visual confirmation so he can chase around corners)
    public void Chase()
    {
        // action is a cooldown type so calculate time while in state
        actionTime += Time.deltaTime;

        // if running into a door // AND the player isn't within the attack distance
        if (velocity.magnitude < agentManager.stuckDoorOpen && playerManager.distanceAway > monsterBalancer.attackDistance / 2)
        {
            OpenNearbyDoor();
        }

        // chasing the player
        // set the playerPursuit distance as a variable[make later], and presence pursuit variable[make later], also make it so that if he can still see you then keep chasing
        if ((playerManager.distanceAway < monsterEvasionDistance) || (presence > monsterBalancer.presenceEvasion))
        {
            if (debug.monsterSpeakStates)
            {
                Debug.Log("HERE'S JOHNNY!!!");
                //Debug.Log("playerManager.distanceAway " + playerManager.distanceAway);
            }


            // travel to player
            agent.SetDestination(player.transform.position);



            // Attack the player within a certain distance
            if (playerManager.distanceAway < monsterBalancer.attackDistance && actionTime > monsterBalancer.actionCooldown)
            {
                Attack();
                actionTime = 0.0f;
            }
        }

        // lost the player
        else
        {
            if (debug.monsterSpeakStates)
                Debug.Log("WHERE DID YOU GO???");

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //state = "patrol";
            playerSeen = false;
            PatrolReturn();
        }

        // LOST PLAYER DUE TO DISTANCE // DEACTIVATE PLAYER SEEB
        if (playerManager.distanceAway > monsterEvasionDistance)
        {
            if (debug.monsterSpeakStates)
            {
                Debug.Log("YOU GOT AWAY");
            }

            playerSeen = false;
            PatrolReturn();
        }

        // LOST PLAYER DUE TO lOWERED PRESENCE // DEACTIVATE PLAYER SEEB
        if (presence < monsterBalancer.presenceEvasion)
        {
            if (debug.monsterSpeakStates)
            {
                Debug.Log("I CAN'T HEAR YOU ANYMORE");
            }

            playerSeen = false;
            PatrolReturn();
        }

        // maybe set new speeds for the agent too?

    }

    // This is a chase which is sustained so long as the player is in range or their presence is still high (no visual confirmation so he can chase around corners)
    public void ScriptedChase()
    {
        // action is a cooldown type so calculate time while in state
        actionTime += Time.deltaTime;

        /// chasing the player
        // set the playerPursuit distance as a variable[make later], and presence pursuit variable[make later], also make it so that if he can still see you then keep chasing

        // if running into a door
        if (velocity.magnitude < agentManager.stuckDoorOpen && playerManager.distanceAway > monsterBalancer.attackDistance / 2)
        {
            OpenNearbyDoor();
        }

        if (debug.monsterSpeakStates)
            Debug.Log("HERE'S JOHNNY!!!");

        // travel to player
        agent.SetDestination(player.transform.position);

        // Attack the player within a certain distance
        if (playerManager.distanceAway < monsterBalancer.attackDistance && actionTime > monsterBalancer.actionCooldown)
        {
            Attack();
            actionTime = 0.0f;
        }

        if (!playerManager.won)
        {
            state = "scriptedChase";
        }

    }

    // an instance function to look at the tape playing's closest search point
    public void Investigate()
    {
        if (investigateRecorder != null)
        {
            float[] searchDistances = new float[searchDestinations.Length];

            if (debug.investigate)
                Debug.Log("new distances instanced " + searchDistances);

            for (int s = 0; s < searchDestinations.Length; s++)
            {
                searchDistances[s] = Vector3.Distance(investigateRecorder.transform.position, searchDestinations[s].transform.position);
            }

            if (debug.investigate)
                Debug.Log("new distances assigned " + searchDistances);

            float toDistance = Mathf.Min(searchDistances);

            if (debug.investigate)
                Debug.Log("todistance is " + toDistance);

            for (int s = 0; s < searchDestinations.Length; s++)
            {
                if (searchDistances[s] == toDistance)
                {
                    searchObject = searchDestinations[s];
                    if (debug.investigate)
                        Debug.Log("now investigating sound at " + searchObject);

                    state = "investigateSound";
                }
            }
        }



    }

    // Monster moves to the trigger sound room
    public void InvestigateSound()
    {
        // go to searchDestination as searchObject
        if (!searchObject.GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
        {
            if (debug.investigateSound)
                Debug.Log("travelling to " + searchObject);



            // if running into a door
            if (velocity.magnitude < agentManager.stuckDoorOpen)
            {
                OpenNearbyDoor();
            }

            agent.SetDestination(searchObject.transform.position);
        }
        else
        {
            actionTime += Time.deltaTime;

            if (actionTime > monsterBalancer.actionCooldown)
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
                // stateSearch = true;
                state = "search";
            }

        }
    }

    // Monster checks to see if it knows the player is there, if it confirms it will do a search
    public void PlayerAwarenessCheck()
    {
        // The only variables it checks is it's search starting distance[make later] checked against player distance, and then presence vs search starting presence[make later]
        if (presence > monsterBalancer.presenceAwareToSearch && playerManager.distanceAway < monsterBalancer.distanceAwareToSearch)
        {
            if (debug.monsterSpeakStates)
                Debug.Log("What was that?");

            state = "search";
        }

    }

    // listening for a close player encounter if their 
    public void Listening()
    {
        if (playerManager.distanceAway < monsterBalancer.closeCallDistance && (playerManager.flashlight || presence > monsterBalancer.closeCallPresence))
        {
            state = "chase";
        }
    }
    ////////// ------ BEHAVIOURS ------ //////////





    ////////// ------ PLAYER MANAGEMENT ------ //////////
    // Attacking the player has resulted in a hit, this will determine what the hit does
    public void DamagePlayer()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<controller>().playerHealth = playerManager.health - 50.0f;

        // PLAYER FALLING DOWN SOUND
        playerManager.clothes.GetComponent<clothSounds>().Fall();

        if (playerManager.health <= 0.0f)
        {
            PlayerDeath();
        }
        else
        {
            PlayerKO();
        }

    }

    // The hit has caused a knock-out this makes the player spawn somewhere else
    public void PlayerKO()
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //state = "patrol";
        PatrolReturn();
        playerManager.fading = true;
        playerManager.KO = true;

        player.GetComponent<controller>().KOVoice();

        // fade to black on controller
        // spawn monster somwhere else
        // spawn player somewhere else (CREATE PLAYER SPAWN POINTS)
        // once fade done, and player moved fade to normal again
    }

    // The hit has caused a death this makes the player spawn in the death room, the only way out is to quit
    public void PlayerDeath()
    {
        if (debug.monsterSpeakStates)
            Debug.Log(" Y O U   D I E D ");

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //state = "patrol";
        PatrolReturn();
        playerManager.fading = true;
        playerManager.death = true;

        player.GetComponent<controller>().DeathVoice();

        // Change scene to death screen 
        // GAME OVER

    }

    // KO Teleport
    public void TPKO()
    {
        int monsterDestination = UnityEngine.Random.Range(0, patrolDestinations.Length);

        gameObject.transform.position = patrolDestinations[monsterDestination].transform.position;
        // idk if this is necessary
        agent.SetDestination(patrolDestinations[monsterDestination].transform.position);


        int playerDestination = UnityEngine.Random.Range(0, playerManager.spawns.Length);

        player.transform.position = playerManager.spawns[playerDestination].transform.position;

        /// re-enable colliders of monster hands
        foreach (GameObject g in monsterBalancer.hands)
        {
            g.GetComponent<Collider>().enabled = true;
        }
    }

    // Death Teleport
    public void TPDeath()
    {
        // GAME OVER

        player.transform.position = deathRoom.transform.position;

        /// re-enable colliders of monster hands
        foreach (GameObject g in monsterBalancer.hands)
        {
            g.GetComponent<Collider>().enabled = true;
        }
    }
    //

    // Death Teleport
    public void TPWon()
    {
        // GAME WON

        player.transform.position = GameObject.FindGameObjectWithTag("winPoint").transform.position;

    }
    //

    // FADEING AND TELEPORTING
    public void FadeEvent()
    {
        if (playerManager.fading)
        {
            // if player died
            if (playerManager.death)
            {
                if(debug.fade)
                    Debug.Log("player death fade");

                if (playerManager.fadeColor > 0.99f)
                {
                    // PLAYER GETTING UP SOUND
                    playerManager.clothes.GetComponent<clothSounds>().Rise();
                    TPDeath();

                    // fading out done
                    playerManager.fading = false;
                    playerManager.death = false;
                }
                else
                {
                    FadeOut();
                }
            }

            // if player was knocked out
            else if (playerManager.KO)
            {
                if (debug.fade)
                    Debug.Log("player KO fade");

                if (playerManager.fadeColor > 0.99f && playerManager.KO)
                {
                    if (!playerManager.won)
                    {
                        // PLAYER GETTING UP SOUND
                        playerManager.clothes.GetComponent<clothSounds>().Rise();
                        TPKO();
                    }
                    else
                    {
                        // PLAYER GETTING UP SOUND
                        playerManager.clothes.GetComponent<clothSounds>().Rise();
                        TPWon();
                    }
                    
                    // fading out done
                    playerManager.fading = false;
                    playerManager.KO = false;
                }
                else
                {
                    FadeOut();
                }
            }
        }
        // fading back if there is no death
        else
        {
            if (debug.fade)
                Debug.Log("player normal fade");

            FadeIn();
        }

        // adding effect
        playerManager.fader.SetColor("_Color", new Color(0, 0, 0, playerManager.fadeColor));
    }

    public void FadeIn()
    {
        playerManager.fadeColor = Mathf.Lerp(playerManager.fadeColor, 0.0f, playerManager.fadeRate * Time.deltaTime);
        playerManager.headMaster.transform.localPosition = Vector3.Lerp(playerManager.headMaster.transform.localPosition, new Vector3(playerManager.headMaster.transform.localPosition.x, 0.0f, playerManager.headMaster.transform.localPosition.z), playerManager.fadeRate * Time.deltaTime);
    }

    public void FadeOut()
    {
        playerManager.fadeColor = Mathf.Lerp(playerManager.fadeColor, 1.0f, playerManager.fadeRate * Time.deltaTime);
        playerManager.headMaster.transform.localPosition = Vector3.Lerp(playerManager.headMaster.transform.localPosition, new Vector3(playerManager.headMaster.transform.localPosition.x, -1.5f, playerManager.headMaster.transform.localPosition.z), playerManager.fadeRate * Time.deltaTime);
    }
    ////////// ------ PLAYER MANAGEMENT ------ //////////





    ////////// ------ INSTANCE ACTIONS ------ //////////
    // Swing arms at player
    public void Attack()
    {

        if (debug.monsterSpeakStates)
            Debug.Log("DIE");

        gameObject.GetComponent<monsterAnimator>().attack = true;

        // for damage to hit arm must pass through player ???
        foreach (GameObject g in monsterBalancer.hands)
        {
            if (g.GetComponent<Collider>().bounds.Intersects(player.GetComponent<Collider>().bounds))
            {
                g.GetComponent<Collider>().enabled = false;
                DamagePlayer();

                if (debug.monsterSpeakStates)
                    Debug.Log("Hit you!");
                //
                return;
            }
                
        }

        
    }

    // Extra actions the monster can do - this one is to prevent getting stuck on a door
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
            // open the door but also only do so if sound is not playing, action cooldown is ready, and the door is within reaching distance
            if (doorDistances[s] == toDistance && actionTime > monsterBalancer.actionCooldown && toDistance < agentManager.doorReachDistance)
            {
                // ONLY REMOVE COLLIDER FOR OPEN DOORS AND STUCK DOOR IS NOT ALREADY RUNNING ON ANOTHER DOOR
                if (doors[s].GetComponent<doorMaster>().doorOpen && !agentManager.stuckOnOpenDoor)
                {
                    agentManager.stuckDoor = doors[s];
                    agentManager.stuckOnOpenDoor = true;
                }

                doors[s].GetComponent<doorMaster>().unLock();
                doors[s].GetComponent<doorMaster>().forceOpen();

                actionTime = 0.0f;
            }
        }
    }
    ////////// ------ INSTANCE ACTIONS ------ //////////


    ////////// ------ CONSTANT ACTIONS ------ //////////
    // Used to fix monster getting stuck on OPEN doors
    public void RemoveDoorCollider(GameObject door)
    {
        agentManager.doorStuckTime += Time.deltaTime;

        if (agentManager.doorStuckTime >= agentManager.doorGhostCooldown)
        {
            if (door.GetComponent<doorMaster>().rDoorSound)
                door.GetComponent<doorMaster>().rDoorSound.GetComponentInParent<Collider>().enabled = true;
            if (door.GetComponent<doorMaster>().lDoorSound)
                door.GetComponent<doorMaster>().lDoorSound.GetComponentInParent<Collider>().enabled = true;
            if(door.GetComponent<doorMaster>().rDoorSound)
                door.GetComponent<doorMaster>().rDoorSound.GetComponentInParent<NavMeshObstacle>().enabled = true;
            if (door.GetComponent<doorMaster>().lDoorSound)
                door.GetComponent<doorMaster>().lDoorSound.GetComponentInParent<NavMeshObstacle>().enabled = true;
            agentManager.stuckDoor = null;
            agentManager.doorStuckTime = 0.0f;
            agentManager.stuckOnOpenDoor = false;
        }
       else
        {
            if (door.GetComponent<doorMaster>().rDoorSound)
                door.GetComponent<doorMaster>().rDoorSound.GetComponentInParent<Collider>().enabled = false;
            if (door.GetComponent<doorMaster>().lDoorSound)
                door.GetComponent<doorMaster>().lDoorSound.GetComponentInParent<Collider>().enabled = false;
            if (door.GetComponent<doorMaster>().rDoorSound)
                door.GetComponent<doorMaster>().rDoorSound.GetComponentInParent<NavMeshObstacle>().enabled = false;
            if (door.GetComponent<doorMaster>().lDoorSound)
                door.GetComponent<doorMaster>().lDoorSound.GetComponentInParent<NavMeshObstacle>().enabled = false;
        }

    }


    // later this will be used to calculate state engagement ranges based on player presence and/or flashlight
    public void CalculateRanges()
    {

    }

    // The function looks for the player and trips a boolean that the player has been seen
    public void Looking()
    {
        foreach (GameObject eye in monsterEyes)
        {
            // Change raycast to use or not use camera
            rayStart = eye.transform.position;

            // raycast from camera
            if (Physics.Raycast(rayStart, eye.transform.forward, out hit, 1000))
            {
                // the object seen is what the raycast hit
                seen = hit.transform.gameObject;

                if (debug.monsterSight)
                {
                    Debug.Log("I see the " + seen);
                }

                // see if the seen object is the player
                if (seen.CompareTag("GameController") && state == "search")
                {
                    if (Vector3.Distance(hit.point, gameObject.transform.position) <= monsterSightDistance)
                    {
                        playerSeen = true;
                    }
                }
            }

        }
        

    }

    // This will control what animations the monster is doing, and the sounds are handled by the monsterAnimation script
    public void MonsterAnimate()
    {
        if (state == "patrol")
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
        else if (state == "scriptedChase")
        {
            // Chase
            gameObject.GetComponent<monsterAnimator>().idle = false;
            gameObject.GetComponent<monsterAnimator>().search = false;
            gameObject.GetComponent<monsterAnimator>().scriptedChase = true;
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

    //  The monster does this all the time, basically he dims eletrical power
    public void DimLights()
    {
        // Lights Dimming
        for (int j = 0; j < unaturalLights.Length; j++)
        {
            float distanceToLight = (unaturalLights[j].transform.position - transform.position).magnitude;

            if (distanceToLight <= 10.0f)
            {
                // if it's a flashlight don't completely dim
                if (unaturalLights[j].GetComponent<flashLightOnOff>() != null)
                {
                    unaturalLights[j].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[j].GetComponent<Light>().intensity, 1.0f, (distanceToLight / 10.0f) * Time.deltaTime);
                    unaturalLights[j].GetComponent<flashLightOnOff>().flashPeriod = 2.0f;
                    unaturalLights[j].GetComponent<flashLightOnOff>().flashing = true;
                }
                // dim normal
                else
                {
                    unaturalLights[j].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[j].GetComponent<Light>().intensity, 0.0f, (distanceToLight / 10.0f) * Time.deltaTime);
                }
            }

            else
            {
                unaturalLights[j].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[j].GetComponent<Light>().intensity, unaturalLightIntensity[j], (distanceToLight / 60.0f) * Time.deltaTime);
            }
        }
        //
    }
    ////////// ------ CONSTANT ACTIONS ------ //////////





    ////////// ------ DEBUG EXTRAS ------ //////////
    /// Debug Vision Gizmos
    public void OnDrawGizmos()
    {

        // Draws a blue line from monster eyes to the target
        if (debug.gizmos)
        {
            // Eyesight
            Gizmos.color = Color.magenta;
            foreach (GameObject eye in monsterEyes)
            {
                Gizmos.DrawLine(eye.transform.position, hit.point);
            }

            // eyesight distance
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(gameObject.transform.position, monsterSightDistance);

            // attack distance
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(gameObject.transform.position, monsterBalancer.attackDistance);

            // evasion distance
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, monsterEvasionDistance);

            // Search for player distance
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(gameObject.transform.position, monsterBalancer.distanceAwareToSearch);

            // Hear player distance
            Gizmos.color = Color.Lerp(Color.yellow, Color.red, presence / 1.0f);
            Gizmos.DrawWireSphere(gameObject.transform.position, monsterBalancer.hearPlayerDistance);

            // player distance
            Gizmos.color = Color.white;
            Gizmos.DrawLine(gameObject.transform.position, player.transform.position);
            Gizmos.color = Color.Lerp(Color.white, Color.red, presence/1.0f);
            Gizmos.DrawWireSphere(player.transform.position, presence * monsterBalancer.distanceAwareToSearch);

        }
        
    }
    ////////// ------ DEBUG EXTRAS ------ //////////





    ////////// ------ EXTRA BEHAVIOURS ------ //////////
    public void MonsterLightReset()
    {
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

        Debug.Log("lightsnorml " + lightsNormal);

        if (lightsNormal >= unaturalLights.Length)
        {
            gameObject.SetActive(false);

                Debug.Log("monster poof");
        }

    }
    ////////// ------ EXTRA BEHAVIOURS ------ //////////



}
