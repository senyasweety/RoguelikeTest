using System.Collections.Generic;
using UnityEngine;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DiceSpriteScriptableObject", menuName = "ScriptableObject/Dice", order = 0)]
    public class DiceSpriteScriptableObject : ScriptableObject
    {
        [SerializeField] private List<Sprite> _sprites;

        public IReadOnlyList<Sprite> Sprites => _sprites;
    }
}