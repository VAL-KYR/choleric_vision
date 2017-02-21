using UnityEngine;
using System.Collections;
using System;

public class MonsterMoveTest : MonoBehaviour {

    public GameObject head;
    public GameObject[] pos;
    int i = 0;

    public Vector3 lastPosition;
    public Vector3 velocity;

    // Use this for initialization
    void Start () {
        lastPosition = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (i >= pos.Length)
        {
            i = 0;
            gameObject.SetActive(false);
        }

        // Hecka good turning code
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(velocity),
            Time.deltaTime * 1.0f
        );
        //


        if (!pos[i].GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
        {    
            transform.position = Vector3.Lerp(transform.position, pos[i].transform.position, 0.3f * Time.deltaTime);
        }
        else
        {
            i++;
        }
    }

}
