using System.Collections.Generic;
using UnityEngine;
using AnimationClip = Assets.Scripts.AnimationComponent.AnimationClip;
using AnimationState = Assets.Scripts.AnimationComponent.AnimationState;

namespace Assets.Person
{
    public class UnitAttackPresenter
    {
        private readonly Unit _unit;
        private readonly UnitAttackView _unitAttackView;
        
        private int _currentClip;

        public UnitAttackPresenter(Unit unit, UnitAttackView unitAttackView)
        {
            _unit = unit;
            _unitAttackView = unitAttackView;
            FillDataForClips();
        }

        public Unit Unit => _unit;

        public UnitAttackView UnitAttackView => _unitAttackView;

        private void FillDataForClips() => 
            _unitAttackView.FillDataForClips(_unit.SpriteAnimation.AnimationClips);

        public void ShowAnimation(AnimationState animationState) => 
            _unitAttackView.SetClip(animationState);
    }
}