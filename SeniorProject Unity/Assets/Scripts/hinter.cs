using UnityEngine;
using System.Collections;

public class hinter : MonoBehaviour {

    public GameObject[] playerHints;
    public string hint;
    public float displayTime = 0;
    public float time = 0;

    // Use this for initialization
    void Start () {


        foreach (GameObject h in playerHints)
        {
            h.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {

        time += Time.deltaTime;

        if (hint != null && (time < displayTime))
        {
            //Debug.Log("hint received");
            foreach (GameObject h in playerHints)
            {
                if (h.CompareTag(hint))
                {
                    //Debug.Log("hint displayed");
                    h.SetActive(true);
                }
            }
        }
        else
        {
            foreach (GameObject h in playerHints)
            {
                //Debug.Log("hint removed");
                h.SetActive(false);
            }                    

            displayTime = 0.0f;
            hint = null;
            time = 0.0f;
        }
        

	}
}
