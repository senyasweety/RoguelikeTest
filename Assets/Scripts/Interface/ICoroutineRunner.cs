using System.Collections;
using UnityEngine;

namespace Assets.Interface
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator enumerator);

        void StopCoroutine(Coroutine coroutine);
    }
}