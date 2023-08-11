using Assets.Scripts.GenerationSystem.LevelMovement;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjectSystem.CanvasInfoSystem
{
    public class UserResponseHandler : MonoBehaviour
    {
        [SerializeField] private MessagePanel _infoPanel;
        [SerializeField] private MouseClickTracker _clickTracker;
        [SerializeField] private InteractiveObjectHandler interactiveObjectHandler;

        private Vector3 _targetPosition;
        private InteractiveObject _selectedObject;

        private void Start()
        {
            _clickTracker.ObjectClick += ShowPanel;
        }

        private void OnDestroy()
        {
            _clickTracker.ObjectClick -= ShowPanel;
            _infoPanel.UserResponse -= MoveFixedAgent;
        }

        private void ShowPanel(InteractiveObject selectedObject, Vector3 position)
        {
            _selectedObject = selectedObject;
            _targetPosition = position;

            // TODO selectedObject.GetData() исправить
            // Первый вариант  _infoPanel.ShowPanel(selectedObject.Type, selectedObject.GetData());
            _infoPanel.ShowPanel(_selectedObject);
            _infoPanel.UserResponse += MoveFixedAgent;
        }

        private void MoveFixedAgent(bool isPositiveResponse)
        {
            _infoPanel.UserResponse -= MoveFixedAgent;
            
            if (isPositiveResponse)
                interactiveObjectHandler.ProduceInteraction(_selectedObject, _targetPosition);           
        }
    }
}