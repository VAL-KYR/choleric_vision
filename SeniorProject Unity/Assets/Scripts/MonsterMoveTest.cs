using UnityEngine;
using System.Collections;
using System;

public class MonsterMoveTest : MonoBehaviour {

    public bool debug = false;
    //public Rigidbody rb;
    public GameObject[] pos;
    GameObject[] unaturalLights;
    float[] unaturalLightIntensity;
    int i = 0;
    int x = 0;

    public bool affectLights = true;
    public bool monsterGone = false;

    private Vector3 lastPosition;
    private Vector3 velocity;

    // Use this for initialization
    void Start () {
        //rb = GetComponent<Rigidbody>();
        lastPosition = new Vector3(0, 0, 0);
        unaturalLights = GameObject.FindGameObjectsWithTag("unaturalLight");

        unaturalLightIntensity = new float[unaturalLights.Length];

        i = 0;

        for (int x = 0; x < unaturalLights.Length; x++)
        {
            unaturalLightIntensity[x] = unaturalLights[x].GetComponent<Light>().intensity;
        }

        monsterGone = false;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (i >= pos.Length)
        {
            if(debug)
                Debug.Log("movement end");

            if (affectLights)
            {
                for (int y = 0; y < unaturalLights.Length; y++)
                {
                    unaturalLights[y].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[y].GetComponent<Light>().intensity, unaturalLightIntensity[y], (20.0f) * Time.deltaTime);
                }


                int lightsNormal = 0;

                for (int y = 0; y < unaturalLights.Length; y++)
                {
                    if (unaturalLights[y].GetComponent<Light>().intensity == unaturalLightIntensity[y])
                    {
                        lightsNormal++;
                    }
                }
                if (debug)
                    Debug.Log("lightsnorml " + lightsNormal);

                if (lightsNormal >= unaturalLights.Length)
                {
                    gameObject.SetActive(false);

                    if (debug)
                        Debug.Log("monster poof");
                }
            }

            
            gameObject.SetActive(false);

            if(debug)
                Debug.Log("monster poof");


            monsterGone = true;

        }
        else
        {

            // Hecka Bad movement code
            //if (!pos[i].GetComponent<SphereCollider>().bounds.Contains(rb.position)) 
            
            // Hecka Good movement code
            if (!pos[i].GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
            {
                transform.position = Vector3.Lerp(transform.position, pos[i].transform.position, 0.3f * Time.deltaTime);
                //rb.MovePosition(Vector3.Lerp(rb.position, pos[i].transform.position, 0.3f * Time.deltaTime));
            }
            else
            {
                i++;
            }
            //

            if (affectLights)
            {
                for (int j = 0; j < unaturalLights.Length; j++)
                {
                    float distanceToLight = (unaturalLights[j].transform.position - transform.position).magnitude;

                    if (distanceToLight <= 10.0f)
                    {
                        if (unaturalLights[j].GetComponent<flashLightOnOff>() != null)
                        {
                            unaturalLights[j].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[j].GetComponent<Light>().intensity, 0.8f, (distanceToLight / 10.0f) * Time.deltaTime);
                            unaturalLights[j].GetComponent<flashLightOnOff>().flashPeriod = 2.0f;
                            unaturalLights[j].GetComponent<flashLightOnOff>().flashing = true;
                        }
                        else
                        {
                            unaturalLights[j].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[j].GetComponent<Light>().intensity, 0.0f, (distanceToLight / 10.0f) * Time.deltaTime);
                        }
                        
                    }

                    else
                    {
                        unaturalLights[j].GetComponent<Light>().intensity = Mathf.Lerp(unaturalLights[j].GetComponent<Light>().intensity, unaturalLightIntensity[j], (distanceToLight / 60.0f) * Time.deltaTime);
                    }
                }
            }
            
        }

        // Hecka bad turning code
        //rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(rb.velocity), 1.0f * Time.deltaTime));

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
