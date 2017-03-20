using UnityEngine;
using System.Collections;

public class breathing : MonoBehaviour {

    private GameObject player;
    private AudioSource lungs;
    private AudioClip[] normalBreaths;
    private AudioClip[] panicBreaths;
    private AudioClip[] exhaustedBreaths;


    public float playerPresence;
    public float breathDelay = 2.0f;
    public float panicBreath = 0.84f;
    public float elevatedBreath = 0.4f;
    //public float idleBreath = 0.4f;
    private float breathReset;
    

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("GameController");
        normalBreaths = Resources.LoadAll<AudioClip>("Player/Breathing/Idle");
        exhaustedBreaths = Resources.LoadAll<AudioClip>("Player/Breathing/Exhausted");
        panicBreaths = Resources.LoadAll<AudioClip>("Player/Breathing/Panicked");
        lungs = gameObject.GetComponent<AudioSource>();

        breathReset = breathDelay;
        playerPresence = player.GetComponent<Presence>().curPres;
	}
	
	// Update is called once per frame
	void Update () {
        playerPresence = player.GetComponent<Presence>().curPres;

        // Breath spaceing
        breathDelay -= (Time.deltaTime * playerPresence);

        // Breath delay and play only is silent
        if (breathDelay <= 0 && !lungs.isPlaying)
        {
            // IDLE BREATH
            if (playerPresence < elevatedBreath)
            {
                lungs.clip = normalBreaths[Random.Range(0, normalBreaths.Length)];
            }
            // EXHAUSTED OR ELEVATED BREATH
            else if (playerPresence >= elevatedBreath && playerPresence < panicBreath)
            {
                lungs.clip = exhaustedBreaths[Random.Range(0, exhaustedBreaths.Length)];
            }
            // PANICKED BREATH
            else if (playerPresence >= panicBreath)
            {
                lungs.clip = panicBreaths[Random.Range(0, panicBreaths.Length)];
            }
            
            lungs.Play();
            breathDelay = breathReset;
        }

    }
}
