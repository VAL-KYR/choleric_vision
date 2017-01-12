using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class heartBeatThump : MonoBehaviour {
	AudioSource heartBeater;
	private GameObject player;

	// Raise/Lower volume in HR
	public AudioMixerSnapshot restingHR;
	public AudioMixerSnapshot elevatedHR;
	public float transitionTime = 5.0f;

	public float beatDelay = 1;
	private float beatReset;

	public int currHeartRate;
	public int avgHeartRate;
	public float restDifference;
    public bool heartListening = false;

	// Threshold of restDifference to change HR volume
	public float elevatedThreshold = 1.2f;

	// trigger volume in HR
	public bool elevatedTrigger;
	public bool restingTrigger;

	// Use this for initialization
	void Start () {
		beatReset = beatDelay;

		elevatedTrigger = false;
		restingTrigger = false;

		player = GameObject.FindGameObjectWithTag("GameController");

		// Resting from calibration
		avgHeartRate = player.GetComponent<heartBeat>().avBeatPerMin;

		heartBeater = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

		if (avgHeartRate == 0)
		{
			// Resting from calibration
			avgHeartRate = player.GetComponent<heartBeat>().avBeatPerMin;
		}

		// Current heart rate with some exclusion
		if (player.GetComponent<heartBeat>().curBeatPerMin > 40)
		{
			currHeartRate = player.GetComponent<heartBeat>().curBeatPerMin;

			// Cheap and easy difference from resting
			restDifference = (float)currHeartRate / (float)avgHeartRate;
		}

		// elevate HR Sound
		if (restDifference >= elevatedThreshold && !elevatedTrigger)
		{
			elevateSoundHR();
		}
		else if (restDifference < elevatedThreshold && !restingTrigger)
		{
			restingSoundHR();
		}

        // elevate HR for listening
        if (heartListening)
            elevateSoundHR();
        else
            restingSoundHR();

        // Beat pacing
        beatDelay -= (Time.deltaTime * restDifference);

		// Beat reset
		if (beatDelay <= 0)
		{
			heartBeater.Play();
			beatDelay = beatReset;
		}
	}

	void elevateSoundHR()
	{
		elevatedHR.TransitionTo(transitionTime);
		elevatedTrigger = true;
		restingTrigger = false;
	}

	void restingSoundHR()
	{
		restingHR.TransitionTo(transitionTime);
		restingTrigger = true;
		elevatedTrigger = false;
	}
}
