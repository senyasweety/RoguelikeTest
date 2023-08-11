using System.Collections.Generic;
using Assets.DefendItems;
using Assets.Enemy;
using Assets.Interface;
using Assets.Person;
using Assets.ScriptableObjects;
using Assets.Scripts.AnimationComponent;
using Assets.Weapon;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.InteractiveObjectSystem
{
    public class InteractiveEnemyObject : InteractiveObject, IEnemyObjectData
    {
        [SerializeField] private int _health;
        [SerializeField] private WeaponScriptableObject _weapon;
        [SerializeField] private ArmorScriptableObject _armor;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private SpriteAnimation _spriteAnimation;
        #region Delete

        //private Enemy.Enemy _enemy;
        // public ArmorScriptableObject Armor { get; }
        // public IReadOnlyWeaponData Weapon { get; }
        // public int Health { get; }
        // public int Count { get; }

        #endregion

        private int _count;
        private string data = "количество врагов = ";
        private MagicItem _magicItem;

        public string Name => _name;
        public List<Enemy.Enemy> Enemy { get; private set; }

        protected override void OnStart()
        {
            Enemy = new List<Enemy.Enemy>();
            _count = GenerateRandomCountEnemy();
            AddInfoInData(data + _count);
            _weapon.SetNewElement(GetRandomElement());
            _armor.BodyPart.SetNewElement(GetRandomElement());

            for (int i = 0; i < _count; i++)
            {
                Enemy.Enemy enemy = GetEnemy();
                enemy.Sprite = _sprite;
                
                if (Type == ObjectType.Boos)
                    enemy.MakeBoss();
                
                Enemy.Add(enemy);
            }
        }

        private Enemy.Enemy GetEnemy()
        {
            EnemyFactory factory = new EnemyFactory();
            WeaponFactory weaponFactory = new WeaponFactory();
            ArmorFactory armorFactory = new ArmorFactory();

            IWeapon weapon = weaponFactory.Create(_weapon.Damage, _weapon.Element, _weapon.ChanceToSplash, _weapon.MinValueToCriticalDamage,
                _weapon.ValueModifier, _weapon.ParticleSystem);

            Body body = new Body(_armor.BodyPart.Value, _armor.BodyPart.Element);
            Head head = new Head(_armor.HeadPart.Value);

            Armor armor = armorFactory.Create(body, head, _armor.ParticleSystem);

            return factory.Create(weapon, armor, _health, _spriteAnimation);
        }

        private int GenerateRandomCountEnemy() =>
            Random.Range(1, 4);
    }

    public interface IEnemyObjectData : IInteractiveObjectData
    {
        public List<Enemy.Enemy> Enemy { get; }
    }

    public interface IInteractiveObjectData
    {
        public string Name { get; }
        public string Data { get; }
    }
}