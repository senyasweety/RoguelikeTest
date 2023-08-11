using Assets.Interface;
using UnityEngine;

namespace Assets.Infrastructure.States
{
    public class GameplayState : IState
    {
        private Player.Player _player;

        public GameplayState(GameStateMachine gameStateMachine)
        {
        }

        // public void Enter(Player.Player payload)
        // {
        //     _player = payload;
        //     SubscribeEvents();
        //     Play();
        // }

        public void Exit() => 
            UnsubscribeEvents();

        public void Enter()
        {
        }

        private void Play() => 
            Debug.Log("Play");

        private void SubscribeEvents()
        {
            // _player.OnWon += PlayerWin;
            // _player.OnDied += PlayerLose;
        }

        private void UnsubscribeEvents()
        {
            // _player.OnWon -= PlayerWin;
            // _player.OnDied -= PlayerLose;
        }
        
        private void PlayerLose() => 
            Debug.Log("Lose");

        private void PlayerWin() => 
            Debug.Log("Win");
    }
}