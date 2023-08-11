using Assets.Interface;
using Assets.Observces;

namespace Assets.Infrastructure.States
{
    public class MainMenuState : IState, IButtonObserver
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private IButton _button;

        public MainMenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter() => 
            MainMenuButtonPlayObserver.Instance.Registry(this);

        public void Exit() => 
            MainMenuButtonPlayObserver.Instance.UnRegistry(this);

        public void Update()
        {
            _gameStateMachine.Enter<GameplayState>();
            _sceneLoader.LoadScene("LevelGeneration");
        }
    }
}