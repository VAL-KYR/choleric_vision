using UnityEngine;
using System.Collections;

public class elevatorMaster : MonoBehaviour {
    // Key audio compenent for all key sounds
    AudioSource elevatorSource;

    // elevator ready sounds
    public AudioClip[] bellSounds;
    public AudioClip bellSound;
    public AudioClip eleStop;
	public GameObject soundDone;
    private bool bellSounded = false;

    public GameObject playerC;
    public GameObject triggerMonsterAppearence;
    public GameObject triggerSeeMonster;
    public GameObject triggerSecondMonsterAppearence;
    public GameObject triggerSecondSeeMonster;
    public GameObject eleLight;

    public bool debug;

	public bool playerInElevator = false;
	public bool powerOn = false;
    public float elevatorStopTime = 7.0f;

    // Movement Variables
    public int currPos = 0;
    public float time = 0.0f;
    public GameObject[] positions;
    //

    // Tape Variables
    public GameObject[] silenceTapes;
    //

    public GameObject rUpperGate;
    public GameObject lUpperGate;

    public GameObject rLowerGate;
    public GameObject lLowerGate;


    public GameObject uRGateStart;
    public GameObject uLGateStart;
    public GameObject uRGateEnd;
    public GameObject uLGateEnd;


    public GameObject lRGateStart;
    public GameObject lLGateStart;
    public GameObject lRGateEnd;
    public GameObject lLGateEnd;

    private bool uGateMove = false;
	private bool uGateMoveBack = false;
	private bool lGateMove = false;
	private bool lGateMoveBack = false;
	private bool gateState = false;
    private bool eleTop;
    private bool onEle;
    private bool eleDone;
    private bool moveEle;
    public bool monsterGone = false;

    // Use this for initialization
    void Start () {
        // bell sound gen
        elevatorSource = gameObject.GetComponent<AudioSource>();
        bellSounds = Resources.LoadAll<AudioClip>("SoundEffects/Elevator/Bell");
        bellSound = bellSounds[Random.Range(0, bellSounds.Length)];

        eleTop = true;
        moveEle = false;
        onEle = false;
        eleDone = true;

        // setting up position markers
        foreach(GameObject g in positions)
        {
            if (g.GetComponent<SphereCollider>() == null)
            {
                g.AddComponent<SphereCollider>();
                g.GetComponent<SphereCollider>().radius = 0.5f;
                g.GetComponent<SphereCollider>().isTrigger = true;
            }
        }
        //
    }

	// Update is called once per frame
	void Update()
	{

        // Hecka Good movement code
        // IF ELEVATOR ON
        if(currPos < positions.Length && !gateState && moveEle)
        {
            if (!positions[currPos].GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
            {
                transform.position = Vector3.Slerp(transform.position, positions[currPos].transform.position, 0.3f * Time.deltaTime);
            }
            else
            {
                currPos++;
            }
        }
        //
        if(monsterGone && playerInElevator && !gateState && powerOn && currPos <= 1)
        {
            moveEle = true;
        }
        else if (monsterGone && playerInElevator && !gateState && powerOn && currPos >= 2 && time < elevatorStopTime)
        {
            moveEle = false;
            time += Time.deltaTime;

            if (!elevatorSource.isPlaying && elevatorSource.clip == bellSound)
            {
                elevatorSource.clip = eleStop;
                elevatorSource.Play();
                elevatorSource.spatialBlend = 0.0f;
                eleLight.GetComponent<Light>().color = new Color(1, 0, 0, 1);
            }
            
            
        }
        else if (monsterGone && playerInElevator && !gateState && powerOn && currPos >= 2 && time > elevatorStopTime && !elevatorSource.isPlaying)
        {
            moveEle = true;
            bellSounded = false;
            elevatorSource.spatialBlend = 1.0f;
            eleLight.GetComponent<Light>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            moveEle = false;
        }


        //unaturalLights[j].GetComponent<flashLightOnOff>().flashPeriod = 2.0f;
        //unaturalLights[j].GetComponent<flashLightOnOff>().flashing = true;

        if (currPos == 0 && powerOn && !playerInElevator)
		{
			uGateMoveBack = false;
			uGateMove = true;
		}
		else if (currPos == 0 && powerOn && playerInElevator) 
		{
			uGateMove = false;
			uGateMoveBack = true;
        }
        else if (currPos >= positions.Length && powerOn && playerInElevator)
        {
            lGateMove = true;
            lGateMoveBack = false;
            uGateMove = true;
            uGateMoveBack = false;
        }

        if (lGateMove)
		{
            lLowerGate.transform.position = Vector3.Slerp(lLowerGate.transform.position, lLGateEnd.transform.position, 0.5f * Time.deltaTime);
            rLowerGate.transform.position = Vector3.Slerp(rLowerGate.transform.position, lRGateEnd.transform.position, 0.5f * Time.deltaTime);
        }

		if (uGateMove)
		{
            lUpperGate.transform.position = Vector3.Lerp(lUpperGate.transform.position, uLGateEnd.transform.position, 0.5f * Time.deltaTime);
            rUpperGate.transform.position = Vector3.Lerp(rUpperGate.transform.position, uRGateEnd.transform.position, 0.5f * Time.deltaTime);
		}

		if (uGateMoveBack)
		{
            lUpperGate.transform.position = Vector3.Lerp(lUpperGate.transform.position, uLGateStart.transform.position, 0.5f * Time.deltaTime);
            rUpperGate.transform.position = Vector3.Lerp(rUpperGate.transform.position, uRGateStart.transform.position, 0.5f * Time.deltaTime);
		}

        /// Monster upper appearence trigger on/off
        if (!gateState && currPos == 0 && playerInElevator)
        {
            triggerMonsterAppearence.SetActive(true);
            triggerSeeMonster.SetActive(true);
        }

        /// Monster lower appearence trigger on/off
        if (gateState && currPos >= positions.Length && playerInElevator)
        {
            triggerSecondMonsterAppearence.SetActive(true);
            triggerSecondSeeMonster.SetActive(true);

            if (!bellSounded)
            {
                elevatorSource.clip = bellSound;
                elevatorSource.Play();
                elevatorSource.pitch = 0.9f;

                bellSounded = true; 
                eleLight.GetComponent<Light>().color = new Color(1, 0, 0, 1);
            }
        }
        else
        {
            triggerSecondMonsterAppearence.SetActive(false);
            triggerSecondSeeMonster.SetActive(false);
        }

        /// Test to see if gates at bottom and top are open
        if (((lUpperGate.transform.position - uLGateStart.transform.position).magnitude <= 0.2f) && ((lLowerGate.transform.position - lLGateStart.transform.position).magnitude <= 0.2f))
        {
            gateState = false;
        }
        else
        {
            gateState = true;
        }

        /// MonsterGone Test
        if (triggerMonsterAppearence.GetComponent<triggerMonsterAppearence>().monster.GetComponent<MonsterMoveTest>().monsterGone)
            monsterGone = true;
    }

	void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.CompareTag("GameController"))
        {
            playerInElevator = true;

            // SILENCE PLAYING TAPE RECORDERS
            SilenceTapes();
        }
			
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("GameController"))
			playerInElevator = false;
	}

    // silences any tapes playing in the array so long as array is larger than 0
    public void SilenceTapes()
    {
        if (silenceTapes.Length > 0)
        {
            foreach (GameObject t in silenceTapes)
            {
                if (t.GetComponent<tapeMaster>().GetComponent<AudioSource>().isPlaying)
                {
                    t.GetComponent<tapeMaster>().forceStop();
                }
            }
        }
        
    }

	public void PowerSupplied()
	{
		powerOn = true;
        if (!bellSounded && currPos == 0)
        {
            elevatorSource.clip = bellSound;
            elevatorSource.Play();
            bellSounded = true;
        }

    }

}
