using UnityEngine;
using System.Collections;

public class monsterAnimator : MonoBehaviour {

    public Animator anim;

    // For triggering later
    static int idleHash = Animator.StringToHash("idleing");
    static int chaseHash = Animator.StringToHash("chasing");
    static int searchHash = Animator.StringToHash("searching");
    static int attackHash = Animator.StringToHash("attack");

    // these are named after the actual layer in unity for the state machine
    static int idleStateHash = Animator.StringToHash("Base Layer.Idle");
    static int chaseStateHash = Animator.StringToHash("Base Layer.Chase");
    static int searchStateHash = Animator.StringToHash("Base Layer.Search");
    static int attackStateHash = Animator.StringToHash("Base Layer.Attack");

    // state tracker
    public AnimatorStateInfo monsterAnimState;

    public bool idle;
    public bool search;
    public bool chase;
    public bool attack;
    public bool headRotating = false;

    // UI FOR PLAYER
    public bool devAnimTesting = false;
    //

    // Use this for initialization
    void Start () {
        anim = gameObject.GetComponent<Animator>();

        // REMOVE FOR ERICA'S VERSION MAYBE? update should fix everything
        if (devAnimTesting)
            idle = true; 
        else
            idle = false;

        search = false;
        chase = false;
        attack = false;
        headRotating = false;
    }

    // Update is called once per frame
    void Update() {

        // accessing animation state
        monsterAnimState = anim.GetCurrentAnimatorStateInfo(0);

        // Set bool parameters in anim from these bools
        anim.SetBool("idleing", idle);
        anim.SetBool("searching", search);
        anim.SetBool("chasing", chase);


        // Developer voice test buttons
        if (devAnimTesting)
        {
            if (Input.GetButtonDown("DevVoice1"))
            {
                idle = true;
                search = false;
                chase = false;
            }
            if (Input.GetButtonDown("DevVoice2"))
            {
                idle = false;
                search = true;
                chase = false;
            }
            if (Input.GetButtonDown("DevVoice3"))
            {
                idle = false;
                search = false;
                chase = true;
            }
            if (Input.GetButtonDown("DevVoice4"))
            {
                attack = true;
            }
        }

        /*
        if(monsterAnimState.fullPathHash == searchStateHash && monsterAnimState.length > monsterAnimState.normalizedTime)
        {
            headRotating = true;
        }
        else
        {
            headRotating = false;
        }
        */

        if (idle)
        {
            gameObject.GetComponent<monsterSound>().Voice("ramble", 0.0f, true);
            //monsterAnimState.fullPathHash == idleStateHash
        }
        else if (search)
        {
            gameObject.GetComponent<monsterSound>().Voice("notice", 0.0f, true);
            //monsterAnimState.fullPathHash == searchStateHash
        }
        else if (chase)
        {
            gameObject.GetComponent<monsterSound>().Voice("alert", 0.0f, true);
            //monsterAnimState.fullPathHash == chaseStateHash
        }

        // CHANGE FOR ERICA'S VERSION'
    
        // make this animation driven later for now just trigger on hit
        // also later it'll do a check for box collider on hand hit player for "death"
        // trigger death function from here

        if (attack)
        {
            // Trigger animation
            anim.SetTrigger("attack");

            // making the attack animation bool instant this way the whole function will only run once
            if(monsterAnimState.fullPathHash != attackStateHash)
            {
                attack = false;
            }
            else
            {
                gameObject.GetComponent<monsterSound>().Voice("attack", 0.5f, false);
            }
            

            // reset animations if in testing mode
            if (devAnimTesting)
            {
                idle = true;
                search = false;
                chase = false;
            }

            //monsterAnimState.fullPathHash == attackStateHash
        }
        
    }
}
