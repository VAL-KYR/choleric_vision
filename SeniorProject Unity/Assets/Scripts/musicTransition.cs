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
        if (musicTrigger == false && c.CompareTag("GameController"))
		{
			musicTrigger = true;
			newSnapshot.TransitionTo(transitionTime);
		}
	}
}