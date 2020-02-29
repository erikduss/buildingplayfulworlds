using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingState : State
{
    public LoadingState(GameStates states, StateMachine stateMachine) : base(states, stateMachine)
    {
    }

    public override void Enter()
    {
       DontDestroyOnLoad scenesLoader = GameObject.FindWithTag("SceneStates").GetComponent<DontDestroyOnLoad>();
       scenesLoader.StartCoroutine("startLoading");
       //stateMachine.ChangeState(state.playGame);
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
        stateMachine.ChangeState(state.playGame);
    }
}
