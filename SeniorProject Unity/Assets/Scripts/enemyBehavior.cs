using UnityEngine;
using System.Collections;

public class enemyBehavior : MonoBehaviour
{


    //Game Object
    public GameObject player;
    public GameObject safePoint;
    public GameObject spawnPoint;
    //Path trackers
    public GameObject[] curTarget;
    public GameObject[] offTrackTarget;

    private Vector3 spawnLoc;
    private Vector3 playerLoc;
    private Vector3 enemyLoc;
    private Vector3 enemyOriLoc;
    private Vector3 safeLoc;
    private Vector3[] cLoc;
    private Vector3[] cOffLoc;
    public int locNum;
    public int offLocNum;
    public int locNumOffPath;


    private float PlayerEnemyPDistance;
    private float PlayerSafePDistance;
    private float PlayerEnemyDistance;
    private float EnemyCDistance;
    private float PlayerCDistance;

    public float moveSpeed;
    public float rotationSpeed;
    public float followPlayerDis;

    public int playerLife;
    public float monsterAttackRange;
    public int monsterAttackHit;
    private Renderer hBrend;

    private bool enemyOnGuard;
    private bool enemyFollowing;
    private bool enemyReturning;

    private int curC;
    

    // Use this for initialization
    void Start()
    {
        //Gather Static Location points
        enemyOriLoc = transform.position;
        spawnLoc = player.transform.position;
        safeLoc = safePoint.transform.position;
        cLoc = new Vector3[locNum];
        

        //hBrend = hitBox.GetComponent<Renderer>();
        //hBrend.enabled = false;

        for (int i = 0; i < locNum; i++)
            cLoc[i] = curTarget[i].transform.position;

        for (int i = 0; i < offLocNum; i++)
            cOffLoc[i] = offTrackTarget[i].transform.position;


        enemyOnGuard = true;
        enemyFollowing = enemyReturning = false;

        curC = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Update Dynamic Location Point
        playerLoc = player.transform.position;
        enemyLoc = transform.position;

        //Update Distace between points
        //PlayerEnemyPDistance = Vector3.Distance(playerLoc, enemyPointLoc);//Player distance from enemy spawn point
        

        if (enemyOnGuard == true)
        { 
            standGuard();
            print("Guard");
        }
        /*else if (enemyReturning == true)
        {
            goBack();
            print("Return");
        }*/
        else if (enemyFollowing == true)
        {
            followPlayer();
            print("Follow");
        }

    }

    void standGuard()
    {
        EnemyCDistance = Vector3.Distance(enemyLoc, cLoc[curC]);//Enemy distance from tracker
        PlayerEnemyDistance = Vector3.Distance(enemyLoc, playerLoc);//Enemy distance from tracker

        //find vector to target
        Vector3 vectorToTarget = curTarget[curC].transform.position - transform.position;
        
        // move towards the target
        transform.position += vectorToTarget.normalized * Time.deltaTime * moveSpeed;

        //rotate to look at the target
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(curTarget[curC].transform.position - transform.position), rotationSpeed * Time.deltaTime);


        if (EnemyCDistance < 0.5)
        {
            int res = locNum - locNumOffPath - 1;         

            if (curC == res)
                curC = 0;
            else
                curC++;         
        }

        if (PlayerEnemyDistance < followPlayerDis)
        {
            enemyOnGuard = false;
            enemyFollowing = true;
        }
        

    }

    void followPlayer()
    {

        //rotate to look at the player
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), rotationSpeed * Time.deltaTime);

        float closestDistance;
        closestDistance = 0.0f;

        for (int i = 0; i < locNum; i++)
        {
            float currentDistance = Vector3.Distance(playerLoc, cLoc[i]);

            if (i == 0)
            {
                curC = 0;
                closestDistance = currentDistance;
            }
            else if (currentDistance < closestDistance)
            {
                curC = i;
                closestDistance = currentDistance;
            }
        }

        print(curC);

        EnemyCDistance = Vector3.Distance(enemyLoc, cLoc[curC]);//Enemy distance from tracker
        PlayerCDistance = Vector3.Distance(playerLoc, cLoc[curC]);//Player distance from tracker
        PlayerEnemyDistance = Vector3.Distance(playerLoc, enemyLoc);//Player distace from enemy


        if (EnemyCDistance < PlayerCDistance)//Enymy to tracker
        {
            //find vector to target
            Vector3 vectorToTarget = curTarget[curC].transform.position - transform.position;

            // move towards the target
            transform.position += vectorToTarget.normalized * Time.deltaTime * moveSpeed;

            if (EnemyCDistance < 5.0f)
                curC++;
        }
        else if (EnemyCDistance > PlayerCDistance)//Follow Payer
        {
            //find vector to target
            Vector3 vectorToTarget = player.transform.position - transform.position;

            // move towards the target
            transform.position += vectorToTarget.normalized * Time.deltaTime * moveSpeed;
        }

        if (PlayerEnemyDistance < monsterAttackRange)
            attackPlayer();

        //for(int i =0; i < offLocNum; )


    }

    /*void goBack()
    {

        int advap = locNum - locNumOffPath;

        print("Total:" + advap + " Current: " + curC);

   
        EnemyCDistance = Vector3.Distance(enemyLoc, cLoc[curC]);//Enemy distance from tracker

        //find vector to target
        Vector3 vectorToTarget = curTarget[curC].transform.position - transform.position;

        // move towards the target
        transform.position += vectorToTarget.normalized * Time.deltaTime * moveSpeed;

        if (EnemyCDistance < 1)
            curC--;

    }*/





   void attackPlayer()
    {
        playerLife -= monsterAttackHit;

        if (playerLife <= 0)
        {
         //   hBrend.enabled = true;
        }
        else
        {
            transform.position = enemyOriLoc;
            player.transform.position = spawnLoc;
            enemyFollowing = false;
            enemyOnGuard = true;
            curC = 0;
        }
    }
}
