using Assets.Interface;

namespace Assets.Infrastructure
{
    public class Game
    {
        private GameStateMachine _gameStateMachine;
        
        public Game(ICoroutineRunner coroutineRunner) => 
            _gameStateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), new SdkLoader(coroutineRunner));

        public GameStateMachine GameStateMachine => _gameStateMachine;
    }
}