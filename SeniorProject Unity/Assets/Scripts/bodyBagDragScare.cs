using UnityEngine;
using System.Collections;

public class bodyBagDragScare : MonoBehaviour {

	//public Rigidbody rb;
    public GameObject[] pos;
	GameObject[] unaturalLights;
	float[] unaturalLightIntensity;
	int i = 0;
	int x = 0;

	public bool scareDone = false;

	private Vector3 lastPosition;
	private Vector3 velocity;

	// Use this for initialization
	void Start()
	{
		lastPosition = new Vector3(0, 0, 0);
		unaturalLights = GameObject.FindGameObjectsWithTag("unaturalLight");

		unaturalLightIntensity = new float[unaturalLights.Length];

		i = 0;

		for (int x = 0; x < unaturalLights.Length; x++)
		{
			unaturalLightIntensity[x] = unaturalLights[x].GetComponent<Light>().intensity;
		}

		scareDone = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (i >= pos.Length)
		{
			gameObject.SetActive(false);
		}
		else
		{
			// Hecka Good movement code
			if (!pos[i].GetComponent<SphereCollider>().bounds.Contains(gameObject.transform.position))
			{
				transform.position = Vector3.Slerp(transform.position, pos[i].transform.position, 0.3f * Time.deltaTime);
			}
			else
			{
				i++;
			}
			//
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
