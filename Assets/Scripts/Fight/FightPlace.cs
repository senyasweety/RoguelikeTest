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

        [Space(25)] 
        [SerializeField] private ElementsDamagePanel _elementsDamagePanel;
        // Места для игрока и врагов на карте битвы
        [SerializeField] private UnitAttackView _playerAttackView;

        [SerializeField] private List<UnitAttackView> _enemyAttackViews;

        private Fight _fight;
        private EnemyPoint _spawnPoint;

        private const int Enemy = 1;
        private const int TwoEnemy = 2;
        private const int ThreeEnemy = 3;
        private const int Boss = 4;
        
        private IElementsDamagePanel _IelementsDamagePanel;
        private RectTransform _playerRectTransform;

        private void OnEnable()
        {
            _IelementsDamagePanel = _elementsDamagePanel;
            _playerRectTransform = _playerPosition.GetComponent<RectTransform>();

            #region Add subscrib event on click panel InfoInLine
            _IelementsDamagePanel.FireElementInfoLine.InfoInLine.ButtonAttack.onClick.AddListener(_IelementsDamagePanel.HidePanel);
            _IelementsDamagePanel.MetalElementInfoLine.InfoInLine.ButtonAttack.onClick.AddListener(_IelementsDamagePanel.HidePanel);
            _IelementsDamagePanel.StoneElementInfoLine.InfoInLine.ButtonAttack.onClick.AddListener(_IelementsDamagePanel.HidePanel);
            _IelementsDamagePanel.TreeElementInfoLine.InfoInLine.ButtonAttack.onClick.AddListener(_IelementsDamagePanel.HidePanel);
            _IelementsDamagePanel.WaterElementInfoLine.InfoInLine.ButtonAttack.onClick.AddListener(_IelementsDamagePanel.HidePanel);
            #endregion
        }

        private void OnDisable()
        {
            _fight.Dispose();

            #region Remove subscrib event on click panel InfoInLine 
            _IelementsDamagePanel.FireElementInfoLine.InfoInLine.ButtonAttack.onClick.RemoveListener(_IelementsDamagePanel.HidePanel);
            _IelementsDamagePanel.MetalElementInfoLine.InfoInLine.ButtonAttack.onClick.RemoveListener(_IelementsDamagePanel.HidePanel);
            _IelementsDamagePanel.StoneElementInfoLine.InfoInLine.ButtonAttack.onClick.RemoveListener(_IelementsDamagePanel.HidePanel);
            _IelementsDamagePanel.TreeElementInfoLine.InfoInLine.ButtonAttack.onClick.RemoveListener(_IelementsDamagePanel.HidePanel);
            _IelementsDamagePanel.WaterElementInfoLine.InfoInLine.ButtonAttack.onClick.RemoveListener(_IelementsDamagePanel.HidePanel);
            #endregion
        }

        private void Update()
        {
            if (_spawnPoint != null) 
                ShowEnemyUI(_spawnPoint);

            _playerAttackView.transform.position = GetScreenCoordinates(_playerRectTransform).center;
        }


        public void Set(IPlayerPresenter playerPresenter, IEnemyPresenter enemyPresenter)
        {
            FillElementsDamagePanel(playerPresenter);

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
                GetDicePresenterAdapter(), _IelementsDamagePanel);

            _fight.Start();

            #endregion
        }

        private void FillElementsDamagePanel(IPlayerPresenter playerPresenter)
        {
            IWeapon weapon = playerPresenter.Player.Weapon;

            IElementsDamagePanel panel = _IelementsDamagePanel;
            
            panel.FireElementInfoLine.InfoInLine.Damage.text = weapon.Damage.ToString();
            panel.FireElementInfoLine.InfoInLine.ChanceCriticalDamage.text = weapon.MinValueToCriticalDamage.ToString();
            panel.FireElementInfoLine.InfoInLine.ChanceToSplash.text = weapon.ChanceToSplash.ToString();
            panel.FireElementInfoLine.InfoInLine.ValueModifier.text = weapon.ValueModifier.ToString();
            
            panel.MetalElementInfoLine.InfoInLine.Damage.text = weapon.Damage.ToString();
            panel.MetalElementInfoLine.InfoInLine.ChanceCriticalDamage.text = weapon.MinValueToCriticalDamage.ToString();
            panel.MetalElementInfoLine.InfoInLine.ChanceToSplash.text = weapon.ChanceToSplash.ToString();
            panel.MetalElementInfoLine.InfoInLine.ValueModifier.text = weapon.ValueModifier.ToString();

            panel.StoneElementInfoLine.InfoInLine.Damage.text = weapon.Damage.ToString();
            panel.StoneElementInfoLine.InfoInLine.ChanceCriticalDamage.text = weapon.MinValueToCriticalDamage.ToString();
            panel.StoneElementInfoLine.InfoInLine.ChanceToSplash.text = weapon.ChanceToSplash.ToString();
            panel.StoneElementInfoLine.InfoInLine.ValueModifier.text = weapon.ValueModifier.ToString();

            panel.TreeElementInfoLine.InfoInLine.Damage.text = weapon.Damage.ToString();
            panel.TreeElementInfoLine.InfoInLine.ChanceCriticalDamage.text = weapon.MinValueToCriticalDamage.ToString();
            panel.TreeElementInfoLine.InfoInLine.ChanceToSplash.text = weapon.ChanceToSplash.ToString();
            panel.TreeElementInfoLine.InfoInLine.ValueModifier.text = weapon.ValueModifier.ToString();

            panel.WaterElementInfoLine.InfoInLine.Damage.text = weapon.Damage.ToString();
            panel.WaterElementInfoLine.InfoInLine.ChanceCriticalDamage.text = weapon.MinValueToCriticalDamage.ToString();
            panel.WaterElementInfoLine.InfoInLine.ChanceToSplash.text = weapon.ChanceToSplash.ToString();
            panel.WaterElementInfoLine.InfoInLine.ValueModifier.text = weapon.ValueModifier.ToString();
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