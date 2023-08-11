using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GenerationSystem
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private int _maxCountOfSpawnItems = 20;
        [SerializeField] private List<SpawnerItem> _spawnerItems;
        [SerializeField] private GameObject _bossTemplate;
        [SerializeField] private Transform _conteiner;

        private GridSpace[,] _grid;
        private List<Vector2> _spawnPosition = new List<Vector2>();
        private Vector2 _bossPosition;
        private List<GameObject> _items;
        
        public void Init(GridSpace[,] grid)
        {
            _grid = grid;

            CreateSpawnPosition(_grid);
            ShuffleItems(_spawnPosition);
            SpawnEnemys();
            SpawnBoss(_bossPosition);
        }

        private void CreateSpawnPosition(GridSpace[,] grid)
        {
            Vector2 offset = new Vector2(_grid.GetLength(0), _grid.GetLength(1)) / 2.0f;
            float gridOffset = 0.5f;
            
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == GridSpace.Floor)
                    {
                        _spawnPosition.Add(new Vector2(i+gridOffset, j+gridOffset) - offset);
                    }
                    else if (grid[i, j] == GridSpace.Last)
                    {
                        _bossPosition = new Vector2(i + gridOffset, j + gridOffset) - offset;
                    }
                }
            }
        }
        
        private void ShuffleItems(List<Vector2> array)
        {
            int upperIndex = array.Count - 1;
            int lowerIndex = 0;
            int coefficientOfMixing = 5;
            int numberOfIterations = coefficientOfMixing * array.Count;

            int oldIndex;
            int newIndex;

            for (int i = 0; i < numberOfIterations; i++)
            {
                oldIndex = Random.Range(lowerIndex, upperIndex + 1);
                newIndex = Random.Range(lowerIndex, upperIndex + 1);

                (array[oldIndex], array[newIndex]) = (array[newIndex], array[oldIndex]);
            }
        }

        private void SpawnEnemys()
        {
            for (int i = 0; i < _maxCountOfSpawnItems; i++)
            {
                foreach (SpawnerItem item in _spawnerItems)
                {
                    if (item.IsMax == false && _spawnPosition.Count > 0)
                    {
                        float chance = Random.value;

                        if (chance <= item.ChanceSpawn)
                        {
                            var newPosition = _spawnPosition[0];
                            _spawnPosition.RemoveAt(0);
                            Instantiate(item.Template, newPosition, Quaternion.identity, _conteiner);
                            item.AddSpawnCount();
                        }
                    }
                    
                    if (_spawnPosition.Count == 0)
                    {
                        return;
                    }
                }
            }
        }

        private void SpawnBoss(Vector2 position)
        {
            Instantiate(_bossTemplate, position, Quaternion.identity, _conteiner);
        }
    }

    [System.Serializable]
    public class SpawnerItem
    {
        [SerializeField] private GameObject _template;
        [SerializeField] private int _maxCount;
        [SerializeField] [Range(0, 1f)] private float _chanceSpawn = 0.05f;

        public GameObject Template => _template;
        public float ChanceSpawn => _chanceSpawn;
        public bool IsMax => CountOfSpawn == _maxCount;

        public int CountOfSpawn { get; private set; }

        public void AddSpawnCount()
        {
            CountOfSpawn++;
        }
    }
}