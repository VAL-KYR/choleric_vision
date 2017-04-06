using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class interactUI : MonoBehaviour
{

    public bool debug = false;
    public bool cursorOn = true;

    public GameObject lookAt;
    public GameObject vrLookAt;
    public GameObject currLookAt;
    public GameObject seeingObject;
    public GameObject lastSeenObject;

    public Shader unlitShader;
    public Shader litShader;

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
    void Start()
    {
        if (lookAt.activeSelf)
            lookAtDist = lookAt.GetComponent<lookAt>().lookAtDist;
        else
            lookAtDist = vrLookAt.GetComponent<lookAt>().lookAtDist;

        uiReset = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

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
            Debug.Log("UI for " + seeingObject + " with sprite " + ui.sprite + " because tag " + seeingObject.tag);

        // Interact objects
        if ((seeingObject.CompareTag("key")
            || seeingObject.CompareTag("generatorLever")
            || seeingObject.CompareTag("lookAtObject")
            || seeingObject.CompareTag("tapeRecorder"))
            && (lookAtDist < 2.0f))
        {
            ui.sprite = doorOpen;
        }

        // Doors & States
        else if (seeingObject.CompareTag(currLookAt.GetComponent<lookAt>().lookAtTag) && (lookAtDist < 2.0f))
        {
            if (seeingObject.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>())
            {
                // Locked door
                if (seeingObject.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorLocked)
                {
                    if (seeingObject.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorHasKey)
                    {
                        ui.sprite = doorLocked;
                    }
                }

                // Not locked door
                else
                {
                    if (seeingObject.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorOpen)
                    {
                        ui.sprite = doorClose;
                    }
                    else if (!seeingObject.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorOpen)
                    {
                        ui.sprite = doorOpen;
                    }
                }
            }



            // document cabinet
            else if (seeingObject.GetComponent<triggerLookAt>().rootObject.GetComponent<documentCabinet>() || seeingObject.GetComponent<triggerLookAt>().rootObject.GetComponent<lookAtObject>())
            {
                ui.sprite = doorOpen;
            }

            // Look At Objects
            if (seeingObject.GetComponent<triggerLookAt>().rootObject.GetComponent<lookAtObject>())
            {
                ui.sprite = doorOpen;
            }
        }

        // Not looking at interactUI objects
        // Ignore the player
        else
        {
            if (!seeingObject.CompareTag("GameController"))
            {
                ui.sprite = hand;
            }
        }

        if (!seeingObject.CompareTag("GameController"))
        {
            uiQueue();
        }
        else
        {
            if (debug)
                Debug.Log("THERE SHOULD BE NO FUCKING SPRITE HERE");
        }


    }

    public void uiQueue()
    {
        if (lookAtDist < 2.0f || lookAtLastDist < 2.0f)
        {
            ui.color = Color.Lerp(ui.color, new Color(0.7f, 0.7f, 0.7f, 1), ((lookAtDist - 2.0f) * -4) * 1.5f * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.0f, 0.0f, uiReset.z + (lookAtDist - 0.55f)), ((lookAtDist - 2.0f) * -12) * 1.5f * Time.deltaTime);

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
