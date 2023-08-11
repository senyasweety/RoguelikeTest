using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GenerationSystem
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2 _roomSizeWorldUnits = new Vector2(30, 30);
        [SerializeField] [Range(0, 1f)] private float _chanceWalkerChangeDirection = 0.5f;
        [SerializeField] [Range(0, 1f)] private float _chanceWalkerSpawn = 0.05f;
        [SerializeField] [Range(0, 1f)] private float _chanceWalkerDestoy = 0.05f;
        [SerializeField] [Range(0, 1f)] private float _percentToFill = 0.2f;
        [SerializeField] private int _maxWalkers = 10;
        [SerializeField] private int _maxCountIterations = 100_000;
   
        private const float WorldUnitsInOneGridCell = 1;

        private GridSpace[,] _grid;
        private int _roomHeight;
        private int _roomWidth;
        private List<Walker> _walkers = new List<Walker>();

        private int _startXPosition;
        private int _startYPosition;
        
        private int _lastXPosition;
        private int _lastYPosition;

        public GridSpace[,] GenerateLevel()
        {
            Setup();
            CreateFloors();
            CreateWalls();
            RemoveSingleWalls();
            CreateInformationSpace();

            return _grid;
        }

        #region Setup

        private void Setup()
        {
            FindGridSize();
            CreateDefaultGrid();
            CreateFirstWalker();
        }

        private void FindGridSize()
        {
            _roomHeight = Mathf.RoundToInt(_roomSizeWorldUnits.x / WorldUnitsInOneGridCell);
            _roomWidth = Mathf.RoundToInt(_roomSizeWorldUnits.y / WorldUnitsInOneGridCell);
        }

        private void CreateDefaultGrid()
        {
            _grid = new GridSpace[_roomWidth, _roomHeight];

            for (int x = 0; x < _roomWidth - 1; x++)
            {
                for (int y = 0; y < _roomHeight - 1; y++)
                {
                    _grid[x, y] = GridSpace.Empty;
                }
            }
        }

        private void CreateFirstWalker()
        {
            Walker newWalker = new Walker();
            Vector2 spawnPosition = new Vector2(Mathf.RoundToInt(_roomWidth / 2.0f), Mathf.RoundToInt(_roomHeight / 2.0f));
            _startXPosition = (int) spawnPosition.x;
            _startYPosition = (int) spawnPosition.y;

            newWalker.direction = GetRandomDirection();
            newWalker.position = spawnPosition;

            _walkers.Add(newWalker);
        }

        private Vector2 GetRandomDirection()
        {
            int choice = Random.Range(0, 4);

            switch (choice)
            {
                case 0:
                    return Vector2.down;
                case 1:
                    return Vector2.left;
                case 2:
                    return Vector2.up;
                default:
                    return Vector2.right;
            }
        }

        #endregion

        #region CreateFloors

        void CreateFloors()
        {
            int iterations = 0;

            while (iterations < _maxCountIterations)
            {
                CreateFloorOnWalkerPosition();
                TryToDestroyWalker();
                TryPickNewDirectionForWalker();
                TrySpawnNewWalker();
                MoveWalkers();

                bool checkToExitLoop = (float) CountOfFloors() / _grid.Length > _percentToFill;

                if (checkToExitLoop)
                {
                    break;
                }

                iterations++;
            }
        }

        private void CreateFloorOnWalkerPosition()
        {
            foreach (Walker walker in _walkers)
            {
                _lastXPosition = (int) walker.position.x;
                _lastYPosition = (int) walker.position.y;

                _grid[_lastXPosition, _lastYPosition] = GridSpace.Floor;
            }
        }

        private void TryToDestroyWalker()
        {
            for (int i = 0; i < _walkers.Count; i++)
            {
                if (Random.value < _chanceWalkerDestoy && _walkers.Count > 1)
                {
                    _walkers.RemoveAt(i);
                    break;
                }
            }
        }

        private void TryPickNewDirectionForWalker()
        {
            for (int i = 0; i < _walkers.Count; i++)
            {
                if (Random.value < _chanceWalkerChangeDirection)
                {
                    Walker walker = _walkers[i];
                    walker.direction = GetRandomDirection();
                    _walkers[i] = walker;
                }
            }
        }

        private void TrySpawnNewWalker()
        {
            for (int i = 0; i < _walkers.Count; i++)
            {
                if (Random.value < _chanceWalkerSpawn && _walkers.Count < _maxWalkers)
                {
                    Walker newWalker = new Walker {direction = GetRandomDirection(), position = _walkers[i].position};
                    _walkers.Add(newWalker);
                }
            }
        }

        private void MoveWalkers()
        {
            for (int i = 0; i < _walkers.Count; i++)
            {
                Walker walker = _walkers[i];
                walker.position += walker.direction;
                CheckBoarderOfGrid(ref walker);
                _walkers[i] = walker;
            }
        }

        private void CheckBoarderOfGrid(ref Walker walker)
        {
            int offset = 2;
            
            walker.position.x = Mathf.Clamp(walker.position.x, 1, _roomWidth - offset);
            walker.position.y = Mathf.Clamp(walker.position.y, 1, _roomHeight - offset);
        }

        private int CountOfFloors()
        {
            int count = 0;

            foreach (GridSpace space in _grid)
            {
                if (space == GridSpace.Floor)
                {
                    count++;
                }
            }

            return count;
        }

        #endregion

        #region CreateWalls

        private void CreateWalls()
        {
            for (int x = 0; x < _roomWidth - 1; x++)
            {
                for (int y = 0; y < _roomHeight - 1; y++)
                {
                    if (_grid[x, y] == GridSpace.Floor)
                    {
                        TryPlaceWalls(x, y);
                    }
                }
            }
        }

        private void TryPlaceWalls(int x, int y)
        {
            if (_grid[x, y + 1] == GridSpace.Empty)
                _grid[x, y + 1] = GridSpace.Wall;

            if (_grid[x, y - 1] == GridSpace.Empty)
                _grid[x, y - 1] = GridSpace.Wall;

            if (_grid[x + 1, y] == GridSpace.Empty)
                _grid[x + 1, y] = GridSpace.Wall;

            if (_grid[x - 1, y] == GridSpace.Empty)
                _grid[x - 1, y] = GridSpace.Wall;
        }

        #endregion

        #region RemoveSingleWalls

        private void RemoveSingleWalls()
        {
            for (int x = 0; x < _roomWidth - 1; x++)
            {
                for (int y = 0; y < _roomHeight - 1; y++)
                {
                    if (_grid[x, y] == GridSpace.Wall)
                    {
                        CheckEachSide(x, y);
                    }
                }
            }
        }

        private void CheckEachSide(int cellX, int cellY)
        {
            bool isAroundAllFloor = true;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    bool isWidthRangeOut = CheckOutOfRange(cellX, x, _roomWidth);
                    bool isHeightRangeOut = CheckOutOfRange(cellY, y, _roomHeight);
                    bool isCorners = x != 0 && y != 0;
                    bool isCenter = x == 0 && y == 0;

                    if (isWidthRangeOut || isHeightRangeOut || isCorners || isCenter)
                    {
                        continue;
                    }

                    if (_grid[cellX + x, cellY + y] != GridSpace.Floor)
                    {
                        isAroundAllFloor = false;
                        break;
                    }
                }

                if (isAroundAllFloor == false)
                {
                    break;
                }
            }

            if (isAroundAllFloor)
            {
                _grid[cellX, cellY] = GridSpace.Floor;
            }
        }

        private bool CheckOutOfRange(int center, int offset, int roomSize) =>
            center + offset < 0 || center + offset > roomSize - 1;

        #endregion

        #region CreateInformationSpace

        private void CreateInformationSpace()
        {
            CreateStartSpace();
            CreateLastSpace();
        }

        private void CreateStartSpace()
        {
            _grid[_startXPosition, _startYPosition] = GridSpace.First;
        }

        private void CreateLastSpace()
        {
            if (_startXPosition != _lastXPosition && _startYPosition != _lastYPosition)
                _grid[_lastXPosition, _lastYPosition] = GridSpace.Last;
            else
                _grid[_lastXPosition + 1, _lastYPosition - 1] = GridSpace.Last;
        }

        #endregion
    }
}