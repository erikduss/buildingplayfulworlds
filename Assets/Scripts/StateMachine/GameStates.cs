using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStates : MonoBehaviour
{

    public StateMachine gameStateSM;
    public SplashScreenState splashScreen;
    public MainMenuState mainMenu;
    public PlayingState playGame;
    public OptionsState optionsMenu;
    public GameOverState endScene;
    public LoadingState loadingGame;

    private void Start()
    {
        gameStateSM = new StateMachine();
        splashScreen = new SplashScreenState(this, gameStateSM);
        mainMenu = new MainMenuState(this, gameStateSM);
        playGame = new PlayingState(this, gameStateSM);
        optionsMenu = new OptionsState(this, gameStateSM);
        endScene = new GameOverState(this, gameStateSM);
        loadingGame = new LoadingState(this, gameStateSM);

        gameStateSM.Initialize(splashScreen);
    }

    private void Update()
    {
        gameStateSM.CurrentState.HandleInput();

        gameStateSM.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        gameStateSM.CurrentState.PhysicsUpdate();
    }
}
