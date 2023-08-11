using Agava.YandexGames;
using Assets.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SdkLoader _sdkLoader;
        private readonly SceneLoader _sceneLoader;

        private const string EnglishLanguage = "en";
        private const string RussianLanguage = "ru";
        private const string TurkishLanguage = "tr";

        public BootstrapState(GameStateMachine gameStateMachine, SdkLoader sdkLoader, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sdkLoader = sdkLoader;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _sdkLoader.LoadSdk(onSuccessCallback: SetStartLanguage);
#endif
            _sceneLoader.LoadScene("Menu");
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void SetStartLanguage()
        {
            switch (YandexGamesSdk.Environment.i18n.lang)
            {
                case EnglishLanguage:
                    Debug.Log(EnglishLanguage);
                    break;
                case RussianLanguage:
                    Debug.Log(RussianLanguage);
                    break;
                case TurkishLanguage:
                    Debug.Log(TurkishLanguage);
                    break;
                default:
                    Debug.Log("");
                    break;
            }
            
            _sceneLoader.LoadScene("Menu");
            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
            Debug.Log("BootstrapState exited");
        }
    }
}