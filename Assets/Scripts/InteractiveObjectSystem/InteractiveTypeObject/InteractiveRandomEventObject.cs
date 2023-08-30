using System.Collections.Generic;
using Assets.Enemy;
using Assets.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjectSystem
{
    public class InteractiveRandomEventObject : InteractiveObject
    {
        [SerializeField] private InteractiveLootObject _interactiveLootObject;
        [SerializeField] private List<EnemyView> _enemyView;
        
        private EnemyPresenter _enemyPresenter;
        public InteractiveLootObject InteractiveLootObject => _interactiveLootObject;
        public IEnemyPresenter GetRandomEnemyPresenter() => 
            _enemyPresenter = new EnemyPresenter(_enemyView[Random.Range(0, _enemyView.Count)]);
    }
}