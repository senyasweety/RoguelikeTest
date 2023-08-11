using UnityEngine;

namespace Assets.Scripts.GenerationSystem.LevelMovement
{
    public class CharacterDestinationMarker : MonoBehaviour
    {
        [SerializeField] private Transform _characterTarget;
        [SerializeField] float _hidingDistance = 1.5f;
        [SerializeField] float _blackAtDistance = 1f;
        [SerializeField] private SpriteRenderer _renderer;

        public void SetPosition(Vector3 position)
        {
            transform.position = new Vector3(position.x, position.y, 0);
        }

        private void Update()
        {
            var distance = (_characterTarget.position - transform.position).magnitude;
            distance -= _blackAtDistance;
            
            var percentage = distance / _hidingDistance;
            percentage = Mathf.Clamp(percentage, 0, 1);

            _renderer.material.color = new Color(1, 1, 1, percentage);
        }
    }
}