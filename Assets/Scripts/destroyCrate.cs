using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyCrate : MonoBehaviour
{

    public GameObject DestroyedSpawnObject;
    private GameController gameManger;

    // Start is called before the first frame update
    void Start()
    {
        gameManger = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            selfDestruct();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E) && gameManger.weaponState >= 1)
            {
                selfDestruct();
            }
        }
    }

    private void selfDestruct()
    {
        Instantiate(DestroyedSpawnObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Destroy(this.gameObject);
    }
}
