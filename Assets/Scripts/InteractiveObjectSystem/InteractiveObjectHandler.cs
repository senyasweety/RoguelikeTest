using System;
using System.Collections;
using Assets.Fight;
using Assets.Loot;
using Assets.Player;
using Assets.Scripts.GenerationSystem.LevelMovement;
using Assets.Scripts.InteractiveObjectSystem.RandomEventSystem;
using Assets.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.InteractiveObjectSystem
{
    public class InteractiveObjectHandler : MonoBehaviour
    {
        [SerializeField] private MouseClickTracker _clickTracker;
        [SerializeField] private AgentMovement _agent;
        [SerializeField] private float _minDistanceToStartBattle = 10.1f;
        [SerializeField] private Button _closeButton;
        [Space] [SerializeField] private UIFight _battlefild;
        [SerializeField] private RandomLootView _lootPanel;

        private InteractiveObject _targetObject;
        private float _distance;

        private void Awake()
        {
            _closeButton.onClick.AddListener(CloseBattlefild);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(CloseBattlefild);
        }

        public void ProduceInteraction(InteractiveObject targetObject, Vector3 targetPosition)
        {
            _targetObject = targetObject;
            _clickTracker.enabled = false;

            Action openPanel = () => { };

            Player.Player player = FindObjectOfType<PlayerPresenter>().Player;
            
            if (targetObject.TryGetComponent(out IEnemyObjectData enemyObject))
                openPanel = () => { Curtain.Instance.ShowAnimation(() => { _battlefild.SetActiveFightPlace(player, enemyObject.Enemy.ToArray()); }); };
            else if (targetObject.TryGetComponent(out InteractiveLootObject lootObject))
                openPanel = () => { _lootPanel.ShowPanel(this); };
            else if (targetObject.TryGetComponent(out InteractiveRandomEventObject randomEventObject))
                openPanel = CreateRandomEvent();

            #region Old Switch

            // switch (targetObject.Type)
            // {
            //     case ObjectType.Enemy:
            //         openPanel = () => { Curtain.Instance.ShowAnimation(() => { _battlefild.gameObject.SetActive(true); }); };
            //         break;
            //     case ObjectType.RandomEvent:
            //         openPanel = CreateRandomEvent();
            //         break;
            //     case ObjectType.Loot:
            //         openPanel = () => { _lootPanel.ShowPanel(this); };
            //         break;
            //     case ObjectType.Boos:
            //         openPanel = () => { Curtain.Instance.ShowAnimation(() => { _battlefild.gameObject.SetActive(true); }); };
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }

            #endregion

            StartCoroutine(GoToTarget(targetPosition, openPanel));
        }

        public void ReturnToGlobalMap()
        {
            _clickTracker.enabled = true;
            _targetObject.DestroyObject();
        }

        // TODO временное решешие. Сам скрипт UIFight должен принимать InteractiveObjectHandler и вызывать метод ReturnToGlobalMap
        private void CloseBattlefild()
        {
            Curtain.Instance.ShowAnimation(() => { _battlefild.gameObject.SetActive(false); });
            ReturnToGlobalMap();
        }

        private Action CreateRandomEvent()
        {
            var levelRandomEvent = new LevelRandomEvent();
            var randomEvent = levelRandomEvent.GetRandomEvent();

            switch (randomEvent)
            {
                case RandomEventType.Enemy:
                    return () => { Curtain.Instance.ShowAnimation(() => { _battlefild.gameObject.SetActive(true); }); };
                case RandomEventType.Loot:
                    return () => { _lootPanel.ShowPanel(this); };
                case RandomEventType.AD:
                    return () => { _lootPanel.ShowPanel(this); };
                default:
                    return () => { _battlefild.gameObject.SetActive(true); };
            }
        }

        private IEnumerator GoToTarget(Vector3 targetPosition, Action action)
        {
            _agent.SetFixedMovement(targetPosition);

            _distance = Vector3.Distance(_agent.transform.position, targetPosition);

            while (_distance > _minDistanceToStartBattle)
            {
                yield return null;
                _distance = Vector3.Distance(_agent.transform.position, targetPosition);
            }

            action();
        }
    }
}