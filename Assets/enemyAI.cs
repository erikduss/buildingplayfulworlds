using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public Transform target;
    private float speed = 6f;
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float timeBetweenAttacks;


    // Use this for initialization
    void Start()
    {
        Rest();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }

    public void MoveToPlayer()
    {
        //rotate to look at player
        transform.LookAt(target.position);

        if (Vector3.Distance(transform.position, target.position) >= 2)
        {

            transform.position += transform.forward * speed * Time.deltaTime;



            if (Vector3.Distance(transform.position, target.position) <= 2)
            {
                //Here Call any function U want Like Shoot at here or something
            }
        }
    }

    public void Rest()
    {

    }
}
