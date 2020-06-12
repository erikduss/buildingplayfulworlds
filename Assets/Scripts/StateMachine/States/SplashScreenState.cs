using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenState : State
{
    public SplashScreenState(GameStates states, StateMachine stateMachine) : base(states, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Splash screen state");
    }

    public override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(state.mainMenu);
            base.startScene("MainMenu");
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
