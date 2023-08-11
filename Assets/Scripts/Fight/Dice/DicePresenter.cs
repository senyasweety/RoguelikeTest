using System;
using System.Collections;
using Assets.Interface;
using UnityEngine;

namespace Assets.Fight.Dice
{
    public class DicePresenter : IDisposable
    {
        private readonly DiceView _diceView;
        private readonly DiceModel _diceModel;
        private readonly ICoroutineRunner _coroutineRunner;

        private Coroutine _coroutine;
        private int _countOfNeedClick = 2;

        public DicePresenter(DiceView diceView, DiceModel diceModel, ICoroutineRunner coroutineRunner)
        {
            _diceView = diceView;
            _diceModel = diceModel;
            _coroutineRunner = coroutineRunner;
            WasShuffeled = false;
            _diceView.Shuffled += StartAnimation;
            CurrentNumberDice = 0;
        }

        public int CurrentNumberDice { get; private set; }
        public bool WasShuffeled { get; set; }

        public void Dispose() =>
            _diceView.Shuffled -= StartAnimation;

        public void SetActive() =>
            _diceView.SetActive();

        public void SetDisactive() =>
            _diceView.SetDisactive();

        private void StartAnimation()
        {
            _countOfNeedClick--;

            if (_countOfNeedClick <= 0)
            {
                _countOfNeedClick = 2;
                WasShuffeled = true;

                if (_coroutine != null)
                {
                    _coroutineRunner.StopCoroutine(_coroutine);
                    _coroutine = null;
                }
                string value = _diceView.CurrentDiceSide[_diceView.CurrentDiceSide.Length - 1].ToString();
                CurrentNumberDice = int.Parse(value) + 1;
                SetDisactive();
                return;
            }

            if (_coroutine != null)
            {
                _coroutineRunner.StopCoroutine(_coroutine);
                _coroutine = null;
                return;
            }

            _coroutine = _coroutineRunner.StartCoroutine(StartAnimationCoroutine());
        }

        private IEnumerator StartAnimationCoroutine()
        {
            int step = 1000;

            while (step > 0)
            {
                // TODO Add Shuffle method for List
                foreach (Sprite sprite in _diceModel.Sprites)
                {

                    _diceView.SetSprite(sprite);
                    step--;
                    yield return null;
                }
            }
        }
    }
}