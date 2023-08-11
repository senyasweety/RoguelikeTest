using Assets.Fight.Element;
using Assets.Scripts.InteractiveObjectSystem;
using UnityEngine;

namespace Assets.Loot
{
    public class RandomLootGenerator
    {
        public InteractiveObjectData GetRandomLoot()
        {
            string name = "Какая-то бум палка";
            string data = $"Сила: {Random.Range(1, 10)}\n" +
                          $"Урон: {Random.Range(1, 10)}\n" +
                          $"Дамаг: {Random.Range(1, 10)}\n";
            Element element = (Element) Random.Range(0, 5);
            
            return new InteractiveObjectData(name, data, element);
        }
    }
}