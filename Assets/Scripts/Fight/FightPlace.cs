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


        // Места для игрока и врагов на карте битвы
        [SerializeField] private UnitAttackView _playerAttackView;

        [SerializeField] private List<UnitAttackView> _enemyAttackViews;

        private Fight _fight;
        private EnemyPoint _spawnPoint;

        private const int Enemy = 1;
        private const int TwoEnemy = 2;
        private const int ThreeEnemy = 3;
        private const int Boss = 4;

        public void Set(IPlayerPresenter playerPresenter, IEnemyPresenter enemyPresenter)
        {
            foreach (UnitAttackView unitAttackView in _enemyAttackViews)
                unitAttackView.gameObject.SetActive(false);

            #region MyRegion

            SelectEnemyUIPosition(enemyPresenter);

            UnitAttackPresenter playerAttackPresenter =
                new UnitAttackPresenter(playerPresenter.Player, _playerAttackView);
            List<UnitAttackPresenter> enemyAttackPresenters = new List<UnitAttackPresenter>();

            for (int i = 0; i < enemyPresenter.Enemy.Count; i++)
                enemyAttackPresenters.Add(new UnitAttackPresenter(enemyPresenter.Enemy[i], _enemyAttackViews[i]));

            _fight = new Fight(this, enemyAttackPresenters, playerAttackPresenter, _stepFightView,
                GetDicePresenterAdapter());

            _fight.Start();

            #endregion
        }

        private DicePresenterAdapter GetDicePresenterAdapter()
        {
            DiceSpriteScriptableObject scriptableObject =
                Resources.Load<DiceSpriteScriptableObject>("BlackDiceSpriteScriptableObject");

            DicePresenter leftDicePresenter =
                new DicePresenter(_leftDice, new DiceModel(scriptableObject.Sprites), this);
            DicePresenter centerDicePresenter =
                new DicePresenter(_centerDice, new DiceModel(scriptableObject.Sprites), this);
            DicePresenter rightDicePresenter =
                new DicePresenter(_rightDice, new DiceModel(scriptableObject.Sprites), this);

            return new DicePresenterAdapter(leftDicePresenter, centerDicePresenter, rightDicePresenter);
        }

        private void SelectEnemyUIPosition(IEnemyPresenter enemyPresenter)
        {
            switch (enemyPresenter.Enemy.Count)
            {
                case Enemy:
                    DisableAllEnemyUI();

                    if (CheckOnTheBoss(enemyPresenter.Enemy[Enemy - 1]))
                        break;

                    _spawnPoint = _spawnPoints[Enemy - 1];
                    ShowEnemyUI(_spawnPoint);
                    break;
                case TwoEnemy:
                    DisableAllEnemyUI();
                    _spawnPoint = _spawnPoints[TwoEnemy - 1];
                    ShowEnemyUI(_spawnPoint);
                    break;
                case ThreeEnemy:
                    DisableAllEnemyUI();
                    _spawnPoint = _spawnPoints[ThreeEnemy - 1]; 
                    ShowEnemyUI(_spawnPoint);
                    break;
            }
        }

        private void OnDisable() =>
            _fight.Dispose();

        private bool CheckOnTheBoss(Enemy.Enemy enemy)
        {
            if (enemy.IsBoss)
            {
                _spawnPoint = _spawnPoints[Boss - 1]; 
                ShowEnemyUI(_spawnPoint);
                return true;
            }

            return false;
        }

        private void Update()
        {
            ShowEnemyUI(_spawnPoint);
            
            RectTransform rectTransform = _playerPosition.GetComponent<RectTransform>();

            _playerAttackView.transform.position = GetScreenCoordinates(rectTransform).center;
        }

        private void ShowEnemyUI(EnemyPoint spawnPoint)
        {
            for (int i = 0; i < spawnPoint.Points.Count; i++)
            {
                RectTransform rectTransform = spawnPoint.Points[i].GetComponent<RectTransform>();

                _enemyAttackViews[i].transform.position = GetScreenCoordinates(rectTransform).center;
                _enemyAttackViews[i].gameObject.SetActive(true);
            }

            spawnPoint.gameObject.SetActive(true);
        }

        public Rect GetScreenCoordinates(RectTransform uiElement)
        {
            Vector3[] worldCorners = new Vector3[4];
            uiElement.GetWorldCorners(worldCorners);
            Rect result = new Rect(
                worldCorners[0].x,
                worldCorners[0].y,
                worldCorners[2].x - worldCorners[0].x,
                worldCorners[2].y - worldCorners[0].y);
            return result;
        }

        private void DisableAllEnemyUI()
        {
            foreach (SpawnPoint spawnPoint in _spawnPoints)
                spawnPoint.gameObject.SetActive(false);
        }
    }
}