using Assets.Infrastructure.States;
using Assets.Interface;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class GameBootstrap : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        private void Awake()
        {
            _game = new Game(this);
            _game.GameStateMachine.Enter<BootstrapState>();
            //DontDestroyOnLoad(this);
        }
    }
}