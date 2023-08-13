using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Fight.Dice;
using Assets.Interface;
using Assets.Person;
using UnityEngine;
using AnimationClip = Assets.Scripts.AnimationComponent.AnimationClip;
using AnimationState = Assets.Scripts.AnimationComponent.AnimationState;
using Random = UnityEngine.Random;

namespace Assets.Fight
{
    public class Fight : IDisposable
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly List<UnitAttackPresenter> _enemyAttackPresenters;
        private readonly UnitAttackPresenter _playerAttackPresenter;
        private readonly IStepFightView _stepFightView;
        private readonly DicePresenterAdapter _dicePresenterAdapter;
        private readonly Queue<UnitAttackPresenter> _unitsOfQueue;
        private int _countSteps = 10;
        private Coroutine _coroutine;
        private Coroutine _animationCoroutine;
        private bool _isCompleteAnimation;
        private bool _pressed = false;

        public Fight(ICoroutineRunner coroutineRunner, List<UnitAttackPresenter> enemyAttackPresenters,
            UnitAttackPresenter playerAttackPresenter, IStepFightView stepFightView,
            DicePresenterAdapter dicePresenterAdapter)
        {
            _coroutineRunner = coroutineRunner;
            _enemyAttackPresenters = enemyAttackPresenters;
            _playerAttackPresenter = playerAttackPresenter;
            _stepFightView = stepFightView;
            _dicePresenterAdapter = dicePresenterAdapter;
            _unitsOfQueue = new Queue<UnitAttackPresenter>();

            SubscribeOnDieEnemies();

            GenerateAttackingSteps(enemyAttackPresenters, playerAttackPresenter);
        }

        public void Dispose() =>
            _dicePresenterAdapter.Dispose();

        public void Start()
        {
            if (_coroutine != null)
                _coroutineRunner.StopCoroutine(_coroutine);

            _coroutine = _coroutineRunner.StartCoroutine(AnimateAttackCoroutine());
        }

        private IEnumerator AnimateAttackCoroutine()
        {
            WaitUntil waitUntil = new WaitUntil(_dicePresenterAdapter.CheckOnDicesShuffeled);

            _playerAttackPresenter.ShowAnimation(AnimationState.Idle);

            foreach (UnitAttackPresenter enemyAttackPresenter in _enemyAttackPresenters)
                enemyAttackPresenter.ShowAnimation(AnimationState.Idle);

            while (_enemyAttackPresenters.All(x => x.Unit.IsDie == false) && _playerAttackPresenter.Unit.IsDie == false)
            {
                if (_unitsOfQueue.Count <= 0)
                    GenerateAttackingSteps(_enemyAttackPresenters, _playerAttackPresenter);

                UnitAttackPresenter unitAttackPresenter = _unitsOfQueue.Dequeue();

                if (unitAttackPresenter.Unit is Player.Player player)
                {
                    _dicePresenterAdapter.SetActive();
                    yield return waitUntil;
                    _dicePresenterAdapter.RestartShuffelValue();
                    
                    Debug.Log("Ходит игрок");
                    // Получить данные с первого кубика
                    Debug.Log("данные с первого кубика " + _dicePresenterAdapter.LeftDiceValue);
                    // Получить данные со второго кубика
                    Debug.Log("данные с второго кубика " + _dicePresenterAdapter.CenterDiceValue);
                    // Получить данные с третьего кубика
                    Debug.Log("данные с третьего кубика " + _dicePresenterAdapter.RightDiceValue);


                    // yield return _coroutineRunner.StartCoroutine(
                    //     StartSingleAnimationCoroutine(AnimationState.Attack, _playerAttackPresenter));
                    //
                    // _playerAttackPresenter.ShowAnimation(AnimationState.Idle);
                    //
                    // yield return _coroutineRunner.StartCoroutine(
                    //     StartSingleAnimationCoroutine(AnimationState.Attack, _enemyAttackPresenters[0]));
                    //
                    // _playerAttackPresenter.ShowAnimation(AnimationState.Idle);
                    //
                    // yield return _coroutineRunner.StartCoroutine(
                    //     StartSingleAnimationCoroutine(AnimationState.Attack, _playerAttackPresenter));
                    //
                    // _playerAttackPresenter.ShowAnimation(AnimationState.Idle);

                    // Нанести урон одному врагу || Нанести урон нескольким врагам

                    // enemy.TakeDamage(player.Weapon.DamageData);
                }
                else if (unitAttackPresenter.Unit is Enemy.Enemy enemy)
                {
                    Debug.Log("Ходит враг");
                    _dicePresenterAdapter.SetDisactive();
                    //_player.TakeDamage(enemy.Weapon.DamageData);
                }
            }
        }

        private IEnumerator StartSingleAnimationCoroutine(AnimationState animationState,
            UnitAttackPresenter attackPresenter)
        {
            Debug.Log("Старт StartAnimationCoroutine");
            bool isComplete = false;

            attackPresenter.ShowAnimation(animationState);

            attackPresenter.UnitAttackView.OnAnimationComplete += () => isComplete = true;

            while (isComplete == false)
                yield return null;

            Debug.Log("Конец StartAnimationCoroutine");
        }

        private IEnumerator StartMultipleAnimationCoroutine(AnimationState animationState,
            List<UnitAttackPresenter> attackPresenters)
        {
            attackPresenters.ForEach(unitAttackPresenter => unitAttackPresenter.ShowAnimation(animationState));

            bool IsCompleteAllAnimations =
                attackPresenters.All(unitAttackPresenter => unitAttackPresenter.UnitAttackView.IsComplete);

            while (IsCompleteAllAnimations == false)
                yield return null;
        }

        private void GenerateAttackingSteps(List<UnitAttackPresenter> enemyAttackPresenters,
            UnitAttackPresenter playerAttackPresenter)
        {
            List<UnitAttackPresenter> persons = new List<UnitAttackPresenter>();

            foreach (UnitAttackPresenter enemyAttackPresenter in enemyAttackPresenters)
                persons.Add(enemyAttackPresenter);

            persons.Add(playerAttackPresenter);

            for (int i = 0; i < _countSteps; i++)
            {
                UnitAttackPresenter unitAttackPresenter = persons[Random.Range(0, persons.Count)];
                _stepFightView.SetSprite(unitAttackPresenter.Unit.Sprite, i);
                _unitsOfQueue.Enqueue(unitAttackPresenter);
            }
        }

        private void SubscribeOnDieEnemies()
        {
            foreach (UnitAttackPresenter unitAttackPresenter in _enemyAttackPresenters)
                unitAttackPresenter.Unit.Died += ActionAfterDie;
        }

        private void ActionAfterDie(Unit unit) =>
            Debug.Log("Я вмЭр");
    }
}