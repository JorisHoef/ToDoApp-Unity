using UnityEngine;

namespace JorisHoef.UI.HoverSystem
{
    public class ExclusiveHoverGroup : MonoBehaviour
    {
        [SerializeField] private HoverItem[] _buttonHover;

        private void Awake()
        {
            foreach (var buttonHover in this._buttonHover)
            { 
                buttonHover.OnSelected += this.ResetOtherButtonHovers;
            }
        }

        private void ResetOtherButtonHovers(HoverItem selectedHoverItem)
        {
            foreach (var buttonHover in this._buttonHover)
            {
                if (buttonHover != selectedHoverItem)
                {
                    buttonHover.SetSelection(false);
                }
            }
            
            selectedHoverItem.SetSelection(true);
        }
    }
}