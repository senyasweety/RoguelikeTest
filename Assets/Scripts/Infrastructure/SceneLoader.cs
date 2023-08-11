using System;
using System.Collections;
using Assets.Interface;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner  coroutineRunner) => 
            _coroutineRunner = coroutineRunner;
        
        public  void LoadScene(string sceneName, Action onLoadedCallback = null)
        {
             LoadSceneCoroutine(sceneName, onLoadedCallback).ToUniTask();
             //_coroutineRunner.StartCoroutine(LoadSceneCoroutine(sceneName, onLoadedCallback));
             
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, Action onLoadedCallback = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                onLoadedCallback?.Invoke();
                yield break;
            }
            
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            
            while (asyncOperation.isDone == false)
                yield return null;
            
            onLoadedCallback?.Invoke();
        }
    }
}