using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : InventoryItem
{
    public bool showInfo = false;
    public Knife(InventoryStates states, InventoryStateMachine inventorySM) : base(states, inventorySM)
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
        if (showInfo)
        {
            base.checkItem("Knife", "Cut and destroy objects", "An old knife this and that, will probably add a better description later. But what do you need to know? Its just a knife, nothing more, nothing less.");
        }
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
