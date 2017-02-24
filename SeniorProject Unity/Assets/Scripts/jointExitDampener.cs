using UnityEngine;
using System.Collections;

public class jointExitDampener : MonoBehaviour {
    public string rigidBodyTag;
    public string maxDepenetrationVelocity;
    public GameObject[] allRigidBodies;

    // Use this for initialization
    void Start () {
        allRigidBodies = GameObject.FindGameObjectsWithTag(rigidBodyTag);
	}
	
	// Update is called once per frame
	void Update () {

        allRigidBodies = GameObject.FindGameObjectsWithTag(rigidBodyTag);

        foreach(GameObject g in allRigidBodies)
        {
            if (g.GetComponent<Rigidbody>())
            {
                //Debug.Log("DePen Velocity: " + g.GetComponent<Rigidbody>().maxDepenetrationVelocity);
                g.GetComponent<Rigidbody>().maxDepenetrationVelocity = 0.5f;
            }
        }
    }
}
