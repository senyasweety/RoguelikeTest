using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Person;
using UnityEngine;

namespace Assets.Scripts.AnimationComponent
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] [Range(1, 30)] private int _frameRate = 10;
        [SerializeField] private AnimationClip[] _clips;
        public event Action OnAnimationComplete;
        private SpriteRenderer _renderer;
        private float _secPerFrame;
        private float _nextFrameTime;
        private int _currentFrame;
        private int _currentClip;
        private bool _isPlaying = true;

        public int FrameRate => _frameRate;
        public IReadOnlyList<AnimationClip> AnimationClips => _clips;
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _secPerFrame = 1f / _frameRate;

            StartAnimation();
        }
        
        private void OnEnable()
        {
            _nextFrameTime = Time.time + _secPerFrame;
        }

        private void OnBecameVisible()
        {
            enabled = _isPlaying;
        }

        private void OnBecameInvisible()
        {
            enabled = false;
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time)
                return;

            AnimationClip clip = _clips[_currentClip];

            if (_currentFrame >= clip.Sprites.Length)
            {
                if (clip.IsLoop)
                {
                    _currentFrame = 0;
                }
                else if (clip.IsAllowNextClip)
                {
                    SetClip(clip.NextState);
                }
                else
                {
                    OnAnimationComplete?.Invoke();
                    enabled = false;
                    _isPlaying = false;
                }
            }
            else
            {
                _renderer.sprite = clip.Sprites[_currentFrame];

                _nextFrameTime += _secPerFrame;
                _currentFrame++;
            }
        }

        public void SetClip(AnimationState state)
        {
            for (var i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].State == state)
                {
                    _currentClip = i;
                    StartAnimation();
                    return;
                }
            }

            enabled = false;
            _isPlaying = false;
        }

        public AnimationClip GetClip(AnimationState state)
        {
            AnimationClip clip = _clips.Where(x => x.State == state).FirstOrDefault();
            
            if (clip == null)
                return null;
            
            return clip;
        }

        private void StartAnimation()
        {
            enabled = true;
            _isPlaying = true;
            _nextFrameTime = Time.time + _secPerFrame;
            _currentFrame = 0;
        }
    }

    [Serializable]
    public class AnimationClip
    {
        [SerializeField] private AnimationState _state;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _isLoop;
        [SerializeField] private bool _isAllowNextClip;
        [SerializeField] private AnimationState _nextState;

        public AnimationState State => _state;
        public Sprite[] Sprites => _sprites;
        public bool IsLoop => _isLoop;
        public bool IsAllowNextClip => _isAllowNextClip;
        public AnimationState NextState => _nextState;
    }
}