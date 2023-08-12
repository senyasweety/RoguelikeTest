using System.Collections.Generic;
using Assets.Fight;
using Assets.Person;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    [RequireComponent(typeof(Button))]
    public class ButtonForGenerateUIFight : MonoBehaviour
    {
        [SerializeField] private UIFight _uiFight;
        [SerializeField] private List<MonoBehaviour> _units;

        Player.Player _player;
        private Button _button;

        private void Awake() =>
            _button = GetComponent<Button>();

        private void OnEnable() =>
            _button.onClick.AddListener(Generate);

        private void OnDisable() =>
            _button.onClick.RemoveListener(Generate);

        private void Generate()
        {
            _uiFight.gameObject.SetActive(true);

            List<Enemy.Enemy> enemies = new List<Enemy.Enemy>();

            foreach (MonoBehaviour unit in _units)
            {
                if (unit.TryGetComponent(out Player.Player player))
                {
                    _player = player;
                    continue;
                }
                
                if (unit.TryGetComponent(out Enemy.Enemy enemy))
                    enemies.Add(enemy);
            }

            // _uiFight.SetActiveFightPlace(_player, enemies.ToArray());
        }
    }
}