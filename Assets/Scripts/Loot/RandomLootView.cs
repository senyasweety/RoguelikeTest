using Assets.Scripts.InteractiveObjectSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Loot
{
    public class RandomLootView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _lable;
        [SerializeField] private TMP_Text _data;
        [SerializeField] private Image _elementImage;
        [SerializeField] private ElementsSpriteView _spriteView;
        [SerializeField] private Button _closeButton;
        private InteractiveObjectHandler _handler;

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(ClosePanel);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(ClosePanel);
        }

        public void ShowPanel(InteractiveObjectHandler handler)
        {
            _handler = handler;
            
            RandomLootGenerator generator = new RandomLootGenerator();
            InteractiveObjectData data = generator.GetRandomLoot();

            _lable.text = data.Name;
            _data.text = data.Data;
            _elementImage.sprite = _spriteView.GetElementSprite(data.Element);
            
            _panel.SetActive(true);
        }

        private void ClosePanel()
        {
            _panel.SetActive(false);
            _handler.ReturnToGlobalMap();
        }
    }
}