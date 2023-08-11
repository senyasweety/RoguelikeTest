using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SoundSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundComponent : MonoBehaviour
    {
        [SerializeField] private SoundType _soundType;
        [SerializeField] private AudioClip _clip;
        [SerializeField] [Range(0, 1)] private float _volume;
        [SerializeField] [Min(0f)] private float _duration;
        [Space(10f)]
        [SerializeField] private bool _isLoop;
        [SerializeField] private bool _isRestartIfCalledAgain;
        [Space(10f)]
        [SerializeField] private bool _smoothFade;
        [SerializeField] private float _timeToFade;

        private AudioSource _source;
        private Coroutine _coroutine;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.clip = _clip;
            _source.loop = false;
            _source.playOnAwake = false;
        }

        public void Play()
        {
            if (_source.isPlaying && _isRestartIfCalledAgain == false)
                return;

            if (_soundType == SoundType.Music && GameRoot.Instance.Sound.IsMusicOn)
                PlaySound();
            else if (_soundType == SoundType.SFX && GameRoot.Instance.Sound.IsSfxOn)
                PlaySound();
        }

        private void StartState(IEnumerator coroutine)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(coroutine);
        }

        private void PlaySound()
        {
            _source.volume = _volume;
            _source.Play();

            if (_duration == 0)
                StartState(WaitPlaySound(_source.clip.length));
            else
                StartState(WaitPlaySound(_duration));
        }

        private IEnumerator WaitPlaySound(float timeToStop)
        {
            while (_source.time < timeToStop)
            {
                yield return null;
            }

            StartState(FadeSound());
        }

        private IEnumerator FadeSound()
        {
            if (_smoothFade == false)
            {
                RestartPlaySound();
                yield break;
            }

            float timeElapsed = 0;

            while (timeElapsed < _timeToFade)
            {
                _source.volume = Mathf.Lerp(_volume, 0, timeElapsed / _timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            
            RestartPlaySound();
        }

        private void RestartPlaySound()
        {
            _source.Stop();
            
            if (_isLoop)
                PlaySound();
        }
    }
}