using Assets.DefendItems;
using Assets.Interface;
using Assets.ScriptableObjects;
using Assets.Scripts.AnimationComponent;
using Assets.Weapon;
using UnityEngine;

namespace Assets.Player
{
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private ArmorScriptableObject _armorScriptableObject;
        [SerializeField] private WeaponScriptableObject _weaponScriptableObject;
        [SerializeField] private SpriteAnimation _spriteAnimation;
        [SerializeField] private Sprite _sprite;

        private Player _player;

        public Player Player => _player;

        private void Start()
        {
            // Если это первый раз или игрок потерял все жизни создаём стартовый билд

            ArmorFactory armorFactory = new ArmorFactory();
            Armor armor = armorFactory.Create(new Body(_armorScriptableObject.BodyPart.Value, _armorScriptableObject.BodyPart.Element),
                new Head(_armorScriptableObject.HeadPart.Value), _armorScriptableObject.ParticleSystem);

            WeaponFactory weaponFactory = new WeaponFactory();
            IWeapon weapon = weaponFactory.Create(_weaponScriptableObject.Damage, _weaponScriptableObject.Element,
                _weaponScriptableObject.ChanceToSplash, _weaponScriptableObject.MinValueToCriticalDamage,
                _weaponScriptableObject.ValueModifier, _weaponScriptableObject.ParticleSystem);

            _player = new Player(_health, weapon, armor, new MagicItem(), _spriteAnimation);
            _player.Sprite = _sprite;
        }
    }
}