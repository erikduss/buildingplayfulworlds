using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStateMachine 
{
    public InventoryItem CurrentItem { get; private set; }

    public void Initialize(InventoryItem startingState)
    {
        CurrentItem = startingState;
        startingState.Enter();
    }

    public void ChangeState(InventoryItem newState)
    {
        CurrentItem.Exit();

        CurrentItem = newState;
        newState.Enter();
    }
}
