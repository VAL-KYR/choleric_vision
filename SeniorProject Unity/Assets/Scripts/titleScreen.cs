using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class titleScreen : MonoBehaviour {

    public GameObject selector;

    public int selection;
    private int selectionState;

    public string levelStart;
    
    public GameObject[] selOpt;
    

	// Use this for initialization
	void Start () {
        selectionState = 0;
        
        
    }
	
	// Update is called once per frame
	void Update () {
	
        if(selectionState == 0)
        {
            selOpt[0].SetActive(true);
            selOpt[1].SetActive(false);

            if (Input.GetButtonDown("Action"))
            {
                SceneManager.LoadScene(levelStart);
            }
        }
        else if (selectionState == 1)
        {
            selOpt[1].SetActive(true);
            selOpt[0].SetActive(false);
        }

        if (Input.GetButtonDown("Vertical"))
        {
            if (selectionState == 0)
                selectionState = 1;
            else if (selectionState == 1)
                selectionState = 0;

        }
    }
}
