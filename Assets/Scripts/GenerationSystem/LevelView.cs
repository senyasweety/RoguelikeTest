using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.GenerationSystem
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemapFloor;
        [SerializeField] private Tilemap _tilemapWall;
        [SerializeField] private RuleTile _ruleTileFloor;
        [SerializeField] private RuleTile _ruleTileWall;
        [SerializeField] private Tile _firstPoint;
        [SerializeField] private Tile _lastPoint;
        [SerializeField] private NavMeshSurface[] _surfaces;
        
        private const float WorldUnitsInOneGridCell = 1;
        
        private Vector2 _roomSizeWorldUnits;
        private GridSpace[,] _grid;
        private int _roomHeight;
        private int _roomWidth;
        
        public void Init(GridSpace[,] grid)
        {
            _grid = grid;
            _roomSizeWorldUnits = new Vector2(grid.GetLength(0), grid.GetLength(1));
            FindGridSize();
            SpawnLevel();
            
            foreach (var surface in _surfaces)
            {
                surface.BuildNavMesh();
            }
        }
        
        private void FindGridSize()
        {
            _roomHeight = Mathf.RoundToInt(_roomSizeWorldUnits.x / WorldUnitsInOneGridCell);
            _roomWidth = Mathf.RoundToInt(_roomSizeWorldUnits.y / WorldUnitsInOneGridCell);
        }
        
        private void SpawnLevel()
        {
            for (int x = 0; x < _roomWidth; x++)
            {
                for (int y = 0; y < _roomHeight; y++)
                {
                    switch (_grid[x, y])
                    {
                        case GridSpace.Empty:
                            Spawn(x, y, _tilemapWall, _ruleTileWall);
                            break;
                        case GridSpace.Floor:
                            Spawn(x, y, _tilemapFloor, _ruleTileFloor);
                            break;
                        case GridSpace.Wall:
                            Spawn(x, y, _tilemapWall, _ruleTileWall);
                            break;
                        case GridSpace.First:
                            Spawn(x, y, _tilemapFloor, _firstPoint);
                            break;
                        case GridSpace.Last:
                            Spawn(x, y, _tilemapFloor, _lastPoint);
                            break;
                    }
                }
            }
        }

        private void Spawn(float x, float y, Tilemap tilemap, RuleTile template)
        {
            tilemap.SetTile(CalculatePosition(x, y), template);
        }
        
        private void Spawn(float x, float y, Tilemap tilemap, Tile template)
        {
            tilemap.SetTile(CalculatePosition(x, y), template);
        }

        private Vector3Int CalculatePosition(float x, float y)
        {
            Vector2 offset = _roomSizeWorldUnits / 2.0f;
            Vector2 spawnPos = new Vector2(x, y) * WorldUnitsInOneGridCell - offset;
            Vector3Int position = new Vector3Int((int) spawnPos.x, (int) spawnPos.y, 0);
            return position;
        }
            
    }
}