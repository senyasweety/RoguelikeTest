using Assets.Fight.Element;
using Assets.Interface;
using UnityEngine;

namespace Assets.Weapon
{
    public class Weapon : IWeapon
    {
        public Weapon(float damage, Element element, int chanceToSplash, int minValueToCriticalDamage, int valueModifier, ParticleSystem particleSystem)
        {
            ChanceToSplash = chanceToSplash;
            MinValueToCriticalDamage = minValueToCriticalDamage;
            ValueModifier = valueModifier;
            ParticleSystem = particleSystem;
            Damage = damage;
            Element = element;
        }

        public float Damage { get; }
        public Element Element { get; }
        public ParticleSystem ParticleSystem { get; }
        public int ChanceToSplash { get; }
        public int MinValueToCriticalDamage { get; }
        public int ValueModifier { get; }
    }
}