using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scareLockedInRoom : MonoBehaviour {

    public List<GameObject> doors;
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

        currentSound = scareSounds[Random.Range(0, scareSounds.Length)];

        scareSounds = GameObject.FindGameObjectsWithTag("lockedScareSound");
    }
	
	// Update is called once per frame
	void Update () {

        time = time + Time.deltaTime;

        if(readyToLock)
        {
            lockT = lockT + Time.deltaTime;
        }

        if (lockT > 1 && readyToLock)
        {
            for (int t = 0; t < doors.Count; t++)
            {
                if (!doors[t].GetComponent<doorAnimator>().doorOpen)
                {
                    doors[t].GetComponent<doorAnimator>().forceLock();
                }
            }

            readyToLock = false;
            //lockT = 0;
        }

        if(time > lockReleaseTime && !readyToLock && previousLockState)
        {
            for (int t = 0; t < doors.Count; t++)
            {
                doors[t].GetComponent<doorAnimator>().unLock();
                readyToLock = false;
                previousLockState = readyToLock;

                lockT = 0;
            }
        }


        if (!currentSound.GetComponent<AudioSource>().isPlaying && lockT > 0)
        {
            currentSound = scareSounds[Random.Range(0, scareSounds.Length)];
            currentSound.GetComponent<AudioSource>().Play();
        }



    }

    void OnTriggerEnter(Collider entity)
    {
        if (entity.gameObject.CompareTag("GameController")){
            if (time > lockReadyTime)
            {
                if (!readyToLock)
                {
                    for (int t = 0; t < doors.Count; t++)
                    {
                        if (doors[t].GetComponent<doorAnimator>().doorOpen)
                        {
                            doors[t].GetComponent<doorAnimator>().forceClosed();
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
