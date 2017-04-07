using UnityEngine;
using System.Collections;

public class triggerMonsterAppearence : MonoBehaviour {

    public GameObject monster;
    public GameObject[] blockingDoors;
    private bool isTripped;

    // Use this for initialization
    void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController") && !monster.activeSelf && !isTripped)
        {
            monster.SetActive(true);

            // if there are blocking doors open them
            if (blockingDoors.Length > 0)
            {
                OpenBlockingDoors();
            }

            isTripped = true;
        }
    }

    // this will open any doors that could block the encounter
    // MAYBE include dooropen check?
    void OpenBlockingDoors()
    {
        foreach (GameObject d in blockingDoors)
        {
            if (d.GetComponent<doorMaster>().doorLocked)
                d.GetComponent<doorMaster>().unLock();
            if (!d.GetComponent<doorMaster>().doorOpen)
                d.GetComponent<doorMaster>().forceOpen();
        }
    }
}
