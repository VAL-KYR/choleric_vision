using UnityEngine;
using System.Collections;

public class footsteps : MonoBehaviour {
	AudioSource stepper;

	public float stepDelay = 1;
	private float currentStepDelay;
	private float stepWalkDelay;
	private float stepCrouchDelay;
	private float stepSprintDelay;
    private GameObject player;

	public AudioClip[] walkSteps;
	public AudioClip[] crouchSteps;
	public AudioClip[] sprintSteps;

	public AudioClip currentStep;
	public AudioClip walkStep;
	public AudioClip crouchStep;
	public AudioClip sprintStep;



	// Use this for initialization
	void Start() {
		stepper = gameObject.GetComponent<AudioSource>();
		stepWalkDelay = stepDelay;
		stepCrouchDelay = stepDelay * 1.5f;
		stepSprintDelay = stepDelay * 0.3f;

		walkSteps = Resources.LoadAll<AudioClip>("Footsteps/Walking");
		crouchSteps = Resources.LoadAll<AudioClip>("Footsteps/Crouching");
		sprintSteps = Resources.LoadAll<AudioClip>("Footsteps/Sprinting");
		Resources.UnloadUnusedAssets();

		walkStep = walkSteps[Random.Range(0, walkSteps.Length)];
		crouchStep = crouchSteps[Random.Range(0, crouchSteps.Length)];
		sprintStep = sprintSteps[Random.Range(0, sprintSteps.Length)];

		currentStep = walkStep;
		currentStepDelay = stepWalkDelay;
		stepper.clip = currentStep;

		player = GameObject.FindGameObjectWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {

		//if the player is moving do steps
		if (player.GetComponent<controller>().moveDirection.x != 0 || player.GetComponent<controller>().moveDirection.z != 0)
		{

			stepDelay -= Time.deltaTime;

			//choose step type based on player movement mode
			if (player.GetComponent<controller>().playerSprint)
			{
				currentStep = sprintStep;
				currentStepDelay = stepSprintDelay;
			}
			else if (player.GetComponent<controller>().crouch)
			{
				currentStep = crouchStep;
				currentStepDelay = stepCrouchDelay;
			}
			else
			{
				currentStep = walkStep;
				currentStepDelay = stepWalkDelay;
			}

			//trigger step after delay
			if (stepDelay <= 0)
			{
				stepper.clip = currentStep;

				stepper.Play();

				walkStep = walkSteps[Random.Range(0, walkSteps.Length)];
				crouchStep = crouchSteps[Random.Range(0, crouchSteps.Length)];
				sprintStep = sprintSteps[Random.Range(0, sprintSteps.Length)];

				stepDelay = currentStepDelay;
			}
			//stepper.clip = walkSteps[Random.Range(0, walkSteps.Length)];
			//StartCoroutine(Example());
		}
	}

	/*
	IEnumerator Example()
	{
		print(Time.time);
		stepper.clip = walkSteps[Random.Range(0, walkSteps.Length)];
		yield return new WaitForSeconds(Time.deltaTime*40);
		print(Time.time);
	}
	*/
}
