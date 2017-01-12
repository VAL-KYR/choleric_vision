using UnityEngine;
using System.Collections;

public class keySpawn : MonoBehaviour {

    public GameObject[] spawns;
	public float locSwitch;

	// Use this for initialization
	void Start () {
		locSwitch = Random.value;

        spawns = GameObject.FindGameObjectsWithTag("keySpawn");

        foreach (GameObject s in spawns)
        {
            // dynamic spawn allocation
        }

		if (locSwitch >= 0 & locSwitch <= 0.33)
		{
            gameObject.transform.position = spawns[0].transform.position;
		}
		if (locSwitch > 0.33 & locSwitch <= 0.66)
		{
            gameObject.transform.position = spawns[1].transform.position;
        }
		if (locSwitch > 0.66 & locSwitch <= 1)
		{
            gameObject.transform.position = spawns[2].transform.position;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
