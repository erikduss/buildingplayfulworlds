using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : State
{
    public MainMenuState(GameStates states, StateMachine stateMachine) : base(states, stateMachine)
    {
    }
    public override void Enter()
    {      
        Debug.Log("main screen state");
    }

    public override void HandleInput() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(state.optionsMenu);
            base.startScene("OptionsScene");
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            stateMachine.ChangeState(state.loadingGame);
            base.startScene("LoadingScene");
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
