using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class lookAtObject : MonoBehaviour {
    private GameObject[] gui;
    private GameObject currGui;

    private Vector3 oriLoc;
    private Vector3 oriSca;
    private Vector3 oriRot;

    public float scaleFactor;

    private bool itemHold;

    private GameObject ObjLookGO;

    public GameObject oriParent;

    private GameObject player;
    private GameObject curCam;

    private GameObject objLookingAt;

    private float distance;

    private float mYaw = 0.0f;
    private float mPitch = 0.0f;

    private float zoomSpeed;
    private float curZoom;

    public float scaleMin;
    public float scaleMax;

    /// For Triggering Flashbacks or Voice Lines
    // Voiceline or flashback
    [System.Serializable]
    public class Narrative
    {
        // VoiceLine code
        public GameObject voiceBox;

        public bool flashbackTrigger = false;
        public bool voiceLineTrigger = true;

        public bool triggerVoiceOnce = true;

        // VoiceLine code

        // Flashback code
        public AudioClip flashback;
        public AudioClip voiceLine;

        public bool flashbackTriggered = false;
        public bool voiceTriggered;
    }
    public Narrative narrative = new Narrative();
    /// For Triggering Flashbacks or Voice Lines

    // Use this for initialization
    void Start () {

        gui = GameObject.FindGameObjectsWithTag("PlayerUI");
        foreach (GameObject u in gui)
        {
            if (u.activeSelf)
            {
                currGui = u;
            }
        }

        oriLoc = transform.position;
        oriSca = transform.localScale;
        oriRot = transform.eulerAngles;

        ObjLookGO = GameObject.FindGameObjectWithTag("ObjLook");
        player = GameObject.FindGameObjectWithTag("GameController");

        itemHold = false;

        if (VRSettings.enabled)
            curCam = GameObject.FindGameObjectWithTag("VRCam");
        else
            curCam = GameObject.FindGameObjectWithTag("NonVRCam");


        zoomSpeed = 1.0f;
        curZoom = scaleFactor;

        /// For Triggering Flashbacks or Voice Lines
        narrative.voiceBox = GameObject.FindGameObjectWithTag("voice");

        narrative.voiceTriggered = false;
        narrative.flashbackTriggered = false;
        /// For Triggering Flashbacks or Voice Lines
    }

    // Update is called once per frame
    void Update()
    {
        if (VRSettings.enabled)
            curCam = GameObject.FindGameObjectWithTag("VRCam");
        else
            curCam = GameObject.FindGameObjectWithTag("NonVRCam");

        // Updating which GUI to use
        foreach (GameObject u in gui)
        {
            if (u.activeSelf)
            {
                currGui = u;
            }
        }

        if (!itemHold)
        {
            // show UI queue
            if (objLookingAt == this.gameObject && distance < 2f)
            {
                // UI queue stuff
                currGui.GetComponent<interactUI>().ui.sprite = currGui.GetComponent<interactUI>().hand;
                currGui.GetComponent<interactUI>().uiQueue();
            }

            objLookingAt = curCam.GetComponent<lookAt>().playerLookAt;

            distance = Vector3.Distance(player.transform.position, transform.position);

            if (Input.GetButtonDown("Action") && objLookingAt == this.gameObject && distance < 2f)
            {
                // fix camera in front of player if not vr camera
                if (!VRSettings.enabled)
                {
                    curCam.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                }

                // disable the collider
                if (gameObject.GetComponent<Collider>() != null)
                {
                    gameObject.GetComponent<Collider>().enabled = false;
                }


                transform.parent = ObjLookGO.transform;
                transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                transform.localScale = (oriSca * scaleFactor);
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

                // disable movement
                itemHold = true;
                player.GetComponent<controller>().holdObj = true;
            }
        }
        else if(itemHold)
        {
            /// TRIGGER NARRATIVE OR FLASHBACK
            if (narrative.flashbackTrigger)
            {
                if (!narrative.flashbackTriggered)
                {
                    // call function in voices.cs
                    narrative.voiceBox.GetComponent<voices>().startFlashback(narrative.flashback);
                    narrative.flashbackTriggered = true;
                }
            }


            if (narrative.voiceLineTrigger)
            {
                if (narrative.triggerVoiceOnce)
                {
                    if (!narrative.voiceTriggered)
                    {
                        narrative.voiceBox.GetComponent<voices>().voiceLinePlay(narrative.voiceLine);
                        narrative.voiceTriggered = true;
                    }
                }
                else
                {
                    narrative.voiceBox.GetComponent<voices>().voiceLinePlay(narrative.voiceLine);
                }
            }
            /// TRIGGER NARRATIVE OR FLASHBACK

            mYaw += Input.GetAxis("Mouse X");
            mPitch += Input.GetAxis("Mouse Y");

            if(Input.GetAxis("Vertical") != 0.0f)
                curZoom += Input.GetAxis("Vertical") * zoomSpeed;


            if (curZoom < scaleMin)
                curZoom = scaleMin;
            if (curZoom > scaleMax)
                curZoom = scaleMax;

            transform.localScale = new Vector3(curZoom , curZoom, curZoom);

            transform.eulerAngles = new Vector3(mPitch, mYaw, 0.0f);
            

            if (Input.GetButtonDown("Action"))
            {
                if(oriParent == null)
                    transform.parent = null;
                else
                    transform.parent = oriParent.transform;

                transform.position = oriLoc;
                transform.localScale = oriSca;
                transform.eulerAngles = oriRot;

                /// TRIGGER NARRATIVE OR FLASHBACK
                // reset voice line
                narrative.voiceTriggered = false;
                /// TRIGGER NARRATIVE OR FLASHBACK

                // re-enable movement
                itemHold = false;
                player.GetComponent<controller>().holdObj = false;

                // disable the collider
                if (gameObject.GetComponent<Collider>() != null)
                {
                    gameObject.GetComponent<Collider>().enabled = true;
                }
            }
        }
        
    }
}
