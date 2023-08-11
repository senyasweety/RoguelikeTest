using Assets.Scripts.InteractiveObjectSystem;

namespace Assets.Interface
{
    public interface IInteractiveService : IService
    {
        public string Data { get; }
        public string Name { get; }

        public ObjectType Type { get; }
    }
}