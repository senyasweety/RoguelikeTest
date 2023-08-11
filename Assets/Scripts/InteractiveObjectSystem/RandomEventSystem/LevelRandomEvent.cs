using UnityEngine;

namespace Assets.Scripts.InteractiveObjectSystem.RandomEventSystem
{
    public class LevelRandomEvent
    {
        private readonly float _chanceForEnemy = 0.4f; 
        private readonly float _chanceForLoot = 0.59f; 
        private readonly float _chanceForAD = 0.01f; 
        
        public RandomEventType GetRandomEvent()
        {
            float randomNumber = Random.value;
            
            if (randomNumber < _chanceForAD)
            {
                return RandomEventType.AD;
            }
            else if (randomNumber < _chanceForAD + _chanceForEnemy)
            {
                return RandomEventType.Enemy;
            }
            else
            {
                return RandomEventType.Loot;
            }
        }
    }
}