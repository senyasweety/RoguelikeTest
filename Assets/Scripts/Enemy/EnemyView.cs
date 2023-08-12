using Assets.Interface;
using Assets.ScriptableObjects;
using Assets.Scripts.AnimationComponent;
using Assets.Scripts.InteractiveObjectSystem;
using UnityEngine;

namespace Assets.Enemy
{
    public class EnemyView : InteractiveObject, IEnemyViewReadOnly
    {
        [SerializeField] private int _health;
        [SerializeField] private WeaponScriptableObject _weapon;
        [SerializeField] private ArmorScriptableObject _armor;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private SpriteAnimation _spriteAnimation;

        public int Health => _health;

        public WeaponScriptableObject Weapon => _weapon;

        public ArmorScriptableObject Armor => _armor;

        public Sprite Sprite => _sprite;

        public SpriteAnimation SpriteAnimation => _spriteAnimation;

        public string Name => _name;
    }
}