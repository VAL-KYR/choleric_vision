using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VR;
using System.Collections;
using System;



public class triggerVoice : MonoBehaviour {

    /// For Triggering Flashbacks or Voice Lines
    // Voiceline or flashback
    [System.Serializable]
    public class Narrative
    {
        // VoiceLine code
        public GameObject voiceBox;

        public bool flashbackTrigger = false;
        public bool voiceLineTrigger = true;

        public bool triggerVoiceOnce = true;
        
        // VoiceLine code

        // Flashback code
        public AudioClip flashback;
        public AudioClip voiceLine;

        public bool flashbackTriggered = false;
        public bool voiceTriggered;
    }
    public Narrative narrative = new Narrative();
    /// For Triggering Flashbacks or Voice Lines

    // Use this for initialization
    void Start () {
        /// For Triggering Flashbacks or Voice Lines
        narrative.voiceBox = GameObject.FindGameObjectWithTag("voice");

        narrative.voiceTriggered = false;
        narrative.flashbackTriggered = false;
        /// For Triggering Flashbacks or Voice Lines
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController"))
        {
            if (narrative.flashbackTrigger)
            {
                if (!narrative.flashbackTriggered)
                {
                    // call function in voices.cs
                    narrative.voiceBox.GetComponent<voices>().startFlashback(narrative.flashback);
                    narrative.flashbackTriggered = true;
                }
            }


            if (narrative.voiceLineTrigger)
            {
                if (narrative.triggerVoiceOnce)
                {
                    if (!narrative.voiceTriggered)
                    {
                        narrative.voiceBox.GetComponent<voices>().voiceLinePlay(narrative.voiceLine);
                        narrative.voiceTriggered = true;
                    }
                }
                else
                {
                    narrative.voiceBox.GetComponent<voices>().voiceLinePlay(narrative.voiceLine);
                }
            }
            
            
        }
    }
}
