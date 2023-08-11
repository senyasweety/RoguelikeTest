using Assets.Scripts.SoundSystem;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameRoot : MonoBehaviour
    {
        public static GameRoot Instance { get; private set; }

        public ISound Sound => _sound;
        
        private Sound _sound;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                Initialization();
                return;
            }
            
            Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            _sound.Dispose();
        }
        
        private void Initialization()
        {
            _sound = new Sound();
        }
    }
}