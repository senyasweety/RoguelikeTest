using Assets.Person.PersonStates;

namespace Assets.Interface
{
    public interface IPersonStateMachine
    {
        public void SetState<T>(T newState) where T : IUnitState;
    }
}