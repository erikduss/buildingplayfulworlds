using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyInventory : InventoryItem
{
    public EmptyInventory(InventoryStates states, InventoryStateMachine inventorySM) : base(states, inventorySM)
    {

    }

    public override void Enter()
    {

    }

    public override void UseItem()
    {

    }

    public override void HandleInput()
    {
        
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void Exit()
    {

    }
}
