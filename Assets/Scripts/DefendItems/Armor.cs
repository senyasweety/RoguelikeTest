using Assets.Person;
using UnityEngine;

namespace Assets.DefendItems
{
    public class Armor
    {
        public Armor(Body body, Head head, ParticleSystem particleSystem)
        {
            Body = body;
            Head = head;
            ParticleSystem = particleSystem;
        }

        public ParticleSystem ParticleSystem { get; private set; }
        public Head Head { get; private set; }
        public Body Body { get; private set; }
    }
}