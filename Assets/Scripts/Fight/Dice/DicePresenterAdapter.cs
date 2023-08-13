using System;

namespace Assets.Fight.Dice
{
    public class DicePresenterAdapter : IDisposable
    {
        private readonly DicePresenter _leftDice;
        private readonly DicePresenter _centerDice;
        private readonly DicePresenter _rightDice;

        public DicePresenterAdapter(DicePresenter leftDice, DicePresenter centerDice, DicePresenter rightDice)
        {
            _leftDice = leftDice;
            _centerDice = centerDice;
            _rightDice = rightDice;
        }

        public int LeftDiceValue => _leftDice.CurrentNumberDice;
        public int CenterDiceValue => _centerDice.CurrentNumberDice;
        public int RightDiceValue => _rightDice.CurrentNumberDice;
        
        public void SetDisactive()
        {
            _leftDice.SetDisactive();
            _centerDice.SetDisactive();
            _rightDice.SetDisactive();
        }

        public void SetActive()
        {
            _leftDice.SetActive();
            _centerDice.SetActive();
            _rightDice.SetActive();
        }

        public void Dispose()
        {
            _leftDice?.Dispose();
            _centerDice?.Dispose();
            _rightDice?.Dispose();
        }

        public void RestartShuffelValue()
        {
            _leftDice.WasShuffeled = false;
            _centerDice.WasShuffeled = false;
            _rightDice.WasShuffeled = false;
        }
        
        public bool CheckOnDicesShuffeled() =>
            _leftDice.WasShuffeled && _centerDice.WasShuffeled && _rightDice.WasShuffeled;
    }
}