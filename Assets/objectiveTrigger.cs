using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class objectiveTrigger : MonoBehaviour
{

    public GameController gameManager;
    public GameObject objectiveBlocker;
    public TextMesh objectiveText;

    // Start is called before the first frame update
    void Start()
    {
        objectiveText.gameObject.SetActive(false);
        objectiveText.text = "You need a knife to cut through the boxes";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(gameManager.weaponState >= 1) // player has knife
            {
                objectiveText.gameObject.SetActive(true);
                objectiveText.text = "Press E to cut through the boxes";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    objectiveText.gameObject.SetActive(false);
                    Destroy(objectiveBlocker.gameObject);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                objectiveText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(objectiveText.gameObject.activeInHierarchy == true)
        {
            objectiveText.gameObject.SetActive(false);
        }
    }
}
