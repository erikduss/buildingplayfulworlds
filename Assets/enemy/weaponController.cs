using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class weaponController : MonoBehaviour
{
    private EnemyController enemyScript;
    private FirstPersonController player;
    private Camera mainCamera;

    private SoundManager soundManager;

    private bool playerCanSeeEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        enemyScript = transform.root.GetComponent<EnemyController>();
        soundManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        mainCamera = Camera.main; //the player's camera
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && enemyScript.isAttacking)
        {
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(enemyScript.transform.position);
            bool playerCanSeeEnemy = screenPoint.z > 0 && screenPoint.x > -.8f && screenPoint.x < 1.8f && screenPoint.y > -.8f && screenPoint.y < 1.8f;

                if (playerCanSeeEnemy)
                {
                    soundManager.PlayHitSound();
                    soundManager.PlayHurtSound_Male1();
                }
                else
                {
                    soundManager.PlayHitSound();
                    soundManager.PlayHurtSound_Male1();
                }
            
        }
    }
}
