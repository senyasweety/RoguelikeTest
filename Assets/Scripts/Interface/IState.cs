using System;

namespace Assets.Interface
{
    public interface IState : IExitableState
    {
        void Enter();
    }
    
    public interface IPayloadState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }

    public interface IParameterState<TPayload>
    { }
    
    public interface IExitableState
    {
        void Exit();
    }
    
}