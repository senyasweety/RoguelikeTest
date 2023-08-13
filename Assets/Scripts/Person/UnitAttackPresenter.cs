using Assets.Scripts.AnimationComponent;

namespace Assets.Person
{
    public class UnitAttackPresenter
    {
        private readonly Unit _unit;
        private readonly UnitAttackView _unitAttackView;
        
        public UnitAttackPresenter(Unit unit, UnitAttackView unitAttackView)
        {
            _unit = unit;
            _unitAttackView = unitAttackView;
        }
        
        public Unit Unit => _unit;

        public UnitAttackView UnitAttackView => _unitAttackView;

        public void ShowAnimation(AnimationState animationState)
        {
            var clip = _unit.SpriteAnimation.GetClip(animationState);
            _unitAttackView.SetClip(clip);
        }
        
        private void FillUnitAttackView()
        {
        }
    }
}