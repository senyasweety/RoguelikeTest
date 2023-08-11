using System.Collections.Generic;
using UnityEngine;

namespace Assets.Fight.Dice
{
    public class DiceModel
    {
        public readonly IReadOnlyCollection<Sprite> Sprites;

        public DiceModel(IReadOnlyList<Sprite> sprites) =>
            Sprites = sprites;
    }
}