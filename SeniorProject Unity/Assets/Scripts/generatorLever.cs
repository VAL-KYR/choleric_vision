﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class generatorLever : MonoBehaviour {
    AudioSource lever;

    public bool debug;
    public bool interactSpace;
    public GameObject generatorAnim;
    public AudioClip[] leverSounds;
    public AudioClip leverSound;
    private bool leverThrown = false;

    Renderer render;
    public bool eventObjectsActivate;
    public GameObject[] eventObjects;
    public GameObject ui;
    public GameObject generatorPowerOn;
    public GameObject lights;
    public GameObject elevator;
    private Text uiText;
    public float interactDistance = 3.0f;

    Animator anim;
    AnimatorStateInfo leverState;

    // For triggering later
    static int doorInteractHash = Animator.StringToHash("leverPull");

    // these are named after the actual layer in unity for the state machine
    static int offStateHash = Animator.StringToHash("Base Layer.off");
    static int onStateHash = Animator.StringToHash("Base Layer.on");
    static int doneStateHash = Animator.StringToHash("Base Layer.done");

    // Use this for initialization
    void Start()
    {
        lever = gameObject.GetComponent<AudioSource>();
        leverSounds = Resources.LoadAll<AudioClip>("SoundEffects/GeneratorLever");
        lever.clip = leverSounds[Random.Range(0, leverSounds.Length)];

        anim = generatorAnim.GetComponent<Animator>();
        render = gameObject.GetComponent<Renderer>();
        uiText = ui.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // accessing lever states
        leverState = anim.GetCurrentAnimatorStateInfo(0);

        // Activate Generator on stuff
        if (leverState.fullPathHash == doneStateHash)
        {
            if (!generatorPowerOn.GetComponent<AudioSource>().isPlaying)
            {
                generatorPowerOn.GetComponent<AudioSource>().Play();
            }

            // Activate event game objects
            if (eventObjectsActivate && eventObjects.Length > 0)
            {
                foreach (GameObject g in eventObjects)
                {
                    // flips active state
                    if (g.gameObject.activeSelf)
                    {
                        g.gameObject.SetActive(false);
                        eventObjectsActivate = false;
                    }

                    else
                    {
                        g.gameObject.SetActive(true);
                        eventObjectsActivate = false;
                    }
                        
                }
            }

            // Power these
            lights.SetActive(true);
            elevator.GetComponent<elevatorMaster>().PowerSupplied();
        }

        if (leverState.fullPathHash == doneStateHash && !leverThrown)
        {
            // lever click sound
            lever.Play();
            leverThrown = true;
        }
    }

    public void lookingAtMe(float lookAtDist)
    {

        //if (lookAtDist <= interactDistance && !buttonPushed)
        if (lookAtDist <= interactDistance && leverState.fullPathHash == offStateHash)
        {

            uiText.enabled = true;

            if (Input.GetButtonDown("Action"))
            {
                // Trigger Animation
                anim.SetTrigger("leverPull");
            }

        }

        else
        {
            uiText.enabled = false;
        }

        if (debug)
            Debug.Log("Looking At Generator " + gameObject);
    }
}
