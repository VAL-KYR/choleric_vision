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
    AudioClip lastMonsterAlertSound;

    AudioClip[] monsterAttackSounds;
    AudioClip monsterAttackSound;
    AudioClip lastMonsterAttackSound;

    AudioClip[] monsterGrowlSounds;
    AudioClip monsterGrowlSound;
    AudioClip lastMonsterGrowlSound;

    AudioClip[] monsterNoticeSounds;
    AudioClip monsterNoticeSound;
    AudioClip lastMonsterNoticeSound;

    AudioClip[] monsterRambleSounds;
    AudioClip monsterRambleSound;
    AudioClip lastMonsterRambleSound;

    AudioClip[] monsterSearchSounds;
    AudioClip monsterSearchSound;
    AudioClip lastMonsterSearchSound;
    //

    // Voice function variables
    bool voiceQueue;
    string voiceQueuedUtterance;
    float voiceQueuedDelay;
    //

    // UI stuff that the player can mess with
    public bool debug = false;
    public bool voiceWarmup = false;
    public bool devVoiceTesting = false;
    int testVoice = 0;
    public float time = 0.0f;
    public float playTime = 2.0f;
    public float playTimeVariance = 0.0f;
    public float randPlayTime = 0.0f;
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

        // Confguring rest period
        time = 0.0f;
        randPlayTime = 0.0f;
        //

        // Load All sound files from resources
        monsterAlertSounds = Resources.LoadAll<AudioClip>("Monster/Alert");
        monsterAlertSound = monsterAlertSounds[Random.Range(0, monsterAlertSounds.Length)];
        lastMonsterAlertSound = monsterAlertSound;

        monsterAttackSounds = Resources.LoadAll<AudioClip>("Monster/Attack");
        monsterAttackSound = monsterAttackSounds[Random.Range(0, monsterAttackSounds.Length)];
        lastMonsterAttackSound = monsterAttackSound;

        monsterGrowlSounds = Resources.LoadAll<AudioClip>("Monster/Growl");
        monsterGrowlSound = monsterGrowlSounds[Random.Range(0, monsterGrowlSounds.Length)];
        lastMonsterGrowlSound = monsterGrowlSound;

        monsterNoticeSounds = Resources.LoadAll<AudioClip>("Monster/Notice");
        monsterNoticeSound = monsterNoticeSounds[Random.Range(0, monsterNoticeSounds.Length)];
        lastMonsterNoticeSound = monsterNoticeSound;

        monsterRambleSounds = Resources.LoadAll<AudioClip>("Monster/Ramble");
        monsterRambleSound = monsterRambleSounds[Random.Range(0, monsterRambleSounds.Length)];
        lastMonsterRambleSound = monsterRambleSound;

        monsterSearchSounds = Resources.LoadAll<AudioClip>("Monster/Search");
        monsterSearchSound = monsterSearchSounds[Random.Range(0, monsterSearchSounds.Length)];
        lastMonsterSearchSound = monsterSearchSound;
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

        time += Time.deltaTime;
        randPlayTime = playTime + Random.Range(playTimeVariance, playTimeVariance * -1);

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
        int alertSoundsChoice = Random.Range(0, monsterAlertSounds.Length);
        int attackSoundsChoice = Random.Range(0, monsterAttackSounds.Length);
        int growlSoundsChoice = Random.Range(0, monsterGrowlSounds.Length);
        int noticeSoundsChoice = Random.Range(0, monsterNoticeSounds.Length);
        int rambleSoundsChoice = Random.Range(0, monsterRambleSounds.Length);
        int searchSoundsChoice = Random.Range(0, monsterSearchSounds.Length);

        monsterAlertSound = monsterAlertSounds[alertSoundsChoice];
        monsterAttackSound = monsterAttackSounds[attackSoundsChoice];
        monsterGrowlSound = monsterGrowlSounds[growlSoundsChoice];
        monsterNoticeSound = monsterNoticeSounds[noticeSoundsChoice];
        monsterRambleSound = monsterRambleSounds[rambleSoundsChoice];
        monsterSearchSound = monsterSearchSounds[searchSoundsChoice];

        if(lastMonsterAlertSound == monsterAlertSound)
        {
            if(alertSoundsChoice <= monsterAlertSounds.Length && alertSoundsChoice > 0)
                monsterAlertSound = monsterAlertSounds[alertSoundsChoice - 1];
            else if (alertSoundsChoice == 0)
                monsterAlertSound = monsterAlertSounds[monsterAlertSounds.Length];
        }
        if (lastMonsterAttackSound == monsterAttackSound)
        {
            if (attackSoundsChoice <= monsterAttackSounds.Length && attackSoundsChoice > 0)
                monsterAttackSound = monsterAttackSounds[attackSoundsChoice - 1];
            else if (attackSoundsChoice == 0)
                monsterAttackSound = monsterAttackSounds[monsterAttackSounds.Length];
        }
        if (lastMonsterGrowlSound == monsterGrowlSound)
        {
            if (growlSoundsChoice <= monsterGrowlSounds.Length && growlSoundsChoice > 0)
                monsterGrowlSound = monsterGrowlSounds[growlSoundsChoice - 1];
            else if (growlSoundsChoice == 0)
                monsterGrowlSound = monsterGrowlSounds[monsterGrowlSounds.Length];
        }
        if (lastMonsterNoticeSound == monsterNoticeSound)
        {
            if (noticeSoundsChoice <= monsterNoticeSounds.Length && noticeSoundsChoice > 0)
                monsterNoticeSound = monsterNoticeSounds[noticeSoundsChoice - 1];
            else if (noticeSoundsChoice == 0)
                monsterNoticeSound = monsterNoticeSounds[monsterNoticeSounds.Length];
        }
        if (lastMonsterRambleSound == monsterRambleSound)
        {
            if (rambleSoundsChoice <= monsterRambleSounds.Length && rambleSoundsChoice > 0)
                monsterRambleSound = monsterRambleSounds[rambleSoundsChoice - 1];
            else if (rambleSoundsChoice == 0)
                monsterRambleSound = monsterRambleSounds[monsterRambleSounds.Length];
        }
        if (lastMonsterSearchSound == monsterSearchSound)
        {
            if (searchSoundsChoice <= monsterSearchSounds.Length && searchSoundsChoice > 0)
                monsterSearchSound = monsterSearchSounds[searchSoundsChoice - 1];
            else if (searchSoundsChoice == 0)
                monsterSearchSound = monsterSearchSounds[monsterSearchSounds.Length];
        }
        
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
        if (utterance == "attack")
        {
            RandomizeSounds();
            monsterVoice.clip = monsterAttackSound;
            monsterVoice.PlayDelayed(delay);
            lastMonsterAttackSound = monsterAttackSound;
        }
        else if (utterance != "attack" && time > randPlayTime && !monsterVoice.isPlaying)
        {
            if (!voiceQueue)
            {
                RandomizeSounds();
            }

            if (utterance == "alert")
            {
                monsterVoice.clip = monsterAlertSound;
                lastMonsterAlertSound = monsterAlertSound;
            }
               
            if (utterance == "growl")
            {
                monsterVoice.clip = monsterGrowlSound;
                lastMonsterGrowlSound = monsterGrowlSound;
            }
               
            if (utterance == "notice")
            {
                monsterVoice.clip = monsterNoticeSound;
                lastMonsterNoticeSound = monsterNoticeSound;
            }
                
            if (utterance == "ramble")
            {
                monsterVoice.clip = monsterRambleSound;
                lastMonsterRambleSound = monsterRambleSound; 
            }   

            if (utterance == "search")
            {
                monsterVoice.clip = monsterSearchSound;
                lastMonsterRambleSound = monsterRambleSound;
            }
               
            //}


            if (waitForSilence)
            {
                if (debug)
                    Debug.Log("Waiting for silence");

                if (!monsterVoice.isPlaying && voiceQueue)
                {
                    if (debug)
                        Debug.Log("Playing Queued Sound");
                    monsterVoice.PlayDelayed(delay);
                    voiceQueue = false;
                }
                else
                {
                    if (debug)
                        Debug.Log("Queued sound is waiting");

                    if (!voiceQueue)
                    {
                        if (debug)
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

            time = 0.0f;
        }


    }


}
