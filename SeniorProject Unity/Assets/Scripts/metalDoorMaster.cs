using UnityEngine;
using System.Collections;

public class metalDoorMaster : MonoBehaviour
{

    public bool debug;
    public bool doorLocked;
    public bool doorOpen = false;
    public bool doorOpenCommand = false;
    public bool interactSpace;
    public bool doorHasKey = false;

    float time = 0.0f;


    public GameObject doorRight;
    public GameObject doorLeft;


    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        /*
        if (doorOpen)
        {
            OpenDoors();
        }
        else if (!doorOpen)
        {
            CloseDoors();
        }
        */

        //doorLeft.transform.localRotation.x > 86.0f

        if (!Mathf.Approximately(doorLeft.transform.rotation.z, -90.0f))
        {
            doorOpen = true;
            OpenDoors();
        }

        else
        {
            doorOpen = false;
            CloseDoors();
        }
        
    }

    public void OpenDoors()
    {
        doorRight.transform.rotation = Quaternion.Slerp(doorRight.transform.rotation, new Quaternion(0.0f, 0.0f, 0.5f, 0.0f), 1.0f * Time.deltaTime);
        doorLeft.transform.rotation = Quaternion.Slerp(doorLeft.transform.rotation, new Quaternion(0.0f, 0.0f, -0.5f, 0.0f), 1.0f * Time.deltaTime);

        Debug.Log("opening");
    }

    public void CloseDoors()
    {
        doorRight.transform.rotation = Quaternion.Slerp(doorRight.transform.rotation, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), 1.0f * Time.deltaTime);
        doorLeft.transform.rotation = Quaternion.Slerp(doorLeft.transform.rotation, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), 1.0f * Time.deltaTime);

        Debug.Log("closing");
    }

    public void lookingAtMe()
    {
        if (Input.GetButtonDown("Action"))
        {
            if (doorOpen)
            {
                doorOpen = false;
                Debug.Log("close");
            }
            else
            {
                doorOpen = true;
                Debug.Log("open");
            }
            
        }
    }

}