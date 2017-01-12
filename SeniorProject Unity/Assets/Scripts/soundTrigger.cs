using UnityEngine;
using System.Collections;

public class soundTrigger : MonoBehaviour
{

	public AudioSource sound;
	public bool debug;
	public bool isArmed = true;

	public bool lookTrip = false;

	public bool multiTrip = false;
	public float multiTripResetTime = 5;
    
	bool soundTrip = false;

    void OnTriggerEnter(Collider other)
    {
		if (isArmed && other.gameObject.CompareTag("GameController") && !lookTrip)
		{
			if (soundTrip == false)
			{
				if (debug)
					Debug.Log(gameObject + " is Playing");
				
				soundTrip = true;
				sound.Play();

				if (multiTrip)
					StartCoroutine(endSound());
			}
		}

		else 
		{
			if (debug)
				Debug.Log(gameObject + " armed state is " + isArmed);
		}
    }

	IEnumerator endSound()
	{
		yield return new WaitForSeconds(multiTripResetTime);
		soundTrip = false;
	}

	public void arm()
	{ 
		isArmed = true;
		if (debug)
			Debug.Log(gameObject + " is now armed: " + isArmed);
	}

	public void lookTrigger()
	{
		if (!soundTrip && lookTrip && isArmed)
		{
			if (debug)
				Debug.Log(gameObject + " is Playing");

			soundTrip = true;
			sound.Play();

			if (multiTrip)
				StartCoroutine(endSound());
		}
	}
}
