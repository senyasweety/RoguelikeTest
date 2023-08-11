using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.GenerationSystem.LevelMovement
{
    [RequireComponent(typeof(MouseClickTracker))]
    public class AgentMovement : MonoBehaviour
    {
        [SerializeField] private Tilemap _floorTilemap;
        [SerializeField] private CharacterDestinationMarker _marker;

        private MouseClickTracker _mouseClickTracker;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _mouseClickTracker = GetComponent<MouseClickTracker>();

            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        private void OnEnable()
        {
            _mouseClickTracker.MoveClick += SetNewTargetPosition;
        }

        private void OnDisable()
        {
            _mouseClickTracker.MoveClick -= SetNewTargetPosition;
        }

        public void SetFixedMovement(Vector3 position)
        {
            _marker.SetPosition(position);
            SetAgentPosition(position);
        }

        private void SetNewTargetPosition(Vector3 position)
        {
            Vector3Int coordinate = _floorTilemap.WorldToCell(position);
            var tile = _floorTilemap.GetTile(coordinate);

            if (tile != null)
            {
                _marker.SetPosition(position);
                SetAgentPosition(position);
            }
        }

        private void SetAgentPosition(Vector3 position)
        {
            _agent.SetDestination(new Vector3(position.x, position.y, transform.position.z));
        }
    }
}