using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickUpItem : MonoBehaviour
{
    // Start is called before the first frame update

    public GameController gameManager;
    public Text pickupText;

    void Start()
    {
        pickupText.gameObject.SetActive(false);
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
                gameManager.ItemPickedUp(this.gameObject.name);
                pickupText.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pickupText.gameObject.SetActive(false);
    }
}
