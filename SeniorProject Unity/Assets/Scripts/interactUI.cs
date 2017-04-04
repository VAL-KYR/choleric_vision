using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class interactUI : MonoBehaviour {

    public bool debug = false;
    public bool cursorOn = true;

    public GameObject lookAt;
    public GameObject vrLookAt;
    public GameObject currLookAt;
    public GameObject seeingObject;
    public GameObject lastSeenObject;

    public Shader unlitShader;
    public Shader litShader;
    //public Material unlitMat;
    //public Material litMat;
    //public List<Shader> shaders;
    //public List<Shader> unlitShaders;
    //public List<Shader> litShaders;

    //public Shader[] unlitShaders;
    //public Shader[] litShaders;

    public Sprite hand;
    public Sprite doorOpen;
    public Sprite doorClose;
    public Sprite doorLocked;
    public Sprite doorBroken;

    public float lookAtDist;
    public float lookAtLastDist;
    public Vector3 uiReset;

    public SpriteRenderer ui;

	// Use this for initialization
	void Start () {
        if(lookAt.activeSelf)
            lookAtDist = lookAt.GetComponent<lookAt>().lookAtDist;
        else
            lookAtDist = vrLookAt.GetComponent<lookAt>().lookAtDist;

        uiReset = transform.localPosition;
        //unlitShader = Shader.Find("Diffuse");
        //litShader = Shader.Find("Outlined/Silhouette Only");
    }
	
	// Update is called once per frame
	void Update () {

        if (lookAt.activeSelf)
        {
            currLookAt = lookAt;
            lookAtDist = lookAt.GetComponent<lookAt>().lookAtDist;
            seeingObject = lookAt.GetComponent<lookAt>().playerLookAt;
            lastSeenObject = lookAt.GetComponent<lookAt>().lastLookedAt;
        }
        else
        {
            currLookAt = vrLookAt;
            lookAtDist = vrLookAt.GetComponent<lookAt>().lookAtDist;
            seeingObject = vrLookAt.GetComponent<lookAt>().playerLookAt;
            lastSeenObject = vrLookAt.GetComponent<lookAt>().lastLookedAt;
        }

        lookAtLastDist = Vector3.Distance(seeingObject.transform.position, gameObject.transform.position);

        ui = gameObject.GetComponent<SpriteRenderer>();

        if (debug)
            Debug.Log("UI for " + currLookAt.GetComponent<lookAt>().playerLookAt + " with sprite " + ui.sprite + " because tag " + currLookAt.GetComponent<lookAt>().lookAtTag);

        // Interact objects
        if ((currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("key") || currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("generatorLever")) && (lookAtDist < 2.0f))
        {
            ui.sprite = doorOpen;
        }

        else if (currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("tapeRecorder") && (lookAtDist < 2.0f))
        {
            ui.sprite = doorOpen;
        }

        else if (currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("lookAtObject") && (lookAtDist < 2.0f))
        {
            ui.sprite = doorOpen;
        }

        // Doors & States
        else if (currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag(currLookAt.GetComponent<lookAt>().lookAtTag) && (lookAtDist < 2.0f))
        {
            if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>())
            {
                // Locked door
                if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorLocked)
                {
                    if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorHasKey)
                    {
                        ui.sprite = doorLocked;
                    }
                }

                // Not locked door
                else
                {
                    if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorOpen)
                    {
                        ui.sprite = doorClose;
                    }
                    else if (!currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorOpen)
                    {
                        ui.sprite = doorOpen;
                    }
                }
            }



            // document cabinet
            else if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<documentCabinet>())
            {
                ui.sprite = doorOpen;
            }
            
            
        }

        // Not looking at interactUI objects
        // Ignore the player
        else
        {
            if (!currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("GameController"))
            {
                ui.sprite = hand;
            }
            
        }
        if (!currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("GameController"))
        {
            uiQueue();
        }

    }
    
    public void uiQueue()
    {
        if (lookAtDist < 2.0f || lookAtLastDist < 2.0f)
        {
            ui.color = Color.Lerp(ui.color, new Color(0.7f, 0.7f, 0.7f, 1), ((lookAtDist - 2.0f) * -3) * 1.5f * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.0f, 0.0f, uiReset.z + (lookAtDist - 0.45f)), ((lookAtDist - 2.0f) * -3) * 1.5f * Time.deltaTime);

        }
        else
        {
            /// Cursor
            if (!cursorOn)
            {
                ui.color = Color.Lerp(ui.color, new Color(0, 0, 0, 0), Time.deltaTime);
            }
            else
            {
                ui.color = Color.Lerp(ui.color, new Color(1, 1, 1, 0.7f), Time.deltaTime);
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, uiReset, 1.7f * Time.deltaTime);
        }

        if (lookAtLastDist >= 2.0f || lookAtDist >= 2.0f)
        {
            /// Cursor
            if (!cursorOn)
            {
                ui.color = Color.Lerp(ui.color, new Color(0, 0, 0, 0), Time.deltaTime);
            }
            else
            {
                ui.color = Color.Lerp(ui.color, new Color(1, 1, 1, 0.7f), Time.deltaTime);
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, uiReset, 1.7f * Time.deltaTime);
        }

    }

   
}
