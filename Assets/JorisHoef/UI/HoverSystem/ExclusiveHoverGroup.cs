using System;
using UnityEngine;

namespace JorisHoef.UI.HoverSystem
{
    public class ExclusiveHoverGroup : MonoBehaviour
    {
        private IHoverable[] _hoverables;

        private void Start()
        {
            this._hoverables = this.GetComponentsInChildren<IHoverable>();
            
            foreach (var hoverItem in this._hoverables)
            { 
                hoverItem.OnSelected += this.ResetOtherButtonHovers;
            }
        }
        
        private void OnDestroy()
        {
            foreach (var hoverItem in this._hoverables)
            { 
                hoverItem.OnSelected -= this.ResetOtherButtonHovers;
            }
        }

        private void ResetOtherButtonHovers(IHoverable selectedHoverItem)
        {
            foreach (var hoverItem in this._hoverables)
            {
                if (hoverItem != selectedHoverItem)
                {
                    hoverItem.SetSelection(false);
                }
            }
            
            selectedHoverItem.SetSelection(true);
        }
    }
}