using UnityEngine;
using UnityEngine.UI;

namespace Assets.Fight
{
    public class ElementsDamagePanel : MonoBehaviour, IElementsDamagePanel
    {
        [SerializeField] private Button _exit;

        public Button Exit => _exit;

        [SerializeField] ElementInfoLine _fireElementInfoLine;
        [SerializeField] ElementInfoLine _treeElementInfoLine;
        [SerializeField] ElementInfoLine _waterElementInfoLine;
        [SerializeField] ElementInfoLine _metalElementInfoLine;
        [SerializeField] ElementInfoLine _stoneElementInfoLine;


        public IElementInfoLine FireElementInfoLine => _fireElementInfoLine;

        public IElementInfoLine TreeElementInfoLine => _treeElementInfoLine;

        public IElementInfoLine WaterElementInfoLine => _waterElementInfoLine;

        public IElementInfoLine MetalElementInfoLine => _metalElementInfoLine;

        public IElementInfoLine StoneElementInfoLine => _stoneElementInfoLine;

        private void OnEnable()
        {
            _exit.onClick.AddListener(HidePanel);
        }

        private void OnDisable()
        {
            _exit.onClick.RemoveListener(HidePanel);
        }

        public void HidePanel()
        {
            print("OnCLick exit button - hide panel");
            gameObject.SetActive(false);
        }

        public void ShowPanel() =>
            gameObject.SetActive(true);
    }

    public interface IElementsDamagePanel
    {
        void ShowPanel();
        public void HidePanel();
        public Button Exit { get; }
        public IElementInfoLine FireElementInfoLine { get; }

        public IElementInfoLine TreeElementInfoLine { get; }

        public IElementInfoLine WaterElementInfoLine { get; }

        public IElementInfoLine MetalElementInfoLine { get; }

        public IElementInfoLine StoneElementInfoLine { get; }
    }
}