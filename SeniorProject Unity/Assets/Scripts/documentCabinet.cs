using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class documentCabinet : MonoBehaviour {

	public bool debug = false;
	public bool cabinetOpen = false;
	public bool cabinetLocked = true;
	public bool interactSpace;

	public GameObject doorOpen;
	public GameObject doorOpenAlt;

	public GameObject eExitLight;
	public GameObject eExitLightRed;

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

	public void lookingAtMe(float lookAtDist)
	{
		// We give a GUI queue here on the door mesh
		if (lookAtDist <= interactDistance)
		{
			uiText.enabled = true;
		}
		else
		{
			uiText.enabled = false;
		}

		if (interactSpace)
		{
			if (Input.GetButtonDown("Action"))
			{
				if (!cabinetOpen)
				{
					// Unlock Emergency Doors
					doorOpen.GetComponent<doorAnimator>().unLock();
					doorOpen.GetComponent<doorAnimator>().forceOpen();
					doorOpenAlt.GetComponent<doorAnimator>().unLock();
					doorOpenAlt.GetComponent<doorAnimator>().forceOpen();

					// Turn off Emergency Light
					eExitLight.SetActive(false);

					// Turn on Emergency Climax Light
					eExitLightRed.SetActive(true);

					cabinetOpen = true;
				}
			}
		}

		if (debug)
			Debug.Log("Looking At Document Cabinet " + gameObject);
	}

	public void unLock()
	{
		cabinetLocked = false;
	}

}