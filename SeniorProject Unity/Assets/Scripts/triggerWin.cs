using UnityEngine;
using System.Collections;

public class triggerWin : MonoBehaviour {

    public GameObject winZone;
    public GameObject tapePlayMusic;
    public GameObject tapePlayStory;
    public bool playerWin = false;

	// Use this for initialization
	void Start () {
        winZone = GameObject.FindGameObjectWithTag("winPoint");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController") && !playerWin)
        {
            c.gameObject.GetComponent<controller>().playerWin = true;
            tapePlayMusic.GetComponent<tapeMaster>().remote();
            tapePlayStory.GetComponent<tapeMaster>().remote();
            playerWin = true;
        }
    }
}
