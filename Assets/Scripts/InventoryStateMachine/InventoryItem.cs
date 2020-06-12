using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem 
{
    //protected GameStates state;
    public InventoryStates states;
    public GameController gameManager;

    protected InventoryItem(InventoryStates states, InventoryStateMachine inventorySM)
    {

    }

    public virtual void Enter()
    {

    }

    public virtual void UseItem()
    {

    }

    protected virtual void checkItem(string name, string usage, string description)
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
