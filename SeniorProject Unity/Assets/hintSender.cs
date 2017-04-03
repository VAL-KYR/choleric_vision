using UnityEngine;
using System.Collections;

public class hintSender : MonoBehaviour {

    public string hintName;
    public float hintTime;
    public bool hintDisplayed = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController") && !hintDisplayed)
        {
            c.GetComponent<hinter>().hint = hintName;
            c.GetComponent<hinter>().displayTime = hintTime;
            hintDisplayed = true;
        }
    }
}
