using Assets.Fight.Element;

namespace Assets.Scripts.InteractiveObjectSystem
{
    public class InteractiveObjectData
    {
        public string Name { get; private set; }
        public string Data { get; private set; }
        public Element Element { get; private set; }
    
        public InteractiveObjectData(string name, string data, Element element)
        {
            Name = name;
            Data = data;
            Element = element;
        }
    }
}