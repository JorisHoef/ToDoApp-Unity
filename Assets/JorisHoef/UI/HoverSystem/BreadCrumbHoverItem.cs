using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JorisHoef.UI.HoverSystem
{
    /// <summary>
    /// Will do equal behaviour as the HoverItem class, can assign a target tween destination (as HoverItem)
    /// Can also assign objects in between which will try to percentage wise move towards the tween destination per item (as a breadcrumb or staircase)
    /// </summary>
    public class BreadCrumbHoverItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IHoverable
    {
        public event Action<IHoverable> OnSelected;

        [SerializeField] private bool _invertBreadcrumb;
        
        [Header("Non-Chaining Graphics")]
        [SerializeField] private HoverItem[] _graphics;
        [SerializeField] private HoverItem[] _invertedGraphics;

        [Header("Chaining Graphics")]
        [SerializeField] private Graphic _startChainItem;
        [SerializeField] private Graphic _endChainItem;
        
        [Header("Colours")]
        [SerializeField] private Color _selectedMaterial;
        [SerializeField] private Color _defaultMaterial;
        [SerializeField] private Color _hoverMaterial;

        [Header("Tween Settings")]
        [SerializeField] private float _tweenDuration = 0.5f;
        [SerializeField] private AnimationCurve _ease;
        
        private readonly Tweener _tweener = new Tweener();
        private readonly List<HoverItem> _chainItems = new List<HoverItem>(); //Gets filled with everything between startChainItem and endChainItem
        private bool _isSelected;
        private bool _isHovered;

#region Initializing
        private void Awake()
        {
            if (this._startChainItem == null || this._endChainItem == null)
            {
                return;
            }
            this.SetItemChain(this._startChainItem, this._endChainItem);
        }
        
        private void SetItemChain(Component startItem, Component endItem)
        {
            for (int i = 0; i < startItem.transform.parent.childCount; i++)
            {
                Transform nextItem = startItem.transform.parent.GetChild(i);
                HoverItem addedComponent = nextItem.gameObject.AddComponent<HoverItem>();
                this._chainItems.Add(addedComponent);
                
                TraverseSiblingsAndChildren(nextItem, endItem);
                
                if (nextItem == endItem.transform.parent)
                {
                    break;
                }
            }
        }

        private void TraverseSiblingsAndChildren(Transform currentItem, Component endItem)
        {
             for (int j = 0; j < currentItem.childCount; j++) 
             { 
                 Transform childItem = currentItem.GetChild(j); 
                 HoverItem addedComponent = childItem.gameObject.AddComponent<HoverItem>();
                 
                 this._chainItems.Add(addedComponent);
                 
                 TraverseSiblingsAndChildren(childItem, endItem);
                 
                 if (childItem == endItem) 
                 { 
                     break; 
                 } 
             }
        }
#endregion

#region HoveringAndSelection
        public void SetSelection(bool isSelected)
        {
            this._isSelected = isSelected;
            this.UpdateState();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this._isHovered = true;
            if (!this._isSelected)
            {
                this.SetHover();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this._isHovered = false;
            if (this._isSelected)
            {
                this.SetSelected();
            }
            else
            {
                this.Deselect();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this._isSelected)
            {
                SetSelection(false);
                return;
            }
            
            if (OnSelected == null)
            {
                SetSelection(true);
            }
            else
            {
                this.OnSelected.Invoke(this);
            }
        }

        private void UpdateState()
        {
            if (this._isSelected)
            {
                this.SetSelected();
            }
            else if (this._isHovered)
            {
                this.SetHover();
            }
            else
            {
                this.Deselect();
            }
        }
        
        private List<List<IUiTween>> SetUITweens(Color targetColor)
        {
            var uiTweens = new List<List<IUiTween>>();
            foreach (var hoverItem in this._graphics)
            {
                hoverItem.SetColor(targetColor, this._tweenDuration);
                uiTweens.Add(hoverItem.SetAndGetTweens());
            }
            
            foreach (var invertedHoverItem in this._invertedGraphics)
            {
                invertedHoverItem.SetColor(targetColor, this._tweenDuration);
                uiTweens.Add(invertedHoverItem.SetAndGetTweens());
            }

            for (int i = 1; i <= this._chainItems.Count; i++)
            {
                int j = this._chainItems.Count - i;
                HoverItem chainItem = this._chainItems[j];
    
                float interpolationFactor;

                if (this._invertBreadcrumb)
                {
                    //Reverse order: start at visual 100% and end at visual 10%
                    interpolationFactor = (float)(i - 1) / (this._chainItems.Count + 1);
                    Color newTargetColor = Color.Lerp(targetColor, this._defaultMaterial, interpolationFactor);
                    chainItem.SetColor(newTargetColor, this._tweenDuration);
                }
                else
                {
                    //Normal order: start at 10% and end at 100%
                    interpolationFactor = (float)(i) / this._chainItems.Count;
                    Color newTargetColor = Color.Lerp(this._defaultMaterial, targetColor, interpolationFactor);
                    chainItem.SetColor(newTargetColor, this._tweenDuration);
                }

                uiTweens.Add(chainItem.SetAndGetTweens());
            }

            return uiTweens;
        }
        
        private void SetSelected()
        {
            List<List<IUiTween>> selected = SetUITweens(this._selectedMaterial);
            foreach (var uiTweens in selected)
            {
                this._tweener.TweenAll(uiTweens, this._ease);   
            }
        }

        private void Deselect()
        {
            List<List<IUiTween>> deselected = SetUITweens(this._defaultMaterial);
            foreach (var uiTweens in deselected)
            {
                this._tweener.TweenAll(uiTweens, this._ease);   
            }
        }

        private void SetHover()
        {
            List<List<IUiTween>> hovered = SetUITweens(this._hoverMaterial);
            foreach (var uiTweens in hovered)
            {
                this._tweener.TweenAll(uiTweens, this._ease);   
            }
        }
#endregion
    }
}