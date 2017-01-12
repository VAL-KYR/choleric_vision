using UnityEngine;
using System.Collections;

public class playerZoneFlag : MonoBehaviour {

    public bool playerFlag = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider Entity)
    {
        if(Entity.CompareTag("GameController"))
        {
            playerFlag = true;
        }
    }
}
