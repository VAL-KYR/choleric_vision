using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VR;
using System.Collections;
using System;



public class triggerVoice : MonoBehaviour {

    // Voiceline or flashback
    public bool flashbackTrigger = false;
    public bool voiceLineTrigger = true;

    // VoiceLine code
    private GameObject voiceBox;

    public bool triggerVoiceOnce = true;
    private bool voiceTriggered;
    // VoiceLine code

    // Flashback code
    public AudioClip flashback;
    public AudioClip voiceLine;

    public bool flashbackTripped = false;

    // Use this for initialization
    void Start () {
        voiceBox = GameObject.FindGameObjectWithTag("voice");
        
        // EITHER LINE OR FLASHBACK NOT BOTH
        /*
        if (flashbackTrigger && voiceLineTrigger)
        {
            voiceLineTrigger = false;
        }
        */

        voiceTriggered = false;
        flashbackTripped = false;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController"))
        {
            if (flashbackTrigger)
            {
                if (!flashbackTripped)
                {
                    // call function in voices.cs
                    GameObject.FindGameObjectWithTag("voice").GetComponent<voices>().startFlashback(flashback);
                    flashbackTripped = true;
                }
            }


            if (voiceLineTrigger)
            {
                if (triggerVoiceOnce)
                {
                    if (!voiceTriggered)
                    {
                        voiceBox.GetComponent<voices>().voiceLinePlay(voiceLine);
                        voiceTriggered = true;
                    }
                }
                else
                {
                    voiceBox.GetComponent<voices>().voiceLinePlay(voiceLine);
                }
            }
            
            
        }
    }
}
