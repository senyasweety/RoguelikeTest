using System;

namespace Assets.Scripts.SoundSystem
{
    public interface ISound
    {
        bool IsMusicOn { get; }
        bool IsSfxOn { get; }
        bool IsHidden { get;}
        
        event Action<bool> OnMusicStateChanged; 
        event Action<bool> OnSfxStateChanged; 
        event Action<bool> OnHiddenStateChanged;
    }
}