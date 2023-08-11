using Assets.DefendItems;
using Assets.Interface;
using Assets.Person;
using Assets.Person.PersonStates;
using Assets.Scripts.AnimationComponent;
using UnityEngine;

namespace Assets.Enemy
{
    public class EnemyFactory
    {
        private const int BaseHealthFirstLevel = 100;

        // public Enemy Create(IWeapon weapon, Armor armor) =>
        //     new Enemy(BaseHealthFirstLevel, weapon, armor, new MagicItem());
        
        public Enemy Create(IWeapon weapon, Armor armor, int health, SpriteAnimation spriteAnimation) =>
            new Enemy(health, weapon, armor, new MagicItem(), spriteAnimation);

    }
}