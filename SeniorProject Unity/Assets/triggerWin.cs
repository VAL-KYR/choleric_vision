using UnityEngine;
using System.Collections;

public class triggerWin : MonoBehaviour {

    public GameObject winZone;

	// Use this for initialization
	void Start () {
        winZone = GameObject.FindGameObjectWithTag("winPoint");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController"))
        {
            c.gameObject.transform.position = winZone.transform.position;
        }
    }
}
