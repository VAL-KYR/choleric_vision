using UnityEngine;
using System.Collections;
using System;

public class MonsterAI : MonoBehaviour {

    public NavMeshAgent agent;

    public bool debug = false;

	public float time = 0.0f;

    // destination points
    public GameObject[] patrolDestinations;
    public GameObject[] searchDestinations;
    int destinationIterator = 0;
    int x = 0;

    // Objects of interest
	public GameObject[] doors;
    public GameObject[] tapeRecorders;
    public GameObject investigateRecorder;
    public GameObject searchObject;

	// door operation handler
	public float actionCooldown = 2.0f;
	public float actionTime = 0.0f;

    // lights
    GameObject[] unaturalLights;
    float[] unaturalLightIntensity;
    

    public bool monsterGone = false;
    public string state = "patrol";

    private Vector3 lastPosition;
    private Vector3 velocity;

    // Use this for initialization
    void Start () {
        // objects of interest startup
        // remember to give him tape recorders

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
    }
	
	// Update is called once per frame
	void Update () {

		time += Time.deltaTime;

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

        /// State switcher / trigger machine - this has the rules for enabling and disabling behaviours
        foreach (GameObject r in tapeRecorders)
        {
			if (r.GetComponent<AudioSource>().isPlaying && r != null && searchObject == null)
            {
				Debug.Log("searching for recorder " + r + " is now recorder " + investigateRecorder);
				investigateRecorder = r;
				state = "investigate"; // instance to determine source of sound and where to go
            }

        }

        
        /// 


        // Generic Monster behaviours
        DimLights();

    }

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

    public void Search()
    {
		// do search
		// play search animation
		// if player found
		// go to chase
		// else
		// go to patrol
    }

    public void Chase()
	{
		// if presence is still within my circle
		// continue to chase player
		// play chase animation
		// // if player within attack range
		// // call instance of Attack() [use cooldown too]
		// else
		// go to patrol
    }

	public void Attack()
	{
		// play attack animation
		// deal damage & remove health
		// fade to black on controller
		// teleport player
		// teleport monster
		// fade back
	}

	// an instance function to look at the tape playing's closest search point
    public void Investigate()
    {
		if (investigateRecorder != null)
		{
			float[] searchDistances = new float[searchDestinations.Length];

			Debug.Log("new distances instanced " + searchDistances);

			for (int s = 0; s < searchDestinations.Length; s++)
			{
				searchDistances[s] = Vector3.Distance(investigateRecorder.transform.position, searchDestinations[s].transform.position);
			}

			Debug.Log("new distances assigned " + searchDistances);

			float toDistance = Mathf.Min(searchDistances);

			Debug.Log("todistance is " + toDistance);

			for (int s = 0; s < searchDestinations.Length; s++)
			{
				if (searchDistances[s] == toDistance)
				{
					searchObject = searchDestinations[s];

					Debug.Log("now investigating sound at " + searchObject);
					state = "investigateSound";
				}
			}
		}
        


    }

    public void InvestigateSound()
    {
		// go to searchDestination as searchObject
		if (!searchObject.GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
		{
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

	public void OpenNearbyDoor()
	{
		actionTime += Time.deltaTime;

		float[] doorDistances = new float[doors.Length];

		Debug.Log("new door distances instanced " + doorDistances);

		for (int s = 0; s < doors.Length; s++)
		{
			doorDistances[s] = Vector3.Distance(gameObject.transform.position, doors[s].transform.position);
		}

		Debug.Log("new door distances assigned " + doorDistances);

		float toDistance = Mathf.Min(doorDistances);

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
