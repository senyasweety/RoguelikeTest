using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.InteractiveObjectSystem;
using Assets.UI;
using UnityEngine;
using UnityEngine.UI;
using AnimationClip = Assets.Scripts.AnimationComponent.AnimationClip;

namespace Assets.Person
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class UnitAttackView : MonoBehaviour
    {
        [SerializeField] private List<HealthBarView> _health;
        [SerializeField] private Image _weaponElement;
        [SerializeField] private Image _armorElement;

        [Space] [SerializeField] private ElementsSpriteView _elementsSpriteView;

        public ElementsSpriteView ElementsSpriteView => _elementsSpriteView;

        #region MyRegion

        private int _frameRate = 10;
        private List<Clip> _clips = new List<Clip>();

        public event Action OnAnimationComplete;
        public bool IsComplete { get; private set; } = false;
        private SpriteRenderer _renderer;
        private float _secPerFrame;
        private float _nextFrameTime;
        private int _currentFrame;
        private bool _isPlaying = true;

        private Sprite[] _sprites;
        private bool _isLoop;
        private Clip _currentClip;
        private List<HealthBarView> _activeHealth;
        private HealthBarView _currentHealthImage;

        public List<HealthBarView> Health => _health;

        public Image WeaponElement => _weaponElement;

        public Image ArmorElement => _armorElement;

        private void Awake() =>
            _renderer = GetComponent<SpriteRenderer>();

        private void OnEnable()
        {
            _secPerFrame = 1f / _frameRate;
            StartAnimation();
            _nextFrameTime = Time.time + _secPerFrame;

            GetActiveHealth();
        }

        private void Update()
        {
            if (_currentClip == null)
                return;

            if (_nextFrameTime > Time.time)
                return;

            if (_currentFrame >= _currentClip.Sprites.Length)
            {
                if (_currentClip.IsLoop)
                {
                    _currentFrame = 0;
                }
                else if (_currentClip.IsAllowNextClip)
                {
                    SetClip(_currentClip.NextState);
                }
                else
                {
                    OnAnimationComplete?.Invoke();
                    IsComplete = true;
                    //_isPlaying = false;
                }
            }
            else
            {
                _renderer.sprite = _currentClip.Sprites[_currentFrame];

                _nextFrameTime += _secPerFrame;
                _currentFrame++;
            }
        }

        public void SetClip(Assets.Scripts.AnimationComponent.AnimationState state)
        {
            for (var i = 0; i < _clips.Count; i++)
            {
                if (_clips[i].State == state)
                {
                    _currentClip = _clips[i];
                    StartAnimation();
                    return;
                }
            }
        }

        private void StartAnimation()
        {
            IsComplete = false;
            enabled = true;
            _isPlaying = true;
            _nextFrameTime = Time.time + _secPerFrame;
            _currentFrame = 0;
        }


        

        #endregion

        public void FillDataForClips(IReadOnlyList<AnimationClip> spriteAnimationAnimationClips)
        {
            foreach (AnimationClip clip in spriteAnimationAnimationClips)
            {
                _clips.Add(new Clip(
                    clip.Sprites,
                    clip.State,
                    clip.IsLoop,
                    clip.NextState,
                    clip.IsAllowNextClip)
                );
            }
        }


        private class Clip
        {
            public Clip(Sprite[] sprites, Assets.Scripts.AnimationComponent.AnimationState state, bool isLoop,
                Assets.Scripts.AnimationComponent.AnimationState nextState,
                bool isAllowNextClip)
            {
                Sprites = sprites;
                State = state;
                IsLoop = isLoop;
                NextState = nextState;
                IsAllowNextClip = isAllowNextClip;
            }

            public Sprite[] Sprites { get; }
            public Assets.Scripts.AnimationComponent.AnimationState State { get; }
            public bool IsLoop { get; }
            public Assets.Scripts.AnimationComponent.AnimationState NextState { get; }
            public bool IsAllowNextClip { get; }
        }

        public void ChangeUIHealthValue(float value)
        {
            GetActiveHealth();
            
            if (_currentHealthImage.HealthBar.fillAmount != 0) 
                _currentHealthImage.HealthBar.fillAmount -= value;
            else
            {
                Debug.Log("_currentHealthImage пустой");
                _currentHealthImage.gameObject.SetActive(false);
            }
            
        }

        private void GetActiveHealth()
        {
            _activeHealth = _health.Where(x => x.gameObject.activeSelf).ToList();
            _currentHealthImage = _activeHealth[_activeHealth.Count - 1];
        }
    }
}