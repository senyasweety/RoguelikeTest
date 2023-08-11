using System;
using System.Collections.Generic;
using Assets.Infrastructure.States;
using Assets.Interface;

namespace Assets.Infrastructure
{
    public class GameStateMachine
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        public GameStateMachine(SceneLoader sceneLoader, SdkLoader sdkLoader)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sdkLoader, sceneLoader),
                [typeof(MainMenuState)] = new MainMenuState(this, sceneLoader),
                [typeof(GameplayState)] = new GameplayState(this),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState newState = ChangeState<TState>();
            newState.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState newState = ChangeState<TState>();
            newState.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();

            TState newState = GetState<TState>();
            _currentState = newState;

            return newState;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}