using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    private Animator anim;
    private bool canAttack = true;
    public bool isAttacking = false;

    private NavMeshAgent agent;

    private FirstPersonController playerScript;

    private CapsuleCollider enemyCollider;

    public int enemyHealth = 100;

    public bool enemyDied = false;
    public bool stunned = false;

    private AudioSource enemyAudio;

    private SoundManager soundManager;
    private GameController gameManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.gameObject.GetComponent<FirstPersonController>();
        enemyCollider = this.gameObject.GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();

        enemyAudio = this.gameObject.GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();

        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.position;
        enemyHealth = 100;

        soundManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyDied || stunned)
        {
            return;
        }
        if (Vector3.Distance(player.position, this.transform.position) < 500)
        {
            Vector3 direction = player.position - this.transform.position;
            float angle = 0;
            direction.y = 0;

            if (canAttack)
            {
                Vector3 targetDir = (player.position - transform.position);
                Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                float singleStep = agent.speed * Time.deltaTime;

                angle = Vector3.Angle(targetDir, transform.forward);

                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0.0f);
                newDirection.y = 0;
                transform.rotation = Quaternion.LookRotation(newDirection);

                if (angle < 31)
                {
                    targetDir.y = 0;
                    transform.rotation = Quaternion.LookRotation(targetDir);
                }
            }

            //anim.SetBool("IsIdle", false);
            if(direction.magnitude > 2 && canAttack)
            {
                agent.destination = player.position;
                if(Vector3.Distance(player.position, this.transform.position) > 10)
                {
                    anim.SetBool("Running", true);
                    agent.speed = 3f;
                }
                else
                {
                    anim.SetBool("Running", false);
                    agent.speed = 2f;
                }
                anim.SetFloat("Speed", agent.speed);
                isAttacking = false;
                anim.SetBool("IsAttacking", isAttacking);
            }
            else if(angle < 30)
            {
                isAttacking = true;
                anim.SetBool("IsAttacking", isAttacking);

                if (canAttack)
                {
                   StartCoroutine(attackCooldown(5));
                   Attack(Random.Range(1, 4)); //melee kick is a special move for if the player is blocking
                }
                
            }
        }
        else
        {
            anim.SetBool("IsAttacking", false);
        }
    }

    private void Attack(int attackID)
    {
        switch (attackID)
        {
            case 1:
                anim.Play("melee_Combo_2");
                StartCoroutine(playAttackSound(0.75f, true));
                break;
            case 2:
                anim.Play("Standing Melee Attack Horizontal");
                StartCoroutine(playAttackSound(0.75f, false));
                break;
            case 3:
                anim.Play("Standing Melee Attack Downward");
                StartCoroutine(playAttackSound(0.75f, false));
                break;
        }
    }

    private IEnumerator playAttackSound(float waitingTime, bool combo)
    {
        yield return new WaitForSeconds(waitingTime);
        soundManager.PlayAttackSound_Female2(enemyAudio);
        if (combo)
        {
            yield return new WaitForSeconds(waitingTime);
            soundManager.PlayAttackSound_Female2(enemyAudio);
            yield return new WaitForSeconds(waitingTime);
            soundManager.PlayAttackSound_Female2(enemyAudio);
        }
    }

    private IEnumerator attackCooldown(int waitingTime)
    {
        canAttack = false;
        yield return new WaitForSeconds(waitingTime);
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        if(enemyDied)
        {
            return;
        }

        agent.destination = this.transform.position;
        agent.speed = 0f;

        enemyHealth -= damage;
        anim.Play("Hurt_Back");
        StunEnemy(1);
        //hurt animation
        string pos = PlayerPosition(false);

        if(pos == "front")
        {
            anim.Play("Hurt_Front");
        }
        else if(pos == "back")
        {
            anim.Play("Hurt_Back");
        }

        if (enemyHealth <= 0)
        {
            string deathPos = PlayerPosition(true);
            StartCoroutine("DeleteEnemy");
            isAttacking = false;
            enemyCollider.enabled = false;
            soundManager.PlayDeathSound_Female2(enemyAudio);
            gameManager.enemyCount--;
            switch (deathPos)
            {
                case "front":
                    anim.SetBool("Dead", true);
                    anim.Play("Died_FrontFall");
                    enemyDied = true;
                    break;
                case "back":
                    anim.SetBool("Dead", true);
                    anim.Play("Died_BackFall");
                    enemyDied = true;
                    break;
                case "right":
                    anim.SetBool("Dead", true);
                    anim.Play("Died_RightFall");
                    enemyDied = true;
                    break;
                case "left":
                    anim.SetBool("Dead", true);
                    anim.Play("Died_LeftFall");
                    enemyDied = true;
                    break;
            }
        }
        else
        {
            soundManager.PlayHurtSound_Female2(enemyAudio);
        }
    }

    private string PlayerPosition(bool useAll)
    {
        Vector3 relativePoint;
        relativePoint = transform.InverseTransformPoint(player.position);
        if (relativePoint.z > 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.z))
        {
            return ("front");
        }
        else if (relativePoint.z < 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.z))
        {
            return ("back");
        }
        if (relativePoint.x < 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y) && useAll)
        {
            return("left");
        }
        else if (relativePoint.x > 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y) && useAll)
        {
            return ("right");
        }

        return "front"; //if nothing gets activated for some reason
        
    }

    private IEnumerator applyStunTime(int waitingTime)
    {
        stunned = true;
        anim.SetBool("Stunned", true);
        yield return new WaitForSeconds(waitingTime);
        stunned = false;
        anim.SetBool("Stunned", false);
    }

    private IEnumerator DeleteEnemy()
    {
        yield return new WaitForSeconds(15);
        Destroy(this.gameObject);
    }

    private void StunEnemy(int stunTime)
    {
        StartCoroutine(applyStunTime(stunTime));
    }
}
