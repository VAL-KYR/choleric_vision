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
	public GameObject activeObject;

	// Use this for initialization
	void Start()
	{
		
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
			if (enableOrDisableObject)
			{
				if (!activeObject.activeSelf)
					activeObject.SetActive(true);
				if (activeObject.activeSelf)
					activeObject.SetActive(false);
			}

			lights.SetActive(true);
			keySpawn.SetActive(true);

			activated = true;

		}

		if (debug)
			Debug.Log("Cieling Collapse " + activated + lookAtDist);
	}
}
