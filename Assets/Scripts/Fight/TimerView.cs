using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Fight
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private int _maxTimeInSeconds = 60;

        private Coroutine _coroutine;
        
        public void StartTimer()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            StartCoroutine(StartTimerCoroutine());
        }

        public void StopTimer()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
        
        private IEnumerator StartTimerCoroutine()
        {
            WaitForSeconds seconds = new WaitForSeconds(1);

            for (int i = 0; i < _maxTimeInSeconds; i++)
            {
                _text.text = _maxTimeInSeconds.ToString();
                yield return seconds;
            }
        }
    }
}