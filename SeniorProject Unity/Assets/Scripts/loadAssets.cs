using UnityEngine;
using System.Collections;

public class loadAssets : MonoBehaviour {

    public GameObject[] unloadThese;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("GameController"))
        {
            foreach (GameObject g in unloadThese)
            {
                if(!g.activeSelf)
                    g.SetActive(true);
            }
        }
        
    }
}
