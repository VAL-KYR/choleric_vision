using UnityEngine;
using System.Collections;

public class fogGlobalSettings : MonoBehaviour {

    public Color fogColour;

	// Use this for initialization
	void Start () {
        RenderSettings.fogColor = fogColour;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
