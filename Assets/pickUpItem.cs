using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickUpItem : MonoBehaviour
{
    // Start is called before the first frame update

    private GameController gameManager;
    public Text pickupText;
    public Image crosshairImage;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
        pickupText.gameObject.SetActive(false);
        crosshairImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(this.gameObject.name);
            pickupText.gameObject.SetActive(true);
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
                pickupText.gameObject.SetActive(false);
                if(crosshairImage.gameObject.activeInHierarchy != true)
                {
                    crosshairImage.gameObject.SetActive(true);
                }
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pickupText.gameObject.SetActive(false);
    }
}
