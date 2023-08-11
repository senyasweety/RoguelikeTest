using Assets.Observces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
    [RequireComponent(typeof(Button))]
    public class ButtonToPlay : MonoBehaviour
    {
        private Button _button;

        private void Awake() =>
            _button = GetComponent<Button>();

        private void OnEnable() =>
            _button.onClick.AddListener(NextScene);

        private void NextScene()
        {
            SceneManager.LoadScene("LevelGeneration");
            //MainMenuButtonPlayObserver.Instance.Notify();
        }

        private void OnDisable() =>
            _button.onClick.RemoveListener(NextScene);

    }
}