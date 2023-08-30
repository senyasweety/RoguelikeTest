using System;
using Assets.Enemy;
using Assets.Loot;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjectSystem.CanvasInfoSystem
{
    public class MessagePanel : MonoBehaviour
    {
        [SerializeField] private EnemyInfoView _enemyInfoPanel;
        [SerializeField] private RandomEventView _randomEventPanel;
        [SerializeField] private LootInfoView _randomLootPanel;
        
        public event Action<bool> UserResponse;
        public event Action<InfoView> UserResponse2;
        
        private void OnEnable()
        {
            _enemyInfoPanel.UserResponse += CallResponse;
            _randomEventPanel.Test += RandomEventPanelOnTest;
        }

        private void RandomEventPanelOnTest(InfoView obj) => 
            UserResponse2?.Invoke(obj);

        private void OnDisable()
        {
            _enemyInfoPanel.UserResponse -= CallResponse;
        }

        public void ShowPanel(InteractiveObject interactiveObject)
        {
            gameObject.SetActive(true);

            if (interactiveObject.TryGetComponent(out EnemyView enemyView))
                _enemyInfoPanel.Show(enemyView.EnemyPresenter);
            else if (interactiveObject.TryGetComponent(out InteractiveLootObject lootObject))
                _randomLootPanel.Show(lootObject);
            else if (interactiveObject.TryGetComponent(out InteractiveRandomEventObject randomEventObject))
                _randomEventPanel.Show(randomEventObject,
                    (interactiveLootObject) => _randomLootPanel.Show(randomEventObject.InteractiveLootObject),
                    (enemyPresenter) => _enemyInfoPanel.Show(randomEventObject.GetRandomEnemyPresenter())
                );
        }

        private void CallResponse(bool answer) =>
            UserResponse.Invoke(answer);
    }
}