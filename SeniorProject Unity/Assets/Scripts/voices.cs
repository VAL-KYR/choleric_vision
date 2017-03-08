using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VR;
using System.Collections;
using System;

public class voices : MonoBehaviour {

    // VoiceLine code
    private AudioSource flashbackOrigin;

    // Flashback code
    private AudioSource voiceBox;
    GameObject vrCam;
    GameObject nonVrCam;

    [System.Serializable]
    public class flashbackManager
    {
        public bool start;
        public bool done;

        public bool voicePlayed;
        public bool musicPlayed;
        public float deSatFactor;

        public AudioMixerSnapshot flashbackSnapshot;
        public AudioMixerSnapshot areaMusicSnapshot;

        public float transitionTime = 8.0f;

        public float deSatTime = 0.5f;
    }
    public flashbackManager flashback = new flashbackManager();
    // Flashback code

    // Use this for initialization
    void Start () {
        flashbackOrigin = gameObject.GetComponent<AudioSource>();
        voiceBox = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();

        // Flashback code
        if (!VRSettings.enabled)
            nonVrCam = GameObject.FindGameObjectWithTag("NonVRCam");
        else
            vrCam = GameObject.FindGameObjectWithTag("VRCam");

        flashback.voicePlayed = false;
        flashback.musicPlayed = false;
        flashback.deSatFactor = 0.0f;
        flashback.done = false;
        flashback.start = false;


        if (!VRSettings.enabled)
        {
            nonVrCam.GetComponent<ColorCurvesManager>().Factor = 0.0f;
            nonVrCam.GetComponent<ColorCurvesManager>().SaturationA = 1.0f;
        }

        else
        {
            vrCam.GetComponent<ColorCurvesManager>().Factor = 0.0f;
            vrCam.GetComponent<ColorCurvesManager>().SaturationA = 1.0f;
        }
        //
    }

    // Update is called once per frame
    void Update () {

        if (!VRSettings.enabled)
        {
            if (!nonVrCam)
                nonVrCam = GameObject.FindGameObjectWithTag("NonVRCam");

            flashback.deSatFactor = nonVrCam.GetComponent<ColorCurvesManager>().SaturationA;
        }
        else
        {
            if (!vrCam)
                vrCam = GameObject.FindGameObjectWithTag("VRCam");

            flashback.deSatFactor = vrCam.GetComponent<ColorCurvesManager>().SaturationA;
        }


        // Start flashback routines
        if (flashback.start)
        {
            // if the flashback isn't done doing it's thing it will start
            if (!flashback.done)
            {
                ApplyVisualEffects();
                PlayFlashbackAudio();
            }

            // if the flashback is done it will slowly remove the sounds and visual effects
            else if (flashback.done)
            {
                RemoveVisualEffects();
                flashback.areaMusicSnapshot.TransitionTo(flashback.transitionTime);

                // reset for new flashback command
                if(flashback.deSatFactor >= 1.0f)
                {
                    flashback.start = false;
                }
            }

        }
        

    }

    /// <summary>
    /// ALL FLASHBACK ROUTINES ///////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public void startFlashback (AudioClip flashbackAudio)
    {
        // can only start a new flashback if the old one is done
        if (!flashback.start)
        {
            flashbackOrigin.Stop();
            flashbackOrigin.clip = flashbackAudio;
            flashback.start = true;
        }
        
    }

    public void PlayFlashbackAudio()
    {
        // plays once the message from the flashback trigger  IS TRIPPED BY VISUALS
        if (!flashback.voicePlayed && flashback.deSatFactor < 0.6f)
        {
            flashbackOrigin.Play();
            flashback.voicePlayed = true;
        }

        // plays once the flashback background music  IS TRIPPED BY VISUALS
        if (!flashback.musicPlayed && !flashback.done && flashback.deSatFactor < 0.1f)
        {
            flashback.flashbackSnapshot.TransitionTo(flashback.transitionTime);
            flashback.musicPlayed = true;
        }

        // Sends that flashback is ready to end  IS TRIPPED BY VOICE ENDING
        if (flashback.voicePlayed && !flashbackOrigin.isPlaying)
        {
            flashback.done = true;
        }

    }

    public void ApplyVisualEffects()
    {
        if (!VRSettings.enabled)
        {
            nonVrCam.GetComponent<ColorCurvesManager>().SaturationA = Mathf.Lerp(nonVrCam.GetComponent<ColorCurvesManager>().SaturationA, 0.0f, flashback.deSatTime * Time.deltaTime);
        }
        else
        {
            vrCam.GetComponent<ColorCurvesManager>().SaturationA = Mathf.Lerp(vrCam.GetComponent<ColorCurvesManager>().SaturationA, 0.0f, flashback.deSatTime * Time.deltaTime);
        }


    }

    public void RemoveVisualEffects()
    {
        if (!VRSettings.enabled)
        {
            nonVrCam.GetComponent<ColorCurvesManager>().SaturationA = Mathf.Lerp(nonVrCam.GetComponent<ColorCurvesManager>().SaturationA, 1.0f, flashback.deSatTime * Time.deltaTime);
        }
        else
        {
            vrCam.GetComponent<ColorCurvesManager>().SaturationA = Mathf.Lerp(vrCam.GetComponent<ColorCurvesManager>().SaturationA, 1.0f, flashback.deSatTime * Time.deltaTime);
        }
    }
    /// <summary>
    /// ALL FLASHBACK ROUTINES ///////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>





    // plays the voiceline given by the trigger
    public void voiceLinePlay(AudioClip voiceLine)
    {
        voiceBox.clip = voiceLine;
        voiceBox.Play();
    }

    public void playerHit()
    {

    }

    public void playerAwake()
    {

    }

    public void playerKO()
    {

    }





}
