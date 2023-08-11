using System;
// TODO: загрузить фикс баг для агавы
//using Agava.WebUtility;

namespace Assets.Scripts.SoundSystem
{
    public class Sound: ISound, IDisposable
    {
        public bool IsMusicOn { get => _isMusicOn; }
        public bool IsSfxOn { get => _isSfxOn; }
        public bool IsHidden { get => _isHiddenOn; }
        
        public event Action<bool> OnMusicStateChanged; 
        public event Action<bool> OnSfxStateChanged; 
        public event Action<bool> OnHiddenStateChanged;

        private bool _isMusicOn;
        private bool _isSfxOn;
        private bool _isHiddenOn;

        public Sound()
        {
            _isMusicOn = true;
            _isSfxOn = false;
            _isHiddenOn = true;

            // TODO: заменить загрузку состояний звуков из PlayerData
            // IsMusicOn = PlayerData.Instance.IsMusicOn;
            // IsSfxOn = PlayerData.Instance.IsSFXOn;
            // WebApplication.InBackgroundChangeEvent += ChangeBackgroundSounds;
        }

        public void Pause()
        {
            OnMusicStateChanged?.Invoke(false);
            OnSfxStateChanged?.Invoke(false);
        }

        public void UpPause()
        {
            OnMusicStateChanged?.Invoke(true);
            OnSfxStateChanged?.Invoke(true);
        }

        public void Dispose()
        {
            // WebApplication.InBackgroundChangeEvent -= ChangeBackgroundSounds;
        }
        
        private void ChangeBackgroundSounds(bool hidden)
        {
            _isHiddenOn = hidden;
            OnHiddenStateChanged?.Invoke(_isHiddenOn);
        }
    }
}
