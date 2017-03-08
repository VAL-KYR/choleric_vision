using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class interactUI : MonoBehaviour {

    public bool debug = false;

    public bool uiGlow = false;

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
        if (currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("key") || currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("generatorLever"))
        {
            ui.sprite = hand;
            uiQueue();
        }

        else if(currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag("tapeRecorder") && currLookAt.GetComponent<lookAt>().lookAtDist <= 2.0f)
        {
            ui.sprite = hand;
            uiQueue();
        }


        // Doors & States
        else if (currLookAt.GetComponent<lookAt>().playerLookAt.CompareTag(currLookAt.GetComponent<lookAt>().lookAtTag))
        {
            if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>())
            {
                // Locked door
                if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorLocked)
                {
                    if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorHasKey)
                    {
                        ui.sprite = doorLocked;
                        uiQueue();
                    }
                    /*
                    else if(!currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorHasKey)
                    {
                        ui.sprite = doorBroken;
                        uiQueue();
                    }
                    */
                }

                // Not locked door
                else
                {
                    if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorOpen)
                    {
                        ui.sprite = doorClose;
                        uiQueue();
                    }
                    else if (!currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<doorMaster>().doorOpen)
                    {
                        ui.sprite = doorOpen;
                        uiQueue();
                    }
                }
            }
            else if (currLookAt.GetComponent<lookAt>().playerLookAt.GetComponent<triggerLookAt>().rootObject.GetComponent<documentCabinet>())
            {
                ui.sprite = hand;
                uiQueue();
            }
            
        }

        // Not looking at interactUI objects
        else
        {
            if (lookAtDist > 2.0f)
            {
                ui.color = new Color(0, 0, 0, 0);
                transform.localPosition = uiReset;

                if (uiGlow)
                {
                    if (seeingObject.GetComponent<Renderer>() != null)
                    {
                        unlitShader = seeingObject.GetComponent<Renderer>().material.shader;
                        //unlitMat = seeingObject.GetComponent<Renderer>().materials[seeingObject.GetComponent<Renderer>().materials.Length - 1];
                        //unlitMat = seeingObject.GetComponent<Renderer>().material;

                        /*
                        if (seeingObject != lastSeenObject)
                        {
                            getMats(seeingObject);
                        }
                        */

                        lastSeenObject.GetComponent<Renderer>().material.shader = unlitShader;
                        //lastSeenObject.GetComponent<Renderer>().materials[lastSeenObject.GetComponent<Renderer>().materials.Length - 1] = unlitMat;
                        //lastSeenObject.GetComponent<Renderer>().material = unlitMat;
                    }
                }
                
            }
        }

        if (lookAtLastDist >= 2.0f)
        {
            ui.color = new Color(0, 0, 0, 0);
            transform.localPosition = uiReset;

            if (uiGlow)
            {
                if (seeingObject.GetComponent<Renderer>() != null)
                {
                    unlitShader = seeingObject.GetComponent<Renderer>().material.shader;
                    //unlitMat = seeingObject.GetComponent<Renderer>().materials[seeingObject.GetComponent<Renderer>().materials.Length - 1];
                    //unlitMat = seeingObject.GetComponent<Renderer>().material;

                    /*
                    if (seeingObject != lastSeenObject)
                    {
                        getMats(seeingObject);
                    }
                    */

                    lastSeenObject.GetComponent<Renderer>().material.shader = unlitShader;
                    //lastSeenObject.GetComponent<Renderer>().materials[lastSeenObject.GetComponent<Renderer>().materials.Length - 1] = unlitMat;
                    //lastSeenObject.GetComponent<Renderer>().material = unlitMat;
                }
            }
            
        }
	
	}

    //seeingObject.GetComponent<Renderer>().materials[seeingObject.GetComponent<Renderer>().materials.Length-1].shader = litShader;

    public void uiQueue()
    {
        if(lookAtDist <= 2.0f)
        {
            ui.color = new Color(1, 1, 1, 1);
            transform.localPosition = new Vector3(0.0f, 0.0f, uiReset.z + (lookAtDist - 0.5f));

            if (uiGlow)
            {
                if (seeingObject.GetComponent<Renderer>() != null)
                {
                    seeingObject.GetComponent<Renderer>().material.shader = litShader;
                    //seeingObject.GetComponent<Renderer>().materials[seeingObject.GetComponent<Renderer>().materials.Length - 1] = litMat;
                    //seeingObject.GetComponent<Renderer>().material = litMat;

                }
            }
                      
            
        }
        

        //changeMats(seeingObject, shaders);
    }

    /*
    public void getMats(GameObject g)
    {
        shaders = new List<Shader>();

        foreach (Material m in g.GetComponent<Renderer>().materials)
        {
            shaders.Add(m.shader);
        }
    }

    public void changeMats(GameObject g, List<Shader> s)
    {
        //int i = s.Count;
        int i = 0;

        foreach (Material m in g.GetComponent<Renderer>().materials)
        {
            i++;
            m.shader = s[i];

        }
    }
    */
}
