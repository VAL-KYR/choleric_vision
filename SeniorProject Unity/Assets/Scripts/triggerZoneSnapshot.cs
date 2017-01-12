using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class triggerZoneSnapshot : MonoBehaviour {

	public GameObject playerC;

	public bool debug;
	public bool playerInZone = false;
	public bool active = false;
	public bool kill = true;

	public GameObject activationSound;

	public AudioMixerSnapshot inactiveSnapshot;
	public AudioMixerSnapshot normalSnapshot;
	public AudioMixerSnapshot effectsSnapshot;
	public float transitionTime = 1.75f;
	public float activationTime = 5.0f;

	// Use this for initialization
	void Start()
	{
		effectsSnapshot.TransitionTo(transitionTime);
	}

	// Update is called once per frame
	void Update()
	{
		if (activationSound != null)
		{
			if (!active)
			{
				inactiveSnapshot.TransitionTo(0.1f);
			}

			if (activationSound.GetComponent<AudioSource>().isPlaying)
			{
				if (playerInZone)
					normalSnapshot.TransitionTo(activationTime);
				else
					effectsSnapshot.TransitionTo(activationTime);
				
				active = true;
			}

			// could set second condition to just be a short time
			else if (active && !activationSound.GetComponent<AudioSource>().isPlaying)
			{
				if (playerInZone && !kill)
				{
					normalSnapshot.TransitionTo(transitionTime);
					if (debug)
						Debug.Log(gameObject + " is in zone: " + normalSnapshot);
				}

				else if (!playerInZone && !kill)
				{
					effectsSnapshot.TransitionTo(transitionTime);
					kill = true;
					if (debug)
						Debug.Log(gameObject + " is in zone: " + effectsSnapshot);
				}
			}
		}

		else 
		{
			if (playerInZone && !kill)
			{
				normalSnapshot.TransitionTo(transitionTime);
				if (debug)
					Debug.Log(gameObject + " is in zone: " + normalSnapshot);
			}

			else if (!playerInZone && !kill)
			{
				effectsSnapshot.TransitionTo(transitionTime);
				kill = true;
				if (debug)
					Debug.Log(gameObject + " is in zone: " + effectsSnapshot);
			}
		}

		if (debug)
			Debug.Log(gameObject + " is in zone: " + playerInZone);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("GameController"))
		{
			playerInZone = true;
			kill = false;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("GameController"))
		{
			playerInZone = false;
		}
	}
}
