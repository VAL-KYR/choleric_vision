using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scareLockedInRoom : MonoBehaviour {

    public List<GameObject> doors;
    public bool lockOnce = false;
    public bool killKey = false;
    public GameObject keyKill;
    public bool playSounds = true;
    public float time = 0;
    public float lockT = 0;
    public bool readyToLock = false;
    public bool previousLockState;
    public float lockReleaseTime = 10;
    public float lockReadyTime = 40;
    public bool shutUpScares = false;

    public GameObject[] scareSounds;
    GameObject currentSound;

    // Use this for initialization
    void Start () {
        previousLockState = readyToLock;
        lockOnce = false;

        if (scareSounds.Length > 0)
            currentSound = scareSounds[Random.Range(0, scareSounds.Length)];
    }
	
	// Update is called once per frame
	void Update () {

        time = time + Time.deltaTime;

        if (!lockOnce)
        {
            if (readyToLock)
            {
                lockT = lockT + Time.deltaTime;

                // get rid of keys
                if (killKey)
                {
                    for (int t = 0; t < doors.Count; t++)
                    {
                        keyKill.SetActive(false);
                    }
                }
            }

            if (lockT > 1 && readyToLock && !lockOnce)
            {
                for (int t = 0; t < doors.Count; t++)
                {
                    if (!doors[t].GetComponent<doorMaster>().doorOpen)
                    {
                        doors[t].GetComponent<doorMaster>().forceLock();
                    }
                }

                readyToLock = false;
                //lockT = 0;
                //lockOnce = true;
            }

            if (time > lockReleaseTime && !readyToLock && previousLockState)
            {
                for (int t = 0; t < doors.Count; t++)
                {
                    doors[t].GetComponent<doorMaster>().unLock();
                    readyToLock = false;
                    previousLockState = readyToLock;

                    lockOnce = true;
                    lockT = 0;
                }
            }


            // play scare sounds
            if (playSounds)
            {
                if (!currentSound.GetComponent<AudioSource>().isPlaying && scareSounds.Length > 0 && lockT > 0)
                {
                    currentSound = scareSounds[Random.Range(0, scareSounds.Length)];
                    currentSound.GetComponent<AudioSource>().Play();
                }
            }
        }

    }

    void OnTriggerEnter(Collider entity)
    {
        if (entity.gameObject.CompareTag("GameController")){
            if (time > lockReadyTime && !lockOnce)
            {
                if (!readyToLock)
                {
                    for (int t = 0; t < doors.Count; t++)
                    {
                        if (doors[t].GetComponent<doorMaster>().doorOpen)
                        {
                            doors[t].GetComponent<doorMaster>().forceClosed();
                            readyToLock = true;
                            previousLockState = readyToLock;

                            lockT = 0;
                            time = 0;
                        }
                    }
                }
                
                
            }
        }
       

    }
}
