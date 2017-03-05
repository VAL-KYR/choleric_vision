using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class generatorButton : MonoBehaviour {

	public bool debug;
	public bool interactSpace;
	public bool buttonPushed = false;

	Renderer render;
    public bool eventObjectsActivate;
    public GameObject[] eventObjects;
    public GameObject ui;
	public GameObject generatorPowerOn;
	public GameObject lights;
	public GameObject elevator;
	private Text uiText;
	public float interactDistance = 3.0f;

	// Use this for initialization
	void Start () {
		render = gameObject.GetComponent<Renderer>();
		uiText = ui.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void lookingAtMe(float lookAtDist)
	{

		if (lookAtDist <= interactDistance && !buttonPushed)
		{
			uiText.enabled = true;

			if (Input.GetButtonDown("Action"))
			{
				// This will be replaced with an animation				transform.Translate(Vector3.back * Time.deltaTime, Space.World);

				if (!generatorPowerOn.GetComponent<AudioSource>().isPlaying)
				{
					generatorPowerOn.GetComponent<AudioSource>().Play();
				}

                // Activate event game objects
                if (eventObjectsActivate && eventObjects.Length > 0)
                {
                    foreach (GameObject g in eventObjects)
                    {
                        // flips active state
                        if(g.gameObject.activeSelf)
                            g.gameObject.SetActive(false);
                        else
                            g.gameObject.SetActive(true);
                    }
                }

				// Power these
				lights.SetActive(true);
				elevator.GetComponent<elevator>().PowerSupplied();

				// Button is now pushed
				buttonPushed = true;

			}

		}

		else
		{
			uiText.enabled = false;
		}

		if (debug)
			Debug.Log("Looking At Generator " + gameObject);
	}
}
