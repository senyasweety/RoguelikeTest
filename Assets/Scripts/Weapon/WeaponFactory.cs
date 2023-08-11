using Assets.Fight.Element;
using Assets.Interface;
using UnityEngine;

namespace Assets.Weapon
{
    public class WeaponFactory
    {
        public IWeapon Create(int damage, Element element, int chanceToSplash, int minValueToCriticalDamage, int valueModifier, ParticleSystem particleSystem) =>
            new Weapon(damage, element, chanceToSplash, minValueToCriticalDamage, valueModifier, particleSystem);
    }
}