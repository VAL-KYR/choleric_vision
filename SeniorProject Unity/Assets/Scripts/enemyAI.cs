using UnityEngine;
using System.Collections;

public class enemyAI : MonoBehaviour {

    public bool debug;

    private Animator monstAni;

    private GameObject player;
    private GameObject safePoint;
    private GameObject spawnPoint;
    //Path trackers
    public GameObject[] curTarget;
    
   

    private Vector3 enemyLoc;
    private Vector3 spawnLoc;
    private Vector3 playerLoc;
    private Vector3 enemyOriLoc;
    private Vector3 safeLoc;
    private Vector3[] cLoc;
    public int locNum;
    private Vector3 lookAtRigPos;

    private float PlayerEnemyDistance;
    private float PlayerEnemyPDistance;
    private float EnemyCDistance;
    private float PlayerCDistance;

    public float visonDistance;
    public float listenDistance;
    private float visionMax;
    private float curVisionDis;
    private float visDif;

    public float moveSpeed;
    public float rotationSpeed;
    public float followPlayerDis;
    public GameObject[] viewPoint;
    private Vector3[] vpPosition;
    private Renderer[] pointMesh;
    private Vector3 centerVec;
    private GameObject lookAtRig;
    private Vector3 lookAtPoint;
    private GameObject mh;

    public int playerLife;
    public float monsterAttackRange;
    public int monsterAttackHit;

    private bool enemyOnGuard;
    private bool enemySearching;
    private bool enemyFollowing;

    private int curC;

    private float presence;
    private float curHearing;

    public GameObject monsterLookAt;

    private float aniPlayTime;
    private float curAniPlayTime;

    public float coolDownTime;
    private float curCoolDownTime;
    private bool prevSearch;
    
    public bool lightOn;
    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectWithTag("GameController");
        safePoint = GameObject.FindGameObjectWithTag("spawnPoint");
        spawnPoint = GameObject.FindGameObjectWithTag("enemySpawnPoint");
        lookAtRig = GameObject.FindGameObjectWithTag("monsterLookAt");
        mh = GameObject.FindGameObjectWithTag("monsterHead");
        monstAni = GetComponent<Animator>();
        
        visDif = visionMax - visonDistance;

        vpPosition = new Vector3[3];

        spawnLoc = safePoint.transform.position;
        enemyOriLoc = spawnPoint.transform.position;

        cLoc = new Vector3[locNum];

        for (int i = 0; i < locNum; i++)
            cLoc[i] = curTarget[i].transform.position;
        

        enemyOnGuard = true;
        enemyFollowing = enemySearching = false;

        curC = 0;

        monstAni.SetBool("Roaming", true);
        monstAni.SetBool("Following", false);
        monstAni.SetBool("Searching", false);


        pointMesh = new Renderer[locNum];

        for (int i = 0; i < locNum; i++)
            pointMesh[i] = curTarget[i].GetComponent<Renderer>();

        aniPlayTime = curAniPlayTime = 1.958f;
        coolDownTime = curCoolDownTime;
        prevSearch = false;

        visionMax = 30.0f;

    }
	
	// Update is called once per frame
	void Update () {

        if (lightOn)
            curVisionDis = visonDistance - visDif;
        else
            curVisionDis = visonDistance;

        lookAtRigPos = lookAtRig.transform.position;
        lookAtRig.transform.position = new Vector3(lookAtRigPos[0], lookAtRigPos[1], lookAtRigPos[2] + curVisionDis);

        RaycastHit hit;

        PlayerEnemyDistance = Vector3.Distance(enemyLoc, playerLoc);//Enemy distance from tracker

        presence = player.GetComponent<Presence>().curPres;
        curHearing = listenDistance * (presence * 2);


        playerLoc = player.transform.position;
        enemyLoc = transform.position;

        for (int i = 0; i < 3; i++)
            vpPosition[i] = viewPoint[i].transform.position;

        centerVec = (vpPosition[0] + vpPosition[1] + vpPosition[2]) / 3;

        lookAtPoint = lookAtRig.transform.position;

        if (Physics.Raycast(centerVec, -mh.transform.forward, out hit, 15) && PlayerEnemyDistance < curVisionDis)
        {
            if (hit.collider.tag == "GameController")
            {
                monsterLookAt = hit.transform.gameObject;
            }

            
        }
        else
        {
            monsterLookAt = null;
        }



        if (debug)
        {

            Debug.DrawLine(centerVec, lookAtPoint, Color.red);

            for (int i = 0; i < locNum; i++)
                pointMesh[i].enabled = true;
        }
        else
        {

            for (int i = 0; i < locNum; i++)
                pointMesh[i].enabled = false;
        }





        if (enemyOnGuard)
            standGuard();
        else if (enemySearching)
            monsterSeach();
        else if (enemyFollowing)
            followPlayer();
    }

    void standGuard()
    {
        EnemyCDistance = Vector3.Distance(enemyLoc, cLoc[curC]);//Enemy distance from tracker
        

        //find vector to target
        Vector3 vectorToTarget = curTarget[curC].transform.position - transform.position;

        // move towards the target
        transform.position += vectorToTarget.normalized * Time.deltaTime * moveSpeed;

        //rotate to look at the target
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(curTarget[curC].transform.position - transform.position), rotationSpeed * Time.deltaTime);


        if (EnemyCDistance < 0.5)
        {
            int res = locNum - 1;

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


        PlayerEnemyDistance = Vector3.Distance(playerLoc, enemyLoc);

        if(!prevSearch)
        {
            if (PlayerEnemyDistance < curHearing)
            {
                monstAni.SetBool("Roaming", false);
                monstAni.SetBool("Following", false);
                monstAni.SetBool("Searching", true);

                enemySearching = true;
                enemyFollowing = enemyOnGuard = false;
            }
        }
        else
        {
            if (curCoolDownTime < 0.0f)
            {
                prevSearch = false;
                curCoolDownTime = coolDownTime;
            }
            else
                curCoolDownTime -= Time.deltaTime;
        }


        
    }

    void monsterSeach()
    {
        if (monsterLookAt == player)
        {
            monstAni.SetBool("Roaming", false);
            monstAni.SetBool("Following", true);
            monstAni.SetBool("Searching", false);

            enemyFollowing = true;
            enemySearching = enemyOnGuard = false;

            curAniPlayTime = aniPlayTime;
        }

        if (curAniPlayTime < 0.0f)
        {
            monstAni.SetBool("Roaming", true);
            monstAni.SetBool("Following", false);
            monstAni.SetBool("Searching", false);

            enemyOnGuard = true;
            enemyFollowing = enemySearching = false;


            curAniPlayTime = aniPlayTime;
            prevSearch = true;
        }
        else
            curAniPlayTime -= Time.deltaTime;
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

    void OnDrawGizmosSelected()
    {
        if(debug)
        {
            Gizmos.DrawWireSphere (transform.position, curHearing);
        }

        
    }


}
