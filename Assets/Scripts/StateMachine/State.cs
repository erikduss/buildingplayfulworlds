using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class State
{
    protected GameStates state;
    protected StateMachine stateMachine;

    protected State(GameStates states, StateMachine stateMachine)
    {
        this.state = states;
        this.stateMachine = stateMachine;
    }

    protected virtual void startScene(string sceneName)
    {
       SceneManager.LoadScene(sceneName);
       Debug.Log("loading: " + sceneName);
    }

    public virtual void Enter()
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
