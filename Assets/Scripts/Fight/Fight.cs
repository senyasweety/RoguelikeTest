using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Fight.Dice;
using Assets.Interface;
using Assets.Person;
using UnityEngine;
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

                    Debug.Log("Ходит игрок жизни врагов = ");
                    int i = 1;
                    foreach (UnitAttackPresenter unit in _enemyAttackPresenters)
                        Debug.Log($"{i} = {unit.Unit.Healh}");

                    bool isSplashAttack = _dicePresenterAdapter.LeftDiceValue == player.Weapon.ChanceToSplash;

                    yield return _coroutineRunner.StartCoroutine(
                        StartSingleAnimationCoroutine(AnimationState.Attack, _playerAttackPresenter));
                    _playerAttackPresenter.ShowAnimation(AnimationState.Idle);

                    List<UnitAttackPresenter> allLiveEnemy =
                        _enemyAttackPresenters.Where(x => x.Unit.IsDie == false).ToList();

                    if (isSplashAttack)
                    {
                        yield return _coroutineRunner.StartCoroutine(
                            StartMultipleAnimationCoroutine(AnimationState.Hit, allLiveEnemy));

                        foreach (UnitAttackPresenter enemy in allLiveEnemy)
                        {
                            enemy.Unit.TakeDamage(player.Weapon);

                            if (enemy.Unit.IsDie)
                                enemy.ShowAnimation(AnimationState.Dei);
                            else
                                enemy.ShowAnimation(AnimationState.Idle);
                        }
                    }
                    else
                    {
                        UnitAttackPresenter randomEnemy =
                            _enemyAttackPresenters[Random.Range(0, allLiveEnemy.Count - 1)];

                        yield return _coroutineRunner.StartCoroutine(
                            StartSingleAnimationCoroutine(AnimationState.Hit, randomEnemy));

                        randomEnemy.Unit.TakeDamage(player.Weapon);

                        if (randomEnemy.Unit.IsDie)
                            randomEnemy.ShowAnimation(AnimationState.Dei);
                        else
                            randomEnemy.ShowAnimation(AnimationState.Idle);
                    }


                    Debug.Log("Игрок походил жизни врагов = ");
                    int j = 1;
                    foreach (UnitAttackPresenter unit in _enemyAttackPresenters)
                        Debug.Log($"{j} = {unit.Unit.Healh}");
                }
                else if (unitAttackPresenter.Unit is Enemy.Enemy enemy)
                {
                    Debug.Log($"Ходит враг жизни игрока = {_playerAttackPresenter.Unit.Healh}");
                    _dicePresenterAdapter.SetDisactive();

                    yield return _coroutineRunner.StartCoroutine(
                        StartSingleAnimationCoroutine(AnimationState.Attack, unitAttackPresenter));
                    unitAttackPresenter.ShowAnimation(AnimationState.Idle);

                    _playerAttackPresenter.Unit.TakeDamage(enemy.Weapon);

                    yield return _coroutineRunner.StartCoroutine(
                        StartSingleAnimationCoroutine(AnimationState.Hit, _playerAttackPresenter));

                    if (_playerAttackPresenter.Unit.IsDie)
                        _playerAttackPresenter.ShowAnimation(AnimationState.Dei);
                    else
                        _playerAttackPresenter.ShowAnimation(AnimationState.Idle);

                    Debug.Log($"Враг походил жизни игрока = {_playerAttackPresenter.Unit.Healh}");
                }
            }
        }

        private IEnumerator StartSingleAnimationCoroutine(AnimationState animationState,
            UnitAttackPresenter attackPresenter)
        {
            Debug.Log("Старт StartAnimationCoroutine");

            attackPresenter.ShowAnimation(animationState);

            yield return new WaitUntil(() => attackPresenter.UnitAttackView.IsComplete);
            Debug.Log("Конец StartAnimationCoroutine");
        }

        private IEnumerator StartMultipleAnimationCoroutine(AnimationState animationState,
            List<UnitAttackPresenter> attackPresenters)
        {
            Debug.Log("Старт StartMultipleAnimationCoroutine");

            attackPresenters.ForEach(unitAttackPresenter => unitAttackPresenter.ShowAnimation(animationState));

            yield return new WaitUntil(
                () => attackPresenters.All(unitAttackPresenter => unitAttackPresenter.UnitAttackView.IsComplete)
            );

            Debug.Log("Конец StartMultipleAnimationCoroutine");
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