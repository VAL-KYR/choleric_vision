using UnityEngine;
using System.Collections;

public class start : MonoBehaviour {

    public Color fadeColor = Color.black;

    public float rateOfChange;


    public GameObject fader;

    // Use this for initialization
    void Start () {


        Material faderMaterial = new Material(Shader.Find("Transparent/Diffuse"));


        fadeColor.g = 0f;
        fadeColor.r = 0f;
        fadeColor.b = 0f;
        fadeColor.a = 1f;

        faderMaterial.color = fadeColor;
        fader.GetComponent<Renderer>().material = faderMaterial;


        (GetComponent("heartBeat") as MonoBehaviour).enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
        if (fadeColor.a > 0)
        {
            fadeColor.a -= rateOfChange;
        }
        else
        {
            fader.SetActive(false);
            (GetComponent("heartBeat") as MonoBehaviour).enabled = true;
        }
	}
}
