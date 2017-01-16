using UnityEngine;
using System.Collections;

public class lookAt : MonoBehaviour {

	public bool debug;
	public string lookAtTag;
	public GameObject playerLookAt;
	public GameObject lastLookedAt;
	bool look = false;


	private Vector3 observer;
	private Vector3 observed;
	public float lookAtDist;



	// Use this for initialization
	void Start () {
		lastLookedAt = playerLookAt;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;

		var cameraCenter = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, GetComponent<Camera>().nearClipPlane));


		if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 1000))
		{
			playerLookAt = hit.transform.gameObject;

			observer = gameObject.transform.position;
			observed = playerLookAt.transform.position;
			lookAtDist = Vector3.Distance(observed, observer);

			// For triggerLookAt elements that need to pass a lookAt command
			if (playerLookAt.CompareTag(lookAtTag))
			{
				playerLookAt.GetComponent<triggerLookAt>().lookTrigger();
				lastLookedAt = playerLookAt;
			}

			// For that corpse in the sink BY SEARCHING FOR TAG
			else if (playerLookAt.CompareTag("sinkCorpse"))
			{
				playerLookAt.GetComponent<soundTrigger>().lookTrigger();
				lastLookedAt = playerLookAt;
			}

			else if (playerLookAt.CompareTag("cielingCollapse"))
			{
				playerLookAt.GetComponent<eventOfficeCollapse>().lookingAtMe(lookAtDist);
				lastLookedAt = playerLookAt;
			}

			else if (playerLookAt.CompareTag("key"))
			{
				if (debug)
					Debug.Log("Key Dist: " + lookAtDist);

				//no idea why this works
				playerLookAt.GetComponent<keyUI>().lookingAtMe(true);

				// debug look to test
				look = playerLookAt.GetComponent<UIqueue>().lookingAtMe(lookAtDist);

				lastLookedAt = playerLookAt;
			}

			else if (playerLookAt.CompareTag("documentCabinet"))
			{
				if (debug)
					Debug.Log("Document Cabinet Dist: " + lookAtDist);

				//no idea why this works
				playerLookAt.GetComponent<documentCabinet>().lookingAtMe(lookAtDist);

				lastLookedAt = playerLookAt;
			}

			else if (playerLookAt.CompareTag("tapeRecorder"))
			{
				if (debug)
					Debug.Log("Recorder Dist: " + lookAtDist);

				playerLookAt.GetComponent<tapeMaster>().lookingAtMe(lookAtDist);

				// Pass Object for UIqueue maybe?
				//look = playerLookAt.GetComponent<UIqueue>().lookingAtMe(lookAtDist);

				lastLookedAt = playerLookAt;
			}

			else if (playerLookAt.CompareTag("generatorButton"))
			{
				if (debug)
					Debug.Log("GeneratorButton Dist: " + lookAtDist);

				playerLookAt.GetComponent<generatorButton>().lookingAtMe(lookAtDist);
				lastLookedAt = playerLookAt;
			}

            else if (playerLookAt.CompareTag("generatorLever"))
            {
                if (debug)
                    Debug.Log("GeneratorLever Dist: " + lookAtDist);

                playerLookAt.GetComponent<generatorLever>().lookingAtMe(lookAtDist);
                lastLookedAt = playerLookAt;
            }

            // Deactivate LookedAt Status
            else 
			{
				if (lastLookedAt.CompareTag("key"))
				{
					lastLookedAt.GetComponent<UIqueue>().lookingAtMe(lookAtDist);
					lastLookedAt.GetComponent<keyUI>().lookingAtMe(false);
				}

				else if (lastLookedAt.CompareTag("tapeRecorder"))
				{
					//lastLookedAt.GetComponent<UIqueue>().lookingAtMe(lookAtDist);
					lastLookedAt.GetComponent<tapeMaster>().lookingAtMe(lookAtDist);
				}
			}

			if (debug)
				Debug.Log("Player looking at " + playerLookAt);

		}
	}
}
