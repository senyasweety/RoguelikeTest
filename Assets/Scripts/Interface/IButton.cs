using System;

namespace Assets.Interface
{
    public interface IButton : IPayloadState<string>
    {
        event Action OnClicked;
    }
}