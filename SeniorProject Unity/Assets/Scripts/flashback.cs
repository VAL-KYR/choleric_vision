using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Audio;
using System.Collections;

public class flashback : MonoBehaviour {

    // visual effects
    public GameObject vrCam;
    public GameObject nonVrCam;
    public bool flashbackTripped = false;
    public bool flashbackDone = false;
	public bool voicePlayed;

	public AudioMixerSnapshot flashbackSnapshot;
	public AudioMixerSnapshot areaMusicSnapshot;

	public float transitionTime = 8.0f;
	public bool musicPlayed;
	public float deSatFactor;
	public float deSatTime = 0.5f;

    // Use this for initialization
    void Start () {
		
        if (!VRSettings.enabled)
			nonVrCam = GameObject.FindGameObjectWithTag("NonVRCam");
		else
			vrCam = GameObject.FindGameObjectWithTag("VRCam");
		
		voicePlayed = false;
		musicPlayed = false;
        flashbackDone = false;
		deSatFactor = 0.0f;

		if (!VRSettings.enabled)
			nonVrCam.GetComponent<ColorCurvesManager>().Factor = 0.0f;
		else
			vrCam.GetComponent<ColorCurvesManager>().Factor = 0.0f;
		
    }
	
	// Update is called once per frame
	void Update () {

		if (!VRSettings.enabled)
		{
			if(!nonVrCam)
				nonVrCam = GameObject.FindGameObjectWithTag("NonVRCam");
			
			deSatFactor = nonVrCam.GetComponent<ColorCurvesManager>().Factor;
		}
		else
		{
			if (!vrCam)
				vrCam = GameObject.FindGameObjectWithTag("VRCam");
			
			deSatFactor = vrCam.GetComponent<ColorCurvesManager>().Factor;
		}


		if (flashbackTripped && !flashbackDone)
		{
			ApplyVisualEffects();
			PlayFlashbackAudio();
		}

		else if (flashbackDone && flashbackTripped)
		{
			RemoveVisualEffects();
			areaMusicSnapshot.TransitionTo(transitionTime);
			Debug.Log("flashback bleed out");
		}

    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController") && !flashbackTripped)
        {
            flashbackTripped = true;
        }
    }

    public void PlayFlashbackAudio()
	{
		// plays once
		if (!voicePlayed && deSatFactor > 0.6f) 
		{
			gameObject.GetComponent<AudioSource>().Play();
			voicePlayed = true;
			Debug.Log("voice played");
		}

		if (!musicPlayed && !flashbackDone && deSatFactor > 0.1f)
		{
			flashbackSnapshot.TransitionTo(transitionTime);
			musicPlayed = true;
			Debug.Log("music played");
		}

		if (voicePlayed && !gameObject.GetComponent<AudioSource>().isPlaying)
		{
			flashbackDone = true;
			Debug.Log("flashback over");
		}
        
    }

    public void ApplyVisualEffects()
    {
		if (!VRSettings.enabled)
		{
			nonVrCam.GetComponent<ColorCurvesManager>().Factor = Mathf.Lerp(nonVrCam.GetComponent<ColorCurvesManager>().Factor, 1.0f, deSatTime * Time.deltaTime);
		}
		else
		{
			vrCam.GetComponent<ColorCurvesManager>().Factor = Mathf.Lerp(vrCam.GetComponent<ColorCurvesManager>().Factor, 1.0f, deSatTime * Time.deltaTime);
		}


    }

    public void RemoveVisualEffects()
    {
		if (!VRSettings.enabled)
		{
			nonVrCam.GetComponent<ColorCurvesManager>().Factor = Mathf.Lerp(nonVrCam.GetComponent<ColorCurvesManager>().Factor, 0.0f, deSatTime * Time.deltaTime);
		}
		else
		{
			vrCam.GetComponent<ColorCurvesManager>().Factor = Mathf.Lerp(vrCam.GetComponent<ColorCurvesManager>().Factor, 0.0f, deSatTime * Time.deltaTime);
		}
    }
}
