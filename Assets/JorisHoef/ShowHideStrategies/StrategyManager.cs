using UnityEngine;

namespace JorisHoef.ShowHideStrategies
{
    public static class StrategyManager
    {
        private static IShowHideStrategy _showHideStrategy;
        
        public static IShowHideStrategy GetShowHideStrategy(GameObject gameObject)
        {
            return new GameObjectSetActiveStrategy(gameObject);
        }
    }
}