using Assets.Scripts.AnimationComponent;
using UnityEngine;

namespace Assets.Person.PersonStates
{
    public abstract class PersonState : IUnitState
    {
        [SerializeField] protected SpriteAnimation SpriteAnimation;

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }
    }
}