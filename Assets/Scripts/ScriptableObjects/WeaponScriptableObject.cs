using System;
using Assets.Fight.Element;
using Assets.Weapon;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObject/Weapon", order = 0)]
    public class WeaponScriptableObject : ScriptableObject, IReadOnlyWeaponData
    {
        [field: SerializeField] public int ChanceToSplash { get; private set; }
        [field: SerializeField] public int MinValueToCriticalDamage { get; private set; }
        [field: SerializeField] public int ValueModifier { get; private set; }
        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }

        [field: SerializeField] public Element Element { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }

        public void SetNewElement(Element element) =>
            Element = element;

    }

    public interface IReadOnlyWeaponData
    {
         public int ChanceToSplash { get;}
         public int MinValueToCriticalDamage { get;}
         public int ValueModifier { get;}
         public ParticleSystem ParticleSystem { get;}
         public Element Element { get;}
         int Damage { get; }
    }
}