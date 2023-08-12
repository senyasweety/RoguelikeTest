using System;
using System.Collections.Generic;
using Assets.Enemy;
using Assets.Fight.Dice;
using Assets.Interface;
using Assets.Person;
using Assets.Player;
using Assets.ScriptableObjects;
using UnityEngine;

namespace Assets.Fight
{
    public class FightPlace : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private PlayerPoint _playerPosition;
        [SerializeField] private List<EnemyPoint> _spawnPoints;
        [SerializeField] private StepFightView _stepFightView;

        [Space(25)] [SerializeField] private DiceView _leftDice;
        [SerializeField] private DiceView _centerDice;
        [SerializeField] private DiceView _rightDice;

        private Fight _fight;

        private const int Enemy = 1;
        private const int TwoEnemy = 2;
        private const int ThreeEnemy = 3;
        private const int Boss = 4;

        public void Set(PlayerPresenter player, EnemyPresenter enemies)
        {
            #region MyRegion

            switch (enemies.Enemy.Count)
            {
                case Enemy:
                    DisableAllEnemyUI();

                    if (CheckOnTheBoss(enemies.Enemy[Enemy - 1]))
                        break;

                    ShowEnemyUI(_spawnPoints[Enemy - 1]);
                    break;
                case TwoEnemy:
                    DisableAllEnemyUI();
                    ShowEnemyUI(_spawnPoints[TwoEnemy - 1]);
                    break;
                case ThreeEnemy:
                    DisableAllEnemyUI();
                    ShowEnemyUI(_spawnPoints[ThreeEnemy - 1]);
                    break;
            }

            DiceSpriteScriptableObject scriptableObject =
                Resources.Load<DiceSpriteScriptableObject>("BlackDiceSpriteScriptableObject");

            DicePresenter leftDicePresenter =
                new DicePresenter(_leftDice, new DiceModel(scriptableObject.Sprites), this);
            DicePresenter centerDicePresenter =
                new DicePresenter(_centerDice, new DiceModel(scriptableObject.Sprites), this);
            DicePresenter rightDicePresenter =
                new DicePresenter(_rightDice, new DiceModel(scriptableObject.Sprites), this);

            // _fight = new Fight(this, enemies, player, _stepFightView,
            //     new DicePresenterAdapter(leftDicePresenter, centerDicePresenter, rightDicePresenter));

            _fight.Start();

            #endregion
        }

        private void OnDisable() =>
            _fight.Dispose();

        private bool CheckOnTheBoss(Enemy.Enemy enemy)
        {
            if (enemy.IsBoss)
            {
                ShowEnemyUI(_spawnPoints[Boss - 1]);
                return true;
            }

            return false;
        }

        private void ShowEnemyUI(EnemyPoint spawnPoint) => 
            spawnPoint.gameObject.SetActive(true);

        private void DisableAllEnemyUI()
        {
            foreach (SpawnPoint spawnPoint in _spawnPoints)
                spawnPoint.gameObject.SetActive(false);
        }

        public void Set()
        {
            throw new NotImplementedException();
        }
    }
}