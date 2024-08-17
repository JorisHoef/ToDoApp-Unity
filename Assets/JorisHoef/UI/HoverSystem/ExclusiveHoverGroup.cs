using UnityEngine;

namespace JorisHoef.UI.HoverSystem
{
    /// <summary>
    /// The assigned HoverGroup determines shared OnSelected state
    /// </summary>
    /// <remarks>This means that when you assign a hovergroup to an object which has 3 child IHoverables, these 3 children now share 1 shared selected state</remarks>
    public class ExclusiveHoverGroup : MonoBehaviour
    {
        [Header("If not assigned will use this as Hover Group")]
        [SerializeField] private Transform _hoverGroup;
        
        private IHoverable[] _hoverables;

        private void Awake()
        {
            if (_hoverGroup == null)
            {
                _hoverGroup = this.transform;
            }
        }

        private void Start()
        {
            this._hoverables = this._hoverGroup.GetComponentsInChildren<IHoverable>();
            
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