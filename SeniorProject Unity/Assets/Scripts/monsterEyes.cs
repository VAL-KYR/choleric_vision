using UnityEngine;
using System.Collections;

public class monsterEyes : MonoBehaviour {

    public bool playerSeen;

	// Use this for initialization
	void Start () {
        playerSeen = false;
        
	}
	
	// Update is called once per frame
	void Update () {
  
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController"))
        {
            playerSeen = true;
        }
    }
}
