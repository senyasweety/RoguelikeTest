using System;
using Assets.Fight.Element;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjectSystem
{
    [CreateAssetMenu(fileName = "ElementSprite", menuName = "Elements", order = 0)]
    public class ElementsSpriteView : ScriptableObject
    {
        [SerializeField] private Sprite _fire;
        [SerializeField] private Sprite _tree;
        [SerializeField] private Sprite _water;
        [SerializeField] private Sprite _metal;
        [SerializeField] private Sprite _stone;

        public Sprite GetElementSprite(Element type)
        {
            switch (type)
            {
                case Element.Fire:
                    return _fire;
                case Element.Tree:
                    return _tree;
                case Element.Water:
                    return _water;
                case Element.Metal:
                    return _metal;
                case Element.Stone:
                    return _stone;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}