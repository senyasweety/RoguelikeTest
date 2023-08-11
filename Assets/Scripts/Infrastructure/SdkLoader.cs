using System;
using System.Collections;
using Agava.YandexGames;
using Assets.Interface;
using Cysharp.Threading.Tasks;

namespace Assets.Infrastructure
{
    public class SdkLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SdkLoader(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void LoadSdk(Action onSuccessCallback = null) => 
            _coroutineRunner.StartCoroutine(LoadSdkCoroutine(onSuccessCallback));

        private IEnumerator LoadSdkCoroutine(Action onSuccessCallback = null)
        {
#if !UNITY_WEBGL && UNITY_EDITOR
             yield break;
#endif
            yield return YandexGamesSdk.Initialize(() => onSuccessCallback?.Invoke());
        }
    }
}