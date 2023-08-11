using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Widgets
{
    public class CustomButton : Button
    {
        [SerializeField] private GameObject _normal;
        [SerializeField] private GameObject _highlighted;
        [SerializeField] private GameObject _pressed;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            DisableStates();
            
            if (state == SelectionState.Highlighted)
                _highlighted.SetActive(true);
            else if (state == SelectionState.Pressed)
                _pressed.SetActive(true);
            else
                _normal.SetActive(true);
        }

        private void DisableStates()
        {
            _normal.SetActive(false);
            _pressed.SetActive(false);
            _highlighted.SetActive(false);
        }
    }
}