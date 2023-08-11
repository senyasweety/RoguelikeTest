using System.Collections.Generic;
using UnityEngine;

namespace Assets.Fight
{
    public class EnemyPoint :  SpawnPoint
    {
        [SerializeField] private List<Point> _points;
        public List<Point> Points => _points;
    }
}