using Assets.Interface;

namespace Assets.Infrastructure.States
{
    public class LoadLevelState : IPayloadState<string>
    {
        public void Enter(string sceneName)
        {
            
        }

        public void Exit()
        {
        }
    }
}