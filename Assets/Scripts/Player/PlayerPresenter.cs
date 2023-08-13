using Assets.DefendItems;
using Assets.Enemy;
using Assets.Interface;
using Assets.Person;
using Assets.Weapon;

namespace Assets.Player
{
    public class PlayerPresenter : IPlayerPresenter
    {
        private readonly PlayerView _playerView;

        private readonly Player _player;

        public PlayerPresenter(PlayerView playerView)
        {
            _playerView = playerView;
            _player = GetNewPlayer();
        }

        public PlayerView PlayerView => _playerView;
        public Player Player => _player;

        private Player GetNewPlayer()
        {
            // Если это первый раз или игрок потерял все жизни создаём стартовый билд

            ArmorFactory armorFactory = new ArmorFactory();
            Armor armor = armorFactory.Create(
                new Body(_playerView.ArmorScriptableObject.BodyPart.Value,
                    _playerView.ArmorScriptableObject.BodyPart.Element),
                new Head(_playerView.ArmorScriptableObject.HeadPart.Value),
                _playerView.ArmorScriptableObject.ParticleSystem);

            WeaponFactory weaponFactory = new WeaponFactory();
            IWeapon weapon = weaponFactory.Create(
                _playerView.WeaponScriptableObject.Damage, _playerView.WeaponScriptableObject.Element,
                _playerView.WeaponScriptableObject.ChanceToSplash,
                _playerView.WeaponScriptableObject.MinValueToCriticalDamage,
                _playerView.WeaponScriptableObject.ValueModifier, _playerView.WeaponScriptableObject.ParticleSystem);

            Player player = new Player(_playerView.Health, weapon, armor, new MagicItem(), _playerView.SpriteAnimation);
            player.Sprite = _playerView.Sprite;
            return player;
        }
    }

    public interface IPlayerPresenter : IUnitPresenter
    {
        public PlayerView PlayerView { get; }
        public Player Player { get; }
    }
}
