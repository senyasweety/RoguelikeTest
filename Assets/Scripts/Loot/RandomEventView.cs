using System;
using Assets.Enemy;
using Assets.Scripts.InteractiveObjectSystem;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Loot
{
    public class RandomEventView : InfoView
    {
        [SerializeField] private TMP_Text _lable;
        [SerializeField] private TMP_Text _data;

        public event Action<InfoView> Test;
        
        private InteractiveRandomEventObject _randomEvent;
        private Action<InteractiveLootObject> _test212;
        private Action<IEnemyPresenter> _test2;

        public void Show(InteractiveRandomEventObject randomEvent, 
            Action<InteractiveLootObject> test212,
            Action<IEnemyPresenter> test2 )
        {
            _test2 = test2;
            _test212 = test212;
            gameObject.SetActive(true);
            
            
            _randomEvent = randomEvent;
            
            // RandomLootGenerator generator = new RandomLootGenerator();
            // InteractiveObjectData data = generator.GetRandomLoot();
            //
            // _lable.text = data.Name;
            // _data.text = data.Data;
        }
        
        protected override void CallPositiveResponse()
        {
            base.CallPositiveResponse();
            
            if (Random.Range(0, 2) == 0)
            {
                _test212?.Invoke(_randomEvent.InteractiveLootObject);
                //randomEvent.EnemyPresenter ShowLootPanel(randomEventObject.InteractiveLootObject);
                Debug.Log("show loot panel");
            }
            else
            {
                _test2?.Invoke(_randomEvent.GetRandomEnemyPresenter());
                //ShowEnemyPanel(randomEventObject.EnemyView);
                Debug.Log("show enemy panel");
            }
        }

        public void ShowPanel(InteractiveObjectHandler interactiveObjectHandler)
        {
            throw new NotImplementedException();
        }
    }
}