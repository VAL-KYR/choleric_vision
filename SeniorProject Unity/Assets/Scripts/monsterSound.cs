using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class monsterSound : MonoBehaviour {
    // Sound object source
    public GameObject monsterVoiceBox;
    AudioSource monsterVoice;
    //

    // setting up the mixerGroup
    public AudioMixerGroup voiceOutput;
    //

    // Sound file arrays
    AudioClip[] monsterAlertSounds;
    AudioClip monsterAlertSound;

    AudioClip[] monsterAttackSounds;
    AudioClip monsterAttackSound;

    AudioClip[] monsterGrowlSounds;
    AudioClip monsterGrowlSound;

    AudioClip[] monsterNoticeSounds;
    AudioClip monsterNoticeSound;

    AudioClip[] monsterRambleSounds;
    AudioClip monsterRambleSound;

    AudioClip[] monsterSearchSounds;
    AudioClip monsterSearchSound;
    //

    // Voice function variables
    bool voiceQueue;
    string voiceQueuedUtterance;
    float voiceQueuedDelay;
    //

    // UI stuff that the player can mess with
    public bool voiceWarmup = false;
    public bool devVoiceTesting = false;
    int testVoice = 0;
    //





    /// <summary>
    /// YOU CAN CALL VOICES IN OTHER SCRIPTS WITH THIS
    /// [TheMonsterScriptsObject].GetComponent<monsterSound>().Voice([string utterance], [float delay], [bool waitForSilence]);
    /// 
    /// </summary>

    // Use this for initialization
    void Start () {
        // Finding the mixerGroup
        //
        //

        // Load All sound files from resources
        monsterAlertSounds = Resources.LoadAll<AudioClip>("Monster/Alert");
        monsterAlertSound = monsterAlertSounds[Random.Range(0, monsterAlertSounds.Length)];

        monsterAttackSounds = Resources.LoadAll<AudioClip>("Monster/Attack");
        monsterAttackSound = monsterAttackSounds[Random.Range(0, monsterAttackSounds.Length)];

        monsterGrowlSounds = Resources.LoadAll<AudioClip>("Monster/Growl");
        monsterGrowlSound = monsterGrowlSounds[Random.Range(0, monsterGrowlSounds.Length)];

        monsterNoticeSounds = Resources.LoadAll<AudioClip>("Monster/Notice");
        monsterNoticeSound = monsterNoticeSounds[Random.Range(0, monsterNoticeSounds.Length)];

        monsterRambleSounds = Resources.LoadAll<AudioClip>("Monster/Ramble");
        monsterRambleSound = monsterRambleSounds[Random.Range(0, monsterRambleSounds.Length)];

        monsterSearchSounds = Resources.LoadAll<AudioClip>("Monster/Search");
        monsterSearchSound = monsterSearchSounds[Random.Range(0, monsterSearchSounds.Length)];
        //

        // Create or load AudioSource
        if (!monsterVoiceBox.GetComponent<AudioSource>())
        {
            monsterVoiceBox.AddComponent<AudioSource>();
        }
        monsterVoice = monsterVoiceBox.GetComponent<AudioSource>();
        //

        // Configure AudioSource
        monsterVoice.playOnAwake = false;
        monsterVoice.dopplerLevel = 0.0f;
        monsterVoice.spatialBlend = 1.0f;
        monsterVoice.maxDistance = 30.0f;
        monsterVoice.minDistance = 1.0f;
        monsterVoice.rolloffMode = AudioRolloffMode.Linear;
        monsterVoice.outputAudioMixerGroup = voiceOutput;
        //

        // Extra Dev options config
        //devVoiceTesting = false;
        //

        // Voice function variables
        voiceQueue = false;
        voiceQueuedUtterance = "nothing";
        voiceQueuedDelay = 0.0f;
        //


    }

    // Update is called once per frame
    void Update () {

        // Test the voice if the user has chosen to start with a voice warmup
        if (voiceWarmup)
        {
            VoiceTest();
        }
        //

        // Sends the queued voice info over and over until it is played in the waitForSilence if version and voiceQueue becomes false
        if (voiceQueue)
        {
            if (!monsterVoice.isPlaying)
            {
                Voice(voiceQueuedUtterance, voiceQueuedDelay, true);
            }
            
        }

        // Developer voice test buttons
        if (devVoiceTesting)
        {
            if (Input.GetButtonDown("DevVoice1"))
            {
                Voice("alert", 0.0f, true);
            }
            if (Input.GetButtonDown("DevVoice2"))
            {
                Voice("attack", 0.0f, true);
            }
            if (Input.GetButtonDown("DevVoice3"))
            {
                Voice("growl", 0.0f, true);
            }
            if (Input.GetButtonDown("DevVoice4"))
            {
                Voice("notice", 0.0f, true);
            }
            if (Input.GetButtonDown("DevVoice5"))
            {
                Voice("ramble", 0.0f, true);
            }
            if (Input.GetButtonDown("DevVoice6"))
            {
                Voice("search", 0.0f, true);
            }
        }

    }

    // Re-randomize all the sounds from the file arrays
    public void RandomizeSounds()
    {
        monsterAlertSound = monsterAlertSounds[Random.Range(0, monsterAlertSounds.Length)];
        monsterAttackSound = monsterAttackSounds[Random.Range(0, monsterAttackSounds.Length)];
        monsterGrowlSound = monsterGrowlSounds[Random.Range(0, monsterGrowlSounds.Length)];
        monsterNoticeSound = monsterNoticeSounds[Random.Range(0, monsterNoticeSounds.Length)];
        monsterRambleSound = monsterRambleSounds[Random.Range(0, monsterRambleSounds.Length)];
        monsterSearchSound = monsterSearchSounds[Random.Range(0, monsterSearchSounds.Length)];
    }

    // Test the voice by cycling once through each utterance type
    public void VoiceTest()
    {
        RandomizeSounds();

        if (!monsterVoice.isPlaying && testVoice == 0)
        {
            monsterVoice.clip = monsterAlertSound;
        }

        if (!monsterVoice.isPlaying && testVoice == 1)
        {
            monsterVoice.clip = monsterAttackSound;
        }

        if (!monsterVoice.isPlaying && testVoice == 2)
        {
            monsterVoice.clip = monsterGrowlSound;
        }

        if (!monsterVoice.isPlaying && testVoice == 3)
        {
            monsterVoice.clip = monsterNoticeSound;
        }   

        if (!monsterVoice.isPlaying && testVoice == 4)
        {
            monsterVoice.clip = monsterRambleSound;
        }

        if (!monsterVoice.isPlaying && testVoice == 5)
        {
            monsterVoice.clip = monsterSearchSound;
        }

        if (!monsterVoice.isPlaying)
        {
            monsterVoice.Play();

            testVoice++;

            if (testVoice > 5)
            {
                testVoice = 0;
                voiceWarmup = false;
            }  
        }

    }

    // The Voice Functions
    public void Voice(string utterance, float delay, bool waitForSilence)
    {

        if (!voiceQueue)
        {
            RandomizeSounds();

            if (utterance == "alert")
                monsterVoice.clip = monsterAlertSound;

            if (utterance == "attack")
                monsterVoice.clip = monsterAttackSound;

            if (utterance == "growl")
                monsterVoice.clip = monsterGrowlSound;

            if (utterance == "notice")
                monsterVoice.clip = monsterNoticeSound;

            if (utterance == "ramble")
                monsterVoice.clip = monsterRambleSound;

            if (utterance == "search")
                monsterVoice.clip = monsterSearchSound;
        }
        

        if (waitForSilence)
        {
            Debug.Log("Waiting for silence");

            if (!monsterVoice.isPlaying && voiceQueue)
            {
                    Debug.Log("Playing Queued Sound");
                    monsterVoice.PlayDelayed(delay);
                    voiceQueue = false;
            }
            else
            {
                Debug.Log("Queued sound is waiting");

                if (!voiceQueue)
                {
                    Debug.Log("Creating a Voice Queue");
                    voiceQueuedUtterance = utterance;
                    voiceQueuedDelay = delay;
                    voiceQueue = true;
                }
            }
        }
        else
        {
            if (!voiceQueue)
            {
                monsterVoice.PlayDelayed(delay);
            }
        }

    }


}
