using Assets.Scripts.AnimationComponent;
using UnityEngine;
using AnimationState = Assets.Scripts.AnimationComponent.AnimationState;

namespace Assets.Scripts.GenerationSystem.LevelMovement
{
    public class PlayerMovementAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteAnimation _animation;

        private readonly float _minDistance = 0.001f;
        
        private Vector3 _lastTranformPosition;
        private bool _isAnimationPlaying = false;

        private void Start()
        {
            _lastTranformPosition = transform.position;
        }

        private void LateUpdate()
        {
            UpdateSpriteRender();
            CheckMovement();
            _lastTranformPosition = transform.position;
        }

        private void UpdateSpriteRender()
        {
            if (_lastTranformPosition.x - transform.position.x < 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (_lastTranformPosition.x - transform.position.x > _minDistance)
            {
                _spriteRenderer.flipX = true;
            }
        }

        private void CheckMovement()
        {
            var distance = Vector3.Distance(transform.position, _lastTranformPosition);

            if(distance > _minDistance)
            {
                if (_isAnimationPlaying == false)
                {
                    _isAnimationPlaying = true;
                    _animation.SetClip(AnimationState.Walk);
                }   
            }
            else
            {
                if (_isAnimationPlaying)
                {
                    _isAnimationPlaying = false;
                    _animation.SetClip(AnimationState.Idle);
                }
            }
        }
    }
}