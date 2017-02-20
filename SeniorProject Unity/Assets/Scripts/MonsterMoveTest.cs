using UnityEngine;
using System.Collections;
using System;

public class MonsterMoveTest : MonoBehaviour {

    public GameObject head;
    public GameObject[] pos;
    int i = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        if (i >= pos.Length)
        {
            i = 0;
            gameObject.SetActive(false);
        }
        

        //if (transform.position != pos[i].transform.position && i < 3)
        //if ((Math.Abs(transform.position.x - pos[i].transform.position.x) <= 0.01f) && (Math.Abs(transform.position.y - pos[i].transform.position.y) <= 0.01f) && (Math.Abs(transform.position.z - pos[i].transform.position.z) <= 0.01f) && i < 3)
        if(!pos[i].GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
        {
            transform.position = Vector3.Lerp(transform.position, pos[i].transform.position, 0.3f * Time.deltaTime);
        }
        else
        {
            i++;
        }

        //transform.position = Vector3.Slerp(transform.position, pos1.transform.position, 0.5f * Time.deltaTime);
    }
}
