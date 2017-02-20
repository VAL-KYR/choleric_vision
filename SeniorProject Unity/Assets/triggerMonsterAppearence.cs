using UnityEngine;
using System.Collections;

public class triggerMonsterAppearence : MonoBehaviour {

    public GameObject monster;

    // Use this for initialization
    void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider c)
    {
        if(c.CompareTag("GameController") && !monster.activeSelf)
        {
            monster.SetActive(true);
        }
    }
}
