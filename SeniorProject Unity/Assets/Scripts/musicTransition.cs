using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class musicTransition : MonoBehaviour
{
	public AudioMixerSnapshot currentSnapshot;
	public AudioMixerSnapshot newSnapshot;
	public float transitionTime = 13.0f;

	//public AudioSource currentMusic;
	//public AudioSource newMusic;
	bool musicTrigger = false;

	void OnTriggerEnter()
	{
		if (musicTrigger == false)
		{
			musicTrigger = true;
			newSnapshot.TransitionTo(transitionTime);
		}
	}
}