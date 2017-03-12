using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class documentCabinet : MonoBehaviour {

	public bool debug = false;
	public bool cabinetOpen = false;
	public bool cabinetLocked = true;
	public bool interactSpace;

	public GameObject doorOpen;

	public GameObject eExitLight;
	public GameObject eExitLightRed;
    public GameObject[] activeObjects;

	// UI
	Renderer render;

	public GameObject ui;
	private Text uiText;
	public float interactDistance = 3.0f;

	// Use this for initialization
	void Start () {
		render = gameObject.GetComponent<Renderer>();
		uiText = ui.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		// UI State
		if (!cabinetLocked)
		{

			if (!cabinetOpen)
			{
				uiText.text = "Press A to open";
			}
			else
			{

                uiText.text = "You've retrieved the documents!";
			}
		}
		else
		{
			uiText.text = "Cabinet locked";
		}

		// We remove UI if the player walks away
		if (!interactSpace)
		{
			uiText.enabled = false;
		}

	}

	public void OnTriggerEnter()
	{
		interactSpace = true;
		if (debug)
			Debug.Log("Entered cabinet interactSpace " + gameObject);
	}

	public void OnTriggerExit()
	{
		interactSpace = false;
		if (debug)
			Debug.Log("Exited cabinet interactSpace " + gameObject);
	}

	public void lookingAtMe()
	{
        if (debug)
            Debug.Log("Looking At Document Cabinet " + gameObject);

        // We give a GUI queue here on the door mesh
		uiText.enabled = true;

		if (interactSpace)
		{
			if (Input.GetButtonDown("Action"))
			{
				if (!cabinetOpen && !cabinetLocked)
				{
					// Unlock Emergency Doors
					doorOpen.GetComponent<doorMaster>().unLock();
					doorOpen.GetComponent<doorMaster>().forceOpen();

					// Turn off Emergency Light
					eExitLight.SetActive(false);

					// Turn on Emergency Climax Light
					eExitLightRed.SetActive(true);

                    foreach(GameObject g in activeObjects)
                    {
                        // flips active state
                        if (g.gameObject.activeSelf)
                            g.gameObject.SetActive(false);
                        else
                            g.gameObject.SetActive(true);
                    }

                    cabinetOpen = true;
				}
			}
		}
	}

	public void unLock()
	{
		cabinetLocked = false;
	}

}