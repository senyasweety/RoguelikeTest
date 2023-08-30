using System;
using UnityEngine;
using AnimationState = Assets.Scripts.AnimationComponent.AnimationState;

namespace Assets.Person
{
    public class UnitAttackPresenter : IDisposable
    {
        private readonly Unit _unit;
        private readonly UnitAttackView _unitAttackView;
        private readonly float _baseHealth;

        public UnitAttackPresenter(Unit unit, UnitAttackView unitAttackView)
        {
            _unit = unit;
            _unitAttackView = unitAttackView;
            FillDataForClips();
            SetElementsSpriteForUI();
            _baseHealth = unit.Healh;
            _unit.HealthChanged += ChangeUIHealthValue;
        }

        private void SetElementsSpriteForUI()
        {
            _unitAttackView.ArmorElement.sprite =
                _unitAttackView.ElementsSpriteView.GetElementSprite(_unit.Armor.Body.Element);
            _unitAttackView.WeaponElement.sprite =
                _unitAttackView.ElementsSpriteView.GetElementSprite(_unit.Weapon.Element);
        }

        public Unit Unit => _unit;

        public UnitAttackView UnitAttackView => _unitAttackView;

        private void FillDataForClips() =>
            _unitAttackView.FillDataForClips(_unit.SpriteAnimation.AnimationClips);

        public void ShowAnimation(AnimationState animationState) =>
            _unitAttackView.SetClip(animationState);

        public void Dispose() => 
            _unit.HealthChanged -= ChangeUIHealthValue;
        
        private void ChangeUIHealthValue(float value)
        {
            float result = 1 - (value / _baseHealth);
            Debug.Log(result);
            _unitAttackView.ChangeUIHealthValue(result);
        }

    }
}