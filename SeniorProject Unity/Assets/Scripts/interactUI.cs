using UnityEngine;
using System.Collections;

public class interactUI : MonoBehaviour {

    public bool debug = false;

    public GameObject lookAt;
    public GameObject vrLookAt;
    public GameObject currLookAt;

    public Sprite hand;
    public Sprite doorOpen;
    public Sprite doorClose;
    public Sprite doorLocked;
    public Sprite doorBroken;

    SpriteRenderer ui;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (lookAt.activeSelf)
        {
            currLookAt = lookAt;
        }
        else
        {
            currLookAt = vrLookAt;
        }

        ui = gameObject.GetComponent<SpriteRenderer>();

        if (debug)
            Debug.Log("UI for " + currLookAt.GetComponent<lookAt>().playerLookAt + " with sprite " + ui.sprite + " because tag " + currLookAt.GetComponent<lookAt>().lookAtTag);

        // Interact objects
        if (currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("key") || currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("documentCabinet") || currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("generatorLever"))
        {
            ui.sprite = hand;
            ui.color = new Color(1, 1, 1, 1);
        }

        else if(currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("tapeRecorder") && currLookAt.GetComponent<lookAt>().lookAtDist <= 2.0f)
        {
            ui.sprite = hand;
            ui.color = new Color(1, 1, 1, 1);
        }


        // Doors & States
        else if (currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag(currLookAt.GetComponent<lookAt>().lookAtTag))
        {

            // Locked door
            if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorLocked)
            {
                if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorHasKey)
                {
                    ui.sprite = doorLocked;
                    ui.color = new Color(1, 1, 1, 1);
                }
                else if(!currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorHasKey)
                {
                    ui.sprite = doorBroken;
                    ui.color = new Color(1, 1, 1, 1);
                }
            }

            // Not locked door
            else
            {
                if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorOpen)
                {
                    ui.sprite = doorClose;
                    ui.color = new Color(1, 1, 1, 1);
                }
                else if (!currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorOpen)
                {
                    ui.sprite = doorOpen;
                    ui.color = new Color(1, 1, 1, 1);
                }
            }
        }

        // Not looking at interactUI objects
        else
        {
            ui.color = new Color(0, 0, 0, 0);
        }
	
	}
}
