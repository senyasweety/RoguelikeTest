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
        private readonly List<Enemy.Enemy> _enemies;
        private readonly Player.Player _player;
        private readonly IStepFightView _stepFightView;
        private readonly DicePresenterAdapter _dicePresenterAdapter;
        private readonly Queue<Unit> _unitStackOfAttack;
        private int _countSteps = 10;
        private Coroutine _coroutine;
        private Coroutine _animationCoroutine;
        private bool _isCompleteAnimation;

        public Fight(ICoroutineRunner coroutineRunner, List<Enemy.Enemy> enemies, Player.Player player, IStepFightView stepFightView,
            DicePresenterAdapter dicePresenterAdapter)
        {
            _coroutineRunner = coroutineRunner;
            _enemies = enemies;
            _player = player;
            _stepFightView = stepFightView;
            _dicePresenterAdapter = dicePresenterAdapter;
            _unitStackOfAttack = new Queue<Unit>();

            SubscribeOnDieEnemies();

            GenerateAttackingSteps(enemies, player);
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
            WaitForSeconds waitForSeconds = new WaitForSeconds(3);
            WaitUntil waitUntil = new WaitUntil(_dicePresenterAdapter.CheckOnDicesShuffeled);

            while (_enemies.All(x => x.IsDie == false) && _player.IsDie == false)
            {
                if (_unitStackOfAttack.Count <= 0)
                    GenerateAttackingSteps(_enemies, _player);

                Unit unit = _unitStackOfAttack.Dequeue();

                if (unit is Player.Player player)
                {
                    yield return waitUntil;
                    // Получить данные с первого кубика
                    Debug.Log("данные с первого кубика " + _dicePresenterAdapter.LeftDiceValue);
                    // Получить данные со второго кубика
                    Debug.Log("данные с второго кубика " + _dicePresenterAdapter.CenterDiceValue);
                    // Получить данные с третьего кубика
                    Debug.Log("данные с третьего кубика " + _dicePresenterAdapter.RightDiceValue);

                    // Проиграть анимацию атаки игрока

                    yield return _coroutineRunner.StartCoroutine(StartAnimationCoroutine(AnimationState.Attack, player));

                    yield return _coroutineRunner.StartCoroutine(StartAnimationCoroutine(AnimationState.Hit, _enemies.ToArray()));

                    // Нанести урон одному врагу || Нанести урон нескольким врагам
                    Debug.Log("Нанесли урон врагам");

                    Debug.Log("Ходит игрок");

                    // enemy.TakeDamage(player.Weapon.DamageData);
                }
                else if (unit is Enemy.Enemy enemy)
                {
                    _dicePresenterAdapter.SetDisactive();

                    Debug.Log("Ходит враг");
                    //_player.TakeDamage(enemy.Weapon.DamageData);
                }

                yield return waitForSeconds;
            }
        }

        private IEnumerator StartAnimationCoroutine(AnimationState animationState, params Unit[] units)
        {
            bool isComplete = false;
            
            units.FirstOrDefault().SpriteAnimation.OnAnimationComplete += () => isComplete = true;

            foreach (Unit unit in units)
            {
                unit.SpriteAnimation.SetClip(animationState);
            }

            while (isComplete == false)
                yield return null;
        }

        private bool SpriteAnimationOAnimationComplete() =>
            _isCompleteAnimation;

        private void SwitchNextAnimation() =>
            _isCompleteAnimation = true;

        private void GenerateAttackingSteps(List<Enemy.Enemy> enemies, Player.Player player)
        {
            List<Unit> persons = new List<Unit>();
            persons.AddRange(enemies);
            persons.Add(player);

            for (int i = 0; i < _countSteps; i++)
            {
                Unit unit = persons[Random.Range(0, persons.Count)];
                _stepFightView.SetSprite(unit.Sprite, i);
                _unitStackOfAttack.Enqueue(unit);
            }
        }

        private void SubscribeOnDieEnemies()
        {
            foreach (Enemy.Enemy enemy in _enemies)
                enemy.Died += ActionAfterDie;
        }

        private void ActionAfterDie(Unit unit) =>
            Debug.Log("Я вмЭр");
    }
}