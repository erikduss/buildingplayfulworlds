using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public int weaponState = 0;
    public GameObject weaponDisplay;

    public PistolController pistol;

    //public InventoryStateMachine inventorySM;
    //public InventoryStates invStates;
    // Start is called before the first frame update
    void Start()
    {
        weaponDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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
            //inventorySM.ChangeState(invStates.knifeItem);
        }
    }
}
