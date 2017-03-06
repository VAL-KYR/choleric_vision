using UnityEngine;
using System.Collections;
using System;

public class MonsterAI : MonoBehaviour {

    public NavMeshAgent agent;

    public bool debug = false;

    // destination points
    public GameObject[] patrolDestinations;
    public GameObject[] searchDestinations;
    int destinationIterator = 0;
    int x = 0;

    // Objects of interest
    public GameObject[] tapeRecorders;
    public GameObject investigateRecorder;
    public GameObject searchObject;


    // lights
    GameObject[] unaturalLights;
    float[] unaturalLightIntensity;
    

    public bool monsterGone = false;
    public bool statePatrol = true;
    public bool stateSearch = false;
    public bool stateChase = false;
    public bool stateInvestigate = false;
    public bool stateInvestigateSound = false;

    private Vector3 lastPosition;
    private Vector3 velocity;

    // Use this for initialization
    void Start () {
        // objects of interest startup
        // remember to give him tape recorders

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
        statePatrol = true;
        stateSearch = false;
        stateChase = false;
        stateInvestigate = false;
        stateInvestigateSound = false;
    }
	
	// Update is called once per frame
	void Update () {

        /// State runner - chooses appropriate function for state, or instances another instant action function (like attack)
        if (statePatrol)
        {
            // stop other states
            stateSearch = false;
            stateChase = false;
            stateInvestigate = false;
            stateInvestigateSound = false;

            Patrol();
        }
        else if (stateSearch)
        {
            // stop other states
            statePatrol = false;
            stateChase = false;
            stateInvestigate = false;
            stateInvestigateSound = false;

            Search();
        }
        else if (stateChase)
        {
            // stop other states
            statePatrol = false;
            stateSearch = false;
            stateInvestigate = false;
            stateInvestigateSound = false;

            Chase();
        }
        else if (stateInvestigateSound)
        {
            // stop other states
            statePatrol = false;
            stateSearch = false;
            stateChase = false;
            stateInvestigate = false;

            InvestigateSound();
        }
        ///

        /// State switcher / trigger machine - this has the rules for enabling and disabling behaviours
        foreach (GameObject r in tapeRecorders)
        {
            if (r.GetComponent<AudioSource>().isPlaying && investigateRecorder == null)
            {
                Debug.Log("searching for recorder " + investigateRecorder);
                investigateRecorder = r;
                Investigate(); // instance to determine source of sound and where to go
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

    }

    public void Chase()
    {

    }

    public void Investigate()
    {
        float[] searchDistances = null;

        for(int s = 0; s < searchDestinations.Length; s++)
        {
            searchDistances[s] = Vector3.Distance(investigateRecorder.transform.position, searchDestinations[s].transform.position);
        }

        float toDistance = Mathf.Min(searchDistances);

        for (int s = 0; s < searchDestinations.Length; s++)
        {
            if (searchDistances[s] == toDistance)
            {
                searchObject = searchDestinations[s];
                stateInvestigateSound = true;
            }
        }




    }

    public void InvestigateSound()
    {
        // go to searchDestination as searchObject
        // stop the current recorder and investigateRecorder.forceStop();
        // set investigateRecorder = null
        // execute search with stateSearch
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
