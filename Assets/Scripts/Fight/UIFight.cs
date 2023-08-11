using System;
using System.Linq;
using Assets.ScriptableObjects;
using Assets.Scripts.InteractiveObjectSystem;
using UnityEngine;

namespace Assets.Fight
{
    public class UIFight : MonoBehaviour
    {
        [SerializeField] private FightPlace _fightPlace;

        public void SetActiveFightPlace(Player.Player player,params Enemy.Enemy[] _enemies)
        {
            gameObject.SetActive(true);
            _fightPlace.Set(player, _enemies.ToList());
        }
    }
}