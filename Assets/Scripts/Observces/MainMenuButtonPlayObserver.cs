using System.Collections.Generic;
using Assets.Interface;

namespace Assets.Observces
{
    public class MainMenuButtonPlayObserver
    {
        private MainMenuButtonPlayObserver() { }

        private static MainMenuButtonPlayObserver _instance;

        public static MainMenuButtonPlayObserver Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainMenuButtonPlayObserver();

                return _instance;
            }
        }

        private List<IButtonObserver> _observers = new List<IButtonObserver>();

        public void Registry(IButtonObserver buttonObserver) =>
            _observers.Add(buttonObserver);

        public void UnRegistry(IButtonObserver buttonObserver) => 
            _observers.Remove(buttonObserver);

        public void Notify()
        {
            foreach (IButtonObserver observer in _observers) 
                observer.Update();
        }
    }
}