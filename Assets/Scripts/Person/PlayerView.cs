using Assets.ScriptableObjects;
using Assets.Scripts.AnimationComponent;
using Assets.Scripts.InteractiveObjectSystem;
using UnityEngine;

namespace Assets.Person
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private ArmorScriptableObject _armorScriptableObject;
        [SerializeField] private WeaponScriptableObject _weaponScriptableObject;
        [SerializeField] private SpriteAnimation _spriteAnimation;
        [SerializeField] private Sprite _sprite;

        public int Health => _health;

        public ArmorScriptableObject ArmorScriptableObject => _armorScriptableObject;

        public WeaponScriptableObject WeaponScriptableObject => _weaponScriptableObject;

        public SpriteAnimation SpriteAnimation => _spriteAnimation;

        public Sprite Sprite => _sprite;

    }
}