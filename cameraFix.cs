using UnityEngine;
using System.Collections;

public class cameraFix : MonoBehaviour {

    public GameObject playerVR;
    public GameObject playerController;
    
    private Vector3 headJointPosition;
    private Vector3 camPosition;

    private bool moving;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        headJointPosition = transform.position;

        camPosition = headJointPosition;

        playerVR.transform.position = camPosition;

        moving = playerController.GetComponent<controller>().playerMoving;

        

        if (moving)
        {
            //playerController.transform.Rotate(Vector3.up, Time.deltaTime * 300, Space.World);

            /*if (playerVR.transform.eulerAngles.y < playerController.transform.eulerAngles.y)
            {

                print("O1");
                /*playerVR.transform.eulerAngles += new Vector3 (0.0f, 5.0f, 0.0f);
                
            }
            else if (playerVR.transform.eulerAngles.y > playerController.transform.eulerAngles.y)
            {
                print("O1");
                /*playerVR.transform.eulerAngles -= new Vector3(0.0f, 5.0f, 0.0f);*
                playerController.transform.eulerAngles += new Vector3(0.0f, 5.0f, 0.0f);
            }*/
        }
    }
}
