using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class flashLightOnOff : MonoBehaviour
{

    public Light flashlight;

	public bool debug = false;

    public float lightIntensity;

    public float flashTime = 0.0f;
    public float flashPeriod = 2.0f;
    public bool flashing = false;
    public bool flashStartSet = false;
    public bool fixFlashLight = false;

    private GameObject heart;

    public Color blackCol = Color.black;
    public Color startCol;

    float time = 0.0f;



    // Use this for initialization
    void Start()
    {
        heart = GameObject.FindGameObjectWithTag("heart");
        flashlight = gameObject.GetComponent<Light>();
        startCol = flashlight.color;

        flashlight.intensity = lightIntensity;

        flashlight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
            Debug.Log(lightIntensity);

        if (Input.GetButtonDown("FlashLight"))
        {
            if (flashlight.isActiveAndEnabled)
                flashlight.enabled = false;
            else
                flashlight.enabled = true;
        }
        if (debug)
        {
            if (Input.GetButtonDown("DevVoice1"))
            {
                flashing = true;
            }
        }
       

        if (heart.GetComponent<heartBeatThump>().restDifference > 1.5)
        {
            flashing = true;
        }

        if (fixFlashLight)
        {
            BackToNormal();
        }

        if (flashing)
        {
            FlashLightFlicker();
        }

        time += (Time.deltaTime);

    }

    void FixedUpdate()
    {
        time += (Time.deltaTime);
    }

    void LastUpdate()
    {
        time += (Time.deltaTime);
    }

    public void FlashLightFlicker()
    {
        flashTime += Time.deltaTime;

        if (flashTime < flashPeriod && flashTime > 0.0f)
        {
            flashlight.color = Color.Lerp(startCol, blackCol, (Mathf.PerlinNoise(time, time)) * 2.0f);
            flashing = true;
        }
        else
        {
            fixFlashLight = true;
            flashing = false;
        }
    }

    public void BackToNormal()
    {
        if (Mathf.Approximately(flashlight.color.r, startCol.r))
        {
            fixFlashLight = false;
            flashTime = 0.0f;
        }
        else
        {
            flashlight.color = Color.Lerp(flashlight.color, startCol, 10.0f * Time.deltaTime);
        }
    }

};
