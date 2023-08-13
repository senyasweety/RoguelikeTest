using System;
using UnityEngine;
using AnimationClip = Assets.Scripts.AnimationComponent.AnimationClip;

namespace Assets.Person
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class UnitAttackView : MonoBehaviour
    {
        [SerializeField] [Range(1, 30)] private int _frameRate = 10;

        public event Action OnAnimationComplete;
        private SpriteRenderer _renderer;
        private float _secPerFrame;
        private float _nextFrameTime;
        private int _currentFrame;
        private int _currentClip;
        private bool _isPlaying = true;
        
        private Sprite[] _sprites;
        private bool _isLoop;


        private void Awake() =>
            _renderer = GetComponent<SpriteRenderer>();

        private void OnEnable()
        {
            _secPerFrame = 1f / _frameRate;

            StartAnimation();
            _nextFrameTime = Time.time + _secPerFrame;
        }

        
        
        // private void OnBecameVisible()
        // {
        //     enabled = _isPlaying;
        // }
        //
        // private void OnBecameInvisible()
        // {
        //     enabled = false;
        // }

        private void Update()
        {
            if (_sprites == null)
                return;
            
            if (_nextFrameTime > Time.time)
                return;

            if (_currentFrame >= _sprites.Length)
            {
                if (_isLoop)
                    _currentFrame = 0;
                else
                {
                    OnAnimationComplete?.Invoke();
                    //enabled = false;
                    _isPlaying = false;
                }
            }
            else
            {
                _renderer.sprite = _sprites[_currentFrame];

                _nextFrameTime += _secPerFrame;
                _currentFrame++;
            }
        }

        #region SetClip

        
        // public void SetClip(AnimationState state)
        // {
        //     // for (var i = 0; i < _clips.Length; i++)
        //     // {
        //     //     if (_clips[i].State == state)
        //     //     {
        //     //         _currentClip = i;
        //     //         StartAnimation();
        //     //         return;
        //     //     }
        //     // }
        //
        //     enabled = false;
        //     _isPlaying = false;
        // }

        #endregion
        
        private void StartAnimation()
        {
            enabled = true;
            _isPlaying = true;
            _nextFrameTime = Time.time + _secPerFrame;
            _currentFrame = 0;
        }

        public void SetClip(AnimationClip clip)
        {
            _sprites = clip.Sprites;
            _isLoop = clip.IsLoop;
        }
    }
}