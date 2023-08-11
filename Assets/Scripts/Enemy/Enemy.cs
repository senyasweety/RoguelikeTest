using Assets.DefendItems;
using Assets.Interface;
using Assets.Person;
using Assets.Scripts.AnimationComponent;
using Assets.Scripts.InteractiveObjectSystem;
using UnityEngine;

namespace Assets.Enemy
{
    public class Enemy : Unit
    {
        private string _name;
        private string _data;

        public Enemy(int health, IWeapon weapon, Armor armor, MagicItem magicItem,  SpriteAnimation spriteAnimation) 
            : base(health, weapon, armor, magicItem, spriteAnimation)
        {
        }
        
        public bool IsBoss { get; private set; }

        public void MakeBoss() =>
            IsBoss = true;
    }
}