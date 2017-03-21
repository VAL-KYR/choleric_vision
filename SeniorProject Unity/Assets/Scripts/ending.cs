using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ending : MonoBehaviour {

    private GameObject winRoom;
    private GameObject deathRoom;
    private GameObject player;

    public float timer = 0.0f;

    private bool triggerDeath = false;
    public float deathTime = 3.0f;
    private bool triggerWin = false;
    public float winTime = 3.0f;
    public bool playerDead = false;
    public bool playerSafe = false;

    

	// Use this for initialization
	void Start () {
        winRoom = GameObject.FindGameObjectWithTag("winRoom");
        deathRoom = GameObject.FindGameObjectWithTag("deathRoom");
        player = GameObject.FindGameObjectWithTag("GameController");
    }
	
	// Update is called once per frame
	void Update () {

        // CHECKING FOR PLAYER TP
        if (winRoom.GetComponent<Collider>().bounds.Contains(player.transform.position))
        {
            triggerWin = true;
        }
        else if (deathRoom.GetComponent<Collider>().bounds.Contains(player.transform.position))
        {
            triggerDeath = true;
        }

        // ENDING TIMER
        if (triggerDeath || triggerWin)
        {
            timer += Time.deltaTime;

            if (triggerWin && timer >= winTime)
            {
                playerSafe = true;

                // change scene to credits
                SceneManager.LoadScene("credits", LoadSceneMode.Single);
            }
            else if (triggerDeath && timer >= deathTime)
            {
                playerDead = true;

                // change scene to credits
                SceneManager.LoadScene("credits", LoadSceneMode.Single);

            }

        }



	}
}
