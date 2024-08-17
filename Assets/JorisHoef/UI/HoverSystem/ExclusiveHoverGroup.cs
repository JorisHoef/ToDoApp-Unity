using System;
using UnityEngine;

namespace JorisHoef.UI.HoverSystem
{
    public class ExclusiveHoverGroup : MonoBehaviour
    {
        [SerializeField] private HoverItem[] _hoverItems;

        private void Start()
        {
            this._hoverItems = this.GetComponentsInChildren<HoverItem>();
            
            foreach (var buttonHover in this._hoverItems)
            { 
                buttonHover.OnSelected += this.ResetOtherButtonHovers;
            }
        }

        private void OnDestroy()
        {
            foreach (var buttonHover in this._hoverItems)
            { 
                buttonHover.OnSelected -= this.ResetOtherButtonHovers;
            }
        }

        private void ResetOtherButtonHovers(HoverItem selectedHoverItem)
        {
            foreach (var buttonHover in this._hoverItems)
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