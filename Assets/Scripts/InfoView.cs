using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public abstract class InfoView : MonoBehaviour
    {
        [SerializeField] private Button _buttonYes;
        [SerializeField] private Button _buttonNo;
        
        public event Action<bool> UserResponse;

        private void Awake()
        {
            _buttonYes.onClick.AddListener(CallPositiveResponse);
            _buttonNo.onClick.AddListener(CallNegativeResponse);
        }
        
        private void OnDestroy()
        {
            _buttonYes.onClick.RemoveListener(CallPositiveResponse);
            _buttonNo.onClick.RemoveListener(CallNegativeResponse);
        }

        protected virtual void CallPositiveResponse()
        {
            UserResponse?.Invoke(true);
            gameObject.SetActive(false);
        }

        protected virtual void CallNegativeResponse()
        {
            UserResponse?.Invoke(false);
            gameObject.SetActive(false);
        }
        
        [Serializable]
        protected class AttackAndDefendView
        {
            [SerializeField] private Image _attactElement;
            [SerializeField] private Image _attactIcon;
            [SerializeField] private Image _defendElement;
            [SerializeField] private Image _defendIcon;
            
            public Image AttactElement => _attactElement;
            public Image AttactIcon => _attactIcon;
            public Image DefendElement => _defendElement;
            public Image DefendIcon => _defendIcon;

            public void Show()
            {
                _attactElement.gameObject.SetActive(true);
                _attactIcon.gameObject.SetActive(true);
                _defendElement.gameObject.SetActive(true);
                _defendIcon.gameObject.SetActive(true);
            }

            public void Hide()
            {
                _attactElement.gameObject.SetActive(false);
                _attactIcon.gameObject.SetActive(false);
                _defendElement.gameObject.SetActive(false);
                _defendIcon.gameObject.SetActive(false);
            }
        }
    }
}