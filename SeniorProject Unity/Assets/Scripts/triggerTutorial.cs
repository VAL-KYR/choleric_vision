using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class triggerTutorial : MonoBehaviour {

	//private GameObject journal;
	public AudioSource journalWrite;

    public GameObject[] noteBooks;
    public string textTriggerNumber;
    private string textRaw;
    public TextAsset noteText;
    public TextAsset hintText;
    private string note;
    private string hint;

    public bool debug;
    public bool isArmed = true;

    public bool lookTrip = false;

    public bool multiTrip = false;
    public float multiTripResetTime = 5;

    bool soundTrip = false;
    public bool intrigue = false;
    private AudioSource intrigueSound;

    // INIT SUCCESS // 
    public bool progress = false;
    private AudioSource success;
    // INIT SUCCESS // 

    void Start()
    {
		//journalWrite = gameObject.GetComponent<AudioSource>();
        intrigueSound = GameObject.FindGameObjectWithTag("intrigueSounder").GetComponent<AudioSource>();


        if (noteText)
        	note = noteText.text;
		else
			note = null;
		if (hintText)
			hint = hintText.text;
		else
			hint = null;

        // START SUCCESS //
        success = GameObject.FindGameObjectWithTag("successSounder").GetComponent<AudioSource>();
        // START SUCCESS //
    }

    void OnTriggerEnter(Collider other)
    {
        if (isArmed && other.gameObject.CompareTag("GameController") && !lookTrip)
        {
            if (soundTrip == false)
            {
                if (debug)
                    Debug.Log(gameObject + " is Playing");

                //
                soundTrip = true;

                noteBooks = GameObject.FindGameObjectsWithTag("noteBook");

                foreach (GameObject n in noteBooks)
                {
                    n.GetComponent<noteBook>().AddTutText(note, hint);
                }

                

				journalWrite.Play();

                if (intrigue)
                {
                    intrigueSound.Play();
                    intrigue = false;
                }

                if (progress)
                {
                    // PLAY GLOBAL SUCCESS SOUND //
                    success.Play();
                    progress = false;
                    // PLAY GLOBAL SUCCESS SOUND //
                }

                if (multiTrip)
                    StartCoroutine(endSound());
                //
            }
        }

        else
        {
            if (debug)
                Debug.Log(gameObject + " armed state is " + isArmed);
        }
    }

    IEnumerator endSound()
    {
        yield return new WaitForSeconds(multiTripResetTime);
        soundTrip = false;
    }

    public void arm()
    {
        isArmed = true;
        if (debug)
            Debug.Log(gameObject + " is now armed: " + isArmed);
    }

    public void lookTrigger()
    {
        if (!soundTrip && lookTrip && isArmed)
        {
            if (debug)
                Debug.Log(gameObject + " is Playing");

            //
            soundTrip = true;

            noteBooks = GameObject.FindGameObjectsWithTag("noteBook");

            foreach (GameObject n in noteBooks)
            {
                n.GetComponent<noteBook>().AddTutText(note, hint);
            }

			journalWrite.Play();

            if (multiTrip)
                StartCoroutine(endSound());
            //
        }
    }
}
