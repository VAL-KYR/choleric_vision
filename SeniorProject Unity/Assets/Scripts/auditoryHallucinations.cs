using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class auditoryHallucinations : MonoBehaviour {

    public GameObject playerHeart;

    public GameObject[] soundLocations;
    GameObject currentSoundLocation;
    public AudioClip[] scareSounds;
    AudioClip currentSound;


    //AuditoryHallucinations
    public bool doOnce = false;
    public bool quietFlag = false;
    public float playTime = 10.0f;
    public float time = 0.0f;

    public bool somethingIsPlaying = false;


    // Use this for initialization
    void Start () {
        somethingIsPlaying = false;
        quietFlag = false;
        scareSounds = Resources.LoadAll<AudioClip>("AuditoryHallucinations");
        currentSound = scareSounds[Random.Range(0, scareSounds.Length)];
    }
	
	// Update is called once per frame
	void Update () {

        time = time + Time.deltaTime;

        for (int i = 0; i < soundLocations.Length; i++)
        {
            if (soundLocations[i].GetComponent<AudioSource>().isPlaying)
            {
                // Reset time to space stuff out and confirm something is playing
                somethingIsPlaying = true;
            }
        }

        if (somethingIsPlaying)
        {
            time = 0;

            int silence = 0;

            for (int i = 0; i < soundLocations.Length; i++)
            {
                if (!soundLocations[i].GetComponent<AudioSource>().isPlaying)
                {
                    silence++;
                }
            }

            if (silence >= soundLocations.Length)
            {
                somethingIsPlaying = false;
            }
        }

        // Conditions to start a new sound
        // Convert to fade in fade out snapshots later
        //if (time > playTime && !somethingIsPlaying && playerHeart.GetComponent<heartBeatThump>().playerPanic)
        //if (time > playTime && !somethingIsPlaying)
        if (time > playTime && !somethingIsPlaying && playerHeart.GetComponent<heartBeatThump>().playerPanic)
        {
            currentSound = scareSounds[Random.Range(0, scareSounds.Length)];
            currentSoundLocation = soundLocations[Random.Range(0, soundLocations.Length)];
            currentSoundLocation.GetComponent<AudioSource>().clip = currentSound;
            currentSoundLocation.GetComponent<AudioSource>().Play();
        }

    }
}
