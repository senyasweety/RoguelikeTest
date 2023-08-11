using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Fight.Dice
{
    public class DiceView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _diceImage;

        public event Action Shuffled;
        public string CurrentDiceSide => _diceImage.sprite.name;
        public TMP_Text Text => _text;
        
        private void OnEnable() =>
            _button.onClick.AddListener(Shuffle);

        private void OnDisable() =>
            _button.onClick.RemoveListener(Shuffle);

        public void SetSprite(Sprite sprite) =>
            _diceImage.sprite = sprite;

        public void SetActive()
        {
            _button.interactable = true;
            _diceImage.color = new Color(255, 255, 255, 255);
        }

        public void SetDisactive()
        {
            _button.interactable = false;
            _diceImage.color = new Color(255, 255, 255, 100);
        }
        
        private void Shuffle() =>
            Shuffled?.Invoke();
    }
}