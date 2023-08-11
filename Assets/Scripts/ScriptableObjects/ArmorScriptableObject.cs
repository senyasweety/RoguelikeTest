using System;
using Assets.Fight.Element;
using UnityEngine;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Armor", menuName = "ScriptableObject/Armor", order = 0)]
    public class ArmorScriptableObject : ScriptableObject
    {
        [field: SerializeField] public Head HeadPart { get; private set; }
        [field: SerializeField] public Body BodyPart { get; private set; }

        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }


        [Serializable]
        public class Head
        {
            [field: SerializeField] public float Value { get; private set; }
        }
        
        [Serializable]
        public class Body
        {
            [field: SerializeField] public float Value { get; private set; }
            [field: SerializeField] public Element Element { get; private set; }
        
            public void SetNewElement(Element element) =>
                Element = element;
        }
    }
}