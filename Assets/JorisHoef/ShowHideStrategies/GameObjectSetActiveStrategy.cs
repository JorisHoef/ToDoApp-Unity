using UnityEngine;

namespace JorisHoef.ShowHideStrategies
{
    public class GameObjectSetActiveStrategy : IShowHideStrategy
    {
        private readonly GameObject _gameObject;
        
        public bool IsShown => _gameObject.activeSelf;
        
        public GameObjectSetActiveStrategy(GameObject gameObject)
        {
            this._gameObject = gameObject;
        }

        public void Show()
        {
            _gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            _gameObject.SetActive(false);
        }
    }
}