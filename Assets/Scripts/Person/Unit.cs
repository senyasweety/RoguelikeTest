using System;
using Assets.DefendItems;
using Assets.Fight.Element;
using Assets.Interface;
using Assets.Person.PersonStates;
using Assets.Scripts.AnimationComponent;
using UnityEngine;

namespace Assets.Person
{
    public  class Unit
    {
        private float _health;
        private IPersonStateMachine _personStateMachine;
        private IWeapon _weapon;
        private Armor _armor;
        private MagicItem _magicItem;
        
        public Unit(int health, IWeapon weapon, Armor armor, MagicItem magicItem, SpriteAnimation spriteAnimation)
        {
            _health = health;
            _weapon = weapon;
            _armor = armor;
            _magicItem = magicItem;
            SpriteAnimation = spriteAnimation;
            _personStateMachine = new PersonStateMachine();
        }

        public event Action<Unit> Died;
        
        public SpriteAnimation SpriteAnimation { get; }
        public float Healh => _health;
        public IWeapon Weapon => _weapon;
        public Armor Armor => _armor;
        public bool IsDie { get; private set; } = false;
        public IPersonStateMachine PersonStateMachine => _personStateMachine;
        public Sprite Sprite { get; set; }

        public void TakeDamage(IWeapon weapon)
        {
            CalculateDamageMultiplier(weapon);
            ConditionForDead();
        }

        protected virtual void ConditionForDead()
        {
            if (_health <= 0)
            {
                IsDie = true;
                Died?.Invoke(this);
                _health = 0;
            }
            else
                IsDie = false;
        }

        protected virtual void CalculateDamageMultiplier(IWeapon weapon)
        {
            float damageMultiplier = weapon.Damage / (CalculateDamageModifier(weapon.Element) * weapon.Damage + (_armor.Body.Value + _armor.Head.Value));
            _health -= damageMultiplier * weapon.Damage;
        }

        private float CalculateDamageModifier(Element element) =>
            ElementManager.GetDamageModifier(element, _armor.Body.Element);
    }
}