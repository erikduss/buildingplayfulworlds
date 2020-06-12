using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickUpItem : MonoBehaviour
{
    // Start is called before the first frame update

    private GameController gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameManager.setPickupText(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(gameObject.tag == "Pickup")
                {
                    gameManager.ItemPickedUp(this.gameObject.name);
                }
                else
                {
                    gameManager.ItemPickedUp(this.gameObject.tag);
                }
                gameManager.setPickupText(false);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameManager.setPickupText(false);
    }
}
