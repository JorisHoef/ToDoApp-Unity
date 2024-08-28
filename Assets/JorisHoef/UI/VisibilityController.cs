using JorisHoef.ShowHideStrategies;
using UnityEngine;

namespace JorisHoef.UI
{
    public class VisibilityController : MonoBehaviour, IStrategySetter
    {
        private IShowHideStrategy _showHideStrategy;

        public void SetState()
        {
            _showHideStrategy ??= StrategyManager.GetShowHideStrategy(this.gameObject);

            if (!_showHideStrategy.IsShown)
            {
                _showHideStrategy.Show();
            }
            else
            {
                _showHideStrategy.Hide();
            }
        }
        
        public void SetStrategy(IShowHideStrategy strategy)
        {
            _showHideStrategy = strategy;
        }
    }
}