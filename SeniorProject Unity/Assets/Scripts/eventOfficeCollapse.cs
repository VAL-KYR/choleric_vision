using UnityEngine;
using System.Collections;

public class eventOfficeCollapse : MonoBehaviour {

	public bool debug;
	public bool interactSpace;
	public bool activated = false;

	public GameObject cielingCollapse;
	public GameObject lights;
	public GameObject keySpawn;
	public float interactDistance = 3.0f;

	public bool enableOrDisableObject = false;
	public GameObject[] activeObject;
    //public bool activeOnce = false;

	// Use this for initialization
	void Start()
	{
        //activeOnce = false;

    }

	// Update is called once per frame
	void Update()
	{
		
	}

	public void lookingAtMe(float lookAtDist)
	{

		if (lookAtDist <= interactDistance && !activated)
		{
			
			if (!cielingCollapse.GetComponent<AudioSource>().isPlaying)
			{
				cielingCollapse.GetComponent<AudioSource>().Play();
			}

			// Flip Active state of despawnObject
			//if (enableOrDisableObject && !activeOnce)
            if (enableOrDisableObject)
            {
                foreach(GameObject g in activeObject)
                {
                    if (!g.activeSelf)
                        g.SetActive(true);
                    else
                        g.SetActive(false);
                }

                //activeOnce = true;

            }

			lights.SetActive(true);
			keySpawn.SetActive(true);

			activated = true;

		}

		if (debug)
			Debug.Log("Cieling Collapse " + activated + lookAtDist);
	}
}
