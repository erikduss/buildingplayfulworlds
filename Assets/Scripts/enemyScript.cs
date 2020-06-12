using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class enemyScript : MonoBehaviour {

    private Vector3 startPosition;  //Give it a startPosition so it knows where it's 'home' location is.
    private bool wandering = true;  //Set a bool or state so it knows if it's wandering or chasing a player
    private bool chasing = false;
    private float wanderSpeed = 1.8f;  //Give it the movement speeds
    private GameObject target;  //The target you want it to chase
    private NavMeshAgent agent;
    private float wanderRange = 100;
    public GameObject player;
    private bool isAgressive = false;
    private bool changedToCrawl = true;

    public bool isStunned = false;

    public GameController gamemanager;

    public FirstPersonController playerScript;

    private SphereCollider col;
    private Vector3 personalLastSighting;
    private bool playerInSight;
    private Vector3 previousSighting;

    private float attackCooldown = 3f;
    private float attackCDTimer;

    private bool addedToGamemanager = false;

    public AudioSource monsterVoice;

    public Animator anim;

    //When the enemy is spawned via script or if it's pre-placed in the world we want it to first
    //Get it's location and store it so it knows where it's 'home' is
    //We also want ti set it's speed and then start wandering
    void Awake()
    {
        //Get the NavMeshAgent so we can send it directions and set start position to the initial location
        agent = GetComponent("NavMeshAgent") as NavMeshAgent;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<FirstPersonController>();
        col = GetComponent<SphereCollider>();
        agent.speed = wanderSpeed;
        startPosition = this.transform.position;
        //Start Wandering
        InvokeRepeating("Wander", 1f, 5f);
        anim.SetInteger("moving",1);
        anim.SetInteger("battle",0);

        attackCDTimer = Time.time + attackCooldown;
    }

    private void Update()
    {
        if (playerScript.playerDead)
        {
            this.gameObject.SetActive(false);
            return;
        }
        if (isStunned)
        {
            anim.Play("defence-start");
            anim.SetInteger("moving", 14);
            anim.SetInteger("battle", 1);
            agent.isStopped = true;
            return;
        }
        col.radius = 50f;
        var distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance <= 2.5f && Time.time > attackCDTimer)
        {
            transform.LookAt(player.transform);
            agent.speed = 0;
            anim.Play("power_attack");
            changedToCrawl = false;
            attackCDTimer = Time.time + attackCooldown;
        }
        else if (distance <= 2.5f && Time.time <= attackCDTimer)
        {
            anim.SetInteger("moving", 0);
            anim.SetInteger("battle", 1);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("run") && playerInSight)
        {
            agent.speed = 8f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("crawl_idle") && isAgressive && !changedToCrawl)
        {
            anim.SetInteger("moving", 3);
            anim.SetInteger("battle", 2);
            agent.speed = 3f;
            changedToCrawl = true;
            attackCDTimer = Time.time + attackCooldown;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("crawl") && isAgressive && changedToCrawl && Time.time > attackCDTimer)
        {
            agent.speed = 0;
            anim.Play("crawl_to_state");
            anim.SetInteger("battle", 1);
            anim.SetInteger("moving", 2);
            
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle_battle_run_mode") && playerInSight && Time.time > (attackCDTimer - 2f) && isAgressive && !changedToCrawl)
        {
            anim.SetInteger("battle", 2);
            anim.Play("roar");
            monsterVoice.Play();
        }
        else if (playerInSight && !isAgressive)
        {
            CancelInvoke("Wander");
            anim.Play("roar");
            anim.SetInteger("moving", 2);
            anim.SetInteger("battle", 1);
            monsterVoice.Play();
            isAgressive = true;
            changedToCrawl = true;
        }
        
        if (isAgressive)
        {
            NewDestination(player.transform.localPosition);
        }
        
        
    }

    public void getUp()
    {
        anim.SetInteger("moving",0);
        anim.Play("defence-end");
    }

    //When we wander we essentially want to pick a random point and then send the agent there
    //Random.Range is perfect for this.
    //If you're working on a hilly terrain you may want to change your y to a higher point and then
    //Use a raycast down to hit the 'terrain' point, rather than keeping y at 0.
    //y at 0 would only work if you have a completely flat floor.
    void Wander()
    {
        if (playerScript.playerDead) return;
        if (isAgressive) return;
        //Pick a random location within wander-range of the start position and send the agent there
        Vector3 destination = startPosition + new Vector3(Random.Range(-wanderRange, wanderRange),
                                                          0,
                                                          Random.Range(-wanderRange, wanderRange));
        NewDestination(destination);
    }


    //Creating this as it's own method so we can send it directions other when it's just wandering.
    public void NewDestination(Vector3 targetPoint)
    {
        //Sets the agents new target destination to the position passed in
        agent.SetDestination(targetPoint);
    }

    void OnTriggerStay(Collider other)
    {
        if (playerScript.playerDead) return;
        if (other.gameObject == player)
        {
            playerInSight = false;
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < 60f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        if (!addedToGamemanager)
                        {
                            gamemanager.chasedByEnemies += 1;
                            addedToGamemanager = true;
                        }
                    }
                }
            }

            if (playerScript.m_CharacterController.velocity.magnitude > 5.1f)
            {
                if (CalculatePathLength(player.transform.position) <= col.radius)
                {
                    personalLastSighting = player.transform.position;
                    playerInSight = true;
                    if (!addedToGamemanager)
                    {
                        gamemanager.chasedByEnemies += 1;
                        addedToGamemanager = true;
                    }
                }
            }
            else if (isAgressive)
            {
                playerInSight = true;
                if (!addedToGamemanager)
                {
                    gamemanager.chasedByEnemies += 1;
                    addedToGamemanager = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (playerScript.playerDead) return;
        if (other.gameObject == player)
        {
            gamemanager.chasedByEnemies -= 1;
            playerInSight = false;
            isAgressive = false;
            changedToCrawl = true;
            addedToGamemanager = false;
            anim.Play("batle_idle");
            anim.SetInteger("moving", 1);
            anim.SetInteger("battle", 0);
            agent.SetDestination(personalLastSighting);
            InvokeRepeating("Wander", 1f, 5f);
        }
    }

    float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if (agent.enabled)
        {
            agent.CalculatePath(targetPosition, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length+2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLengh = 0f;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLengh += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLengh;
    }
}
