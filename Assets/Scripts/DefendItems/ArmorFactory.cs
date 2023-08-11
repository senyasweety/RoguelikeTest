using UnityEngine;

namespace Assets.DefendItems
{
    public class ArmorFactory
    {
        public Armor Create(Body body, Head head, ParticleSystem particleSystem) =>
            new Armor(body, head, particleSystem);
    }
}