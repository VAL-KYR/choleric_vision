using UnityEngine;
using System.Collections;

public class flashback : MonoBehaviour {

    // audio file
    public GameObject flashbackAudioObject;
    public GameObject flashbackMusicObject;
    AudioSource flashbackAudioSource;
    AudioSource flashbackMusicSource;
    public AudioClip flashbackAudio;
    public AudioClip flashbackMusic;
    // visual effects
    public GameObject vrCam;
    public GameObject nonVrCam;
    public bool flashbackTripped = false;
    public bool flashbackDone = false;

    // Use this for initialization
    void Start () {
        vrCam = GameObject.FindGameObjectWithTag("vrCam");
        nonVrCam = GameObject.FindGameObjectWithTag("NonVRCam");

        if (flashbackAudioObject.GetComponent<AudioSource>() == null)
        {
            flashbackAudioObject.AddComponent<AudioSource>();
            flashbackAudioSource = flashbackAudioObject.GetComponent<AudioSource>();
        }
        if (flashbackMusicObject.GetComponent<AudioSource>() == null)
        {
            flashbackMusicObject.AddComponent<AudioSource>();
            flashbackMusicSource = flashbackMusicObject.GetComponent<AudioSource>();
        }

        flashbackAudioSource.clip = flashbackAudio;
        flashbackMusicSource.clip = flashbackMusic;

        flashbackDone = false;

        nonVrCam.GetComponent<ColorCorrectionCurves>();
    }
	
	// Update is called once per frame
	void Update () {

        if (flashbackAudioSource.isPlaying)
        {
            ApplyVisualEffects();
        }

        else
        {
            RemoveVisualEffects();
        }

    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController") && !flashbackTripped)
        {
            flashbackTripped = true;
            PlayFlashbackAudio();
        }
    }

    public void PlayFlashbackAudio()
    {
        flashbackAudioSource.Play();
        flashbackMusicSource.Play();
    }

    public void ApplyVisualEffects()
    {

    }

    public void RemoveVisualEffects()
    {

    }
}
