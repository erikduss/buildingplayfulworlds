using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GameController : MonoBehaviour
{

    public int weaponState = 0;
    public GameObject weaponDisplay;

    public PistolController pistol;

    public FirstPersonController player;
    public Text pistolBulletsUI;

    public int chasedByEnemies = 0;

    public GameObject EnemyPrefab;
    public List<GameObject> enemySpawnPoints = new List<GameObject>();

    public int enemyCount = 0;

    private int maxEnemies = 10;

    private bool canSpawnEnemy = true;

    //public InventoryStateMachine inventorySM;
    //public InventoryStates invStates;
    // Start is called before the first frame update
    void Start()
    {
        pistolBulletsUI.gameObject.SetActive(false);
        weaponDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (canSpawnEnemy)
        {
            if(enemyCount < maxEnemies)
            {
                int cooldown = Random.Range(5, 30);
                StartCoroutine(enemySpawnCooldown(cooldown));
                spawnEnemy();
            }
        }
    }

    private IEnumerator enemySpawnCooldown(float cooldown)
    {
        canSpawnEnemy = false;
        yield return new WaitForSeconds(cooldown);
        canSpawnEnemy = true;
    }

    public void spawnEnemy()
    {
        int spawnLocation = Random.Range(0, enemySpawnPoints.Count - 1);
        Instantiate(EnemyPrefab, enemySpawnPoints[spawnLocation].transform.position, Quaternion.identity);
        enemyCount++;
    }

    public void ItemPickedUp(string itemName)
    {
        if(itemName == "Knife_pickup")
        {
            weaponState = 1;
            //inventorySM.ChangeState(invStates.knifeItem);
        }
        if (itemName == "Pistol_pickup")
        {
            weaponState = 2;
            weaponDisplay.SetActive(true);
            pistolBulletsUI.gameObject.SetActive(true);
            updateBulletsPistol();
            //inventorySM.ChangeState(invStates.knifeItem);
        }
        if (itemName == "PistolAmmo")
        {
            player.pistolBullets += 8;
            updateBulletsPistol();
            //inventorySM.ChangeState(invStates.knifeItem);
        }
    }

    public void updateBulletsPistol()
    {
        pistolBulletsUI.text = "Ammo: " + player.loadedBullets + "/" + player.pistolBullets;
    }
}
