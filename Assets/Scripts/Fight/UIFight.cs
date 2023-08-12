using System;
using System.Linq;
using Assets.Enemy;
using Assets.Player;
using Assets.ScriptableObjects;
using Assets.Scripts.InteractiveObjectSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Fight
{
    public class UIFight : MonoBehaviour
    {
        [SerializeField] private Transform _globalMap;
        [SerializeField] private Transform _battlefieldMap;
        [SerializeField] private FightPlace _fightPlace;

        private PlayerPresenter _playerPresenter;
        private EnemyPresenter _enemyPresenter;

        public void SetActiveFightPlace(PlayerPresenter playerPresenter, EnemyPresenter enemyPresenter)
        {
            _enemyPresenter = enemyPresenter;
            _playerPresenter = playerPresenter;
            
            _battlefieldMap.gameObject.SetActive(true);
            _globalMap.gameObject.SetActive(true);

            _fightPlace.Set(_playerPresenter, _enemyPresenter);
        }
    }
}