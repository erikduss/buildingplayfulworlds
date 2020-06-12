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

    public Text pickupText;

    public int chasedByEnemies = 0;

    public GameObject EnemyPrefab;
    public List<GameObject> enemySpawnPoints = new List<GameObject>();

    public int enemyCount = 0;

    private int maxEnemies = 10;

    private bool canSpawnEnemy = true;
    private bool canSpawnPowerUp = false;

    public Image crosshairImage;

    public GameObject ammoPickupParent;
    public GameObject ammoPrefab;
    private List<Vector3> ammoLocations = new List<Vector3>();

    public List<GameObject> powerups = new List<GameObject>();
    public List<Transform> powerupSpawnLocations = new List<Transform>();

    //public InventoryStateMachine inventorySM;
    //public InventoryStates invStates;
    // Start is called before the first frame update
    void Start()
    {
        pistolBulletsUI.gameObject.SetActive(false);
        weaponDisplay.SetActive(false);
        foreach(Transform child in ammoPickupParent.transform)
        {
            ammoLocations.Add(child.position);
        }
        setPickupText(false);
        StartCoroutine(powerUpSpawnCooldown(1));
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

        if (canSpawnPowerUp)
        {
            spawnPowerup();
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

    private void spawnPowerup()
    {
        GameObject powerup = powerups[Random.Range(0, 2)];
        Instantiate(powerup, powerupSpawnLocations[Random.Range(0, powerupSpawnLocations.Count - 1)].position, Quaternion.identity);
        float cooldown = Random.Range(30, 90);
        StartCoroutine(powerUpSpawnCooldown(cooldown));
    }

    private IEnumerator powerUpSpawnCooldown(float cooldown)
    {
        canSpawnPowerUp = false;
        yield return new WaitForSeconds(cooldown);
        canSpawnPowerUp = true;
    }

    public void resetAmmoPlacements()
    {
        foreach (Transform child in ammoPickupParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(Vector3 ammoPos in ammoLocations)
        {
            Instantiate(ammoPrefab, ammoPos, Quaternion.identity);
        }
    }

    public void setPickupText(bool isActive)
    {
        pickupText.gameObject.SetActive(isActive);
    }

    public void ItemPickedUp(string itemName)
    {
        if(itemName == "Knife_pickup")
        {
            if (weaponState < 1)
            {
                weaponState = 1;
            }
            //inventorySM.ChangeState(invStates.knifeItem);
        }
        if (itemName == "Pistol_pickup")
        {
            weaponState = 2;
            weaponDisplay.SetActive(true);
            pistolBulletsUI.gameObject.SetActive(true);
            updateBulletsPistol();

            if (crosshairImage.gameObject.activeInHierarchy != true)
            {
                crosshairImage.gameObject.SetActive(true);
            }
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
