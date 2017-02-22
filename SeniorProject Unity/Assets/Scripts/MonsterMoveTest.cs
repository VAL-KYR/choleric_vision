using UnityEngine;
using System.Collections;
using System;

public class MonsterMoveTest : MonoBehaviour {

    public GameObject head;
    public GameObject[] pos;
    public GameObject[] unaturalLights;
    public float[] unaturalLightIntensity;
    int i = 0;
    int x = 0;

    private Vector3 lastPosition;
    private Vector3 velocity;

    // Use this for initialization
    void Start () {
        lastPosition = new Vector3(0, 0, 0);
        unaturalLights = GameObject.FindGameObjectsWithTag("unaturalLight");

        unaturalLightIntensity = new float[unaturalLights.Length];

        i = 0;

        for (int x = 0; x < unaturalLights.Length; x++)
        {
            unaturalLightIntensity[x] = unaturalLights[x].GetComponent<Light>().intensity;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
        if (i >= pos.Length)
        {

            Debug.Log("movement end");

            for (int y = 0; y < unaturalLights.Length; y++)
            {
                unaturalLights[y].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[y].GetComponent<Light>().intensity, unaturalLightIntensity[y], (20.0f) * Time.deltaTime);
            }


            int lightsNormal = 0;

            for (int y = 0; y < unaturalLights.Length; y++)
            {
                if(unaturalLights[y].GetComponent<Light>().intensity == unaturalLightIntensity[y]){
                    lightsNormal++;
                }
            }

            Debug.Log("lightsnorml " + lightsNormal);

            if (lightsNormal >= unaturalLights.Length)
            {
                gameObject.SetActive(false);
                Debug.Log("monster poof");
            }

        }
        else
        {

            if (!pos[i].GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
            {
                transform.position = Vector3.Lerp(transform.position, pos[i].transform.position, 0.3f * Time.deltaTime);
            }
            else
            {
                i++;
            }


            for (int j = 0; j < unaturalLights.Length; j++)
            {
                float distanceToLight = (unaturalLights[j].transform.position - transform.position).magnitude;

                if (distanceToLight <= 10.0f)
                {
                    unaturalLights[j].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[j].GetComponent<Light>().intensity, 0.0f, (distanceToLight / 10.0f) * Time.deltaTime);
                }

                else
                {
                    unaturalLights[j].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[j].GetComponent<Light>().intensity, unaturalLightIntensity[j], (distanceToLight / 60.0f) * Time.deltaTime);
                }
            }
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


        

        
    }

}
