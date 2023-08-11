using Assets.Scripts.AnimationComponent;

namespace Assets.Person.PersonStates
{
    public class PersonStateTakeDamage : PersonState
    {
        private SpriteAnimation _spriteAnimation;

        public override void Enter()
        {
            //_spriteAnimation = GetComponent<SpriteAnimation>();
            _spriteAnimation.SetClip(AnimationState.Hit);
        }
    }
}