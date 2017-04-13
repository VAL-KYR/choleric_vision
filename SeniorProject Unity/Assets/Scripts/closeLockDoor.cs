using UnityEngine;
using System.Collections;

public class closeLockDoor : MonoBehaviour {

    public GameObject[] doors;
    public bool doorsClosed = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject d in doors)
        {
            if (d.GetComponent<doorMaster>().doorOpen = false)
            {
                doorsClosed = true;
            }

            if (doorsClosed)
            {
                d.GetComponent<doorMaster>().forceLock();
                d.GetComponent<doorMaster>().doorHasKey = false;
            }
        }
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController") && !doorsClosed)
        {
            foreach (GameObject d in doors)
            {
                d.GetComponent<doorMaster>().forceClosed();
            }
        }
    }
}
