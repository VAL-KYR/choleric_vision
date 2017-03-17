using UnityEngine;
using System.Collections;

public class clothSounds : MonoBehaviour {

    private GameObject activeMonster;

    private AudioSource clothes;

    public AudioClip[] getUp;
    public AudioClip[] fallDown;

    // Use this for initialization
    void Start () {
        clothes = GetComponent<AudioSource>();
        getUp = Resources.LoadAll<AudioClip>("Player/GettingUp");
        fallDown = Resources.LoadAll<AudioClip>("Player/FallingDown");
    }
	
	// Update is called once per frame
	void Update () {
        // Monster GET
        if (GameObject.FindGameObjectWithTag("Monster"))
        {
            activeMonster = GameObject.FindGameObjectWithTag("Monster");
        }


    }

    public void Fall()
    {
        clothes.clip = fallDown[Random.Range(0, fallDown.Length - 1)];
        clothes.Play();
    }

    public void Rise()
    {
        clothes.clip = getUp[Random.Range(0, getUp.Length - 1)];
        clothes.Play();
    }
}
