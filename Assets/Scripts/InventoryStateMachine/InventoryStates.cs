using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStates : MonoBehaviour
{
    public InventoryStateMachine inventoryStateSM;
    public Knife knifeItem;
    public EmptyInventory empty;

    private void Start()
    {
        inventoryStateSM = new InventoryStateMachine();
        knifeItem = new Knife(this, inventoryStateSM);

        inventoryStateSM.Initialize(empty);
    }

    private void Update()
    {
        inventoryStateSM.CurrentItem.HandleInput();

        inventoryStateSM.CurrentItem.LogicUpdate();
    }

    private void FixedUpdate()
    {
        inventoryStateSM.CurrentItem.PhysicsUpdate();
    }
}
