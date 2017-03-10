using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class musicTransition : MonoBehaviour
{
	//public AudioMixerSnapshot currentSnapshot;
	public AudioMixerSnapshot newSnapshot;
	public float transitionTime = 13.0f;

	bool musicTrigger = false;

	void OnTriggerEnter(Collider c)
	{
        //Debug.Log("Collider Entered " + c);
        if (!musicTrigger && c.CompareTag("GameController"))
		{
			musicTrigger = true;
            Debug.Log("Music changed");
            newSnapshot.TransitionTo(transitionTime);
		}
	}
}