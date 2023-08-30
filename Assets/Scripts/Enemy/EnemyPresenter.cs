using System;
using System.Collections.Generic;
using Assets.DefendItems;
using Assets.Fight.Element;
using Assets.Interface;
using Assets.Scripts.InteractiveObjectSystem;
using Assets.Weapon;
using Random = UnityEngine.Random;

namespace Assets.Enemy
{
    public class EnemyPresenter : IEnemyPresenter
    {
        private readonly EnemyView _enemyView;

        private List<Enemy> _enemy;
        private int _count;

        public EnemyPresenter(EnemyView enemyView)
        {
            _enemyView = enemyView;
            Start();
        }

        public EnemyView EnemyView => _enemyView;
        public IReadOnlyList<Enemy> Enemy => _enemy;

        private void Start()
        {
            _enemy = new List<Enemy>();
            _count = GenerateRandomCountEnemy();
            string data = _enemyView.Data;
            data += _count;
            
            _enemyView.Weapon.SetNewElement(GetRandomElement());
            _enemyView.Armor.BodyPart.SetNewElement(GetRandomElement());

            for (int i = 0; i < _count; i++)
            {
                Enemy enemy = GetEnemy();
                enemy.Sprite = _enemyView.Sprite;

                if (_enemyView.Type == ObjectType.Boos)
                    enemy.MakeBoss();

                _enemy.Add(enemy);
            }
        }

        private Element GetRandomElement() => (Element)Random.Range(0, Enum.GetValues(typeof(Element)).Length);

        private Enemy GetEnemy()
        {
            EnemyFactory factory = new EnemyFactory();
            WeaponFactory weaponFactory = new WeaponFactory();
            ArmorFactory armorFactory = new ArmorFactory();

            IWeapon weapon = CreateWeapon(weaponFactory);

            Body body = new Body(_enemyView.Armor.BodyPart.Value, GetRandomElement());
            Head head = new Head(_enemyView.Armor.HeadPart.Value);

            Armor armor = armorFactory.Create(body, head, _enemyView.Armor.ParticleSystem);

            return factory.Create(weapon, armor, _enemyView.Health, _enemyView.SpriteAnimation);
        }

        private IWeapon CreateWeapon(WeaponFactory weaponFactory)
        {
            return weaponFactory.Create(
                _enemyView.Weapon.Damage, GetRandomElement(), _enemyView.Weapon.ChanceToSplash,
                _enemyView.Weapon.MinValueToCriticalDamage,
                _enemyView.Weapon.ValueModifier, _enemyView.Weapon.ParticleSystem);
        }

        private int GenerateRandomCountEnemy() =>
            Random.Range(1, 4);
    }

    public interface IEnemyPresenter : IUnitPresenter
    {
        public EnemyView EnemyView { get; }
        public IReadOnlyList<Enemy> Enemy { get; }
    }

    public interface IUnitPresenter
    {
    }
}