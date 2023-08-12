using Assets.ScriptableObjects;
using Assets.Scripts.AnimationComponent;
using UnityEngine;

namespace Assets.Interface
{
    public interface IEnemyViewReadOnly
    {
        public int Health { get; }

        public WeaponScriptableObject Weapon { get; }

        public ArmorScriptableObject Armor { get; }

        public Sprite Sprite { get; }

        public SpriteAnimation SpriteAnimation { get; }

        public string Name { get; }
    }
}