using Assets.Fight.Element;
using Assets.Weapon;
using UnityEngine;

namespace Assets.Interface
{
    public interface IWeapon
    {
        ParticleSystem ParticleSystem { get; }
        public float Damage { get; }
        public Element Element { get; }
        int ChanceToSplash { get; }
        int MinValueToCriticalDamage { get; }
        int ValueModifier { get; }
    }
}