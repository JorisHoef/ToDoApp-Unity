﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.UI.HoverSystem
{
    /// <summary>
    /// Will do equal behaviour as the HoverItem class, can assign a target tween destination (as HoverItem)
    /// Can also assign objects in between which will try to percentage wise move towards the tween destination per item (as a breadcrumb or staircase)
    /// </summary>
    public class BreadCrumbHoverItem : MonoBehaviour, IHoverable
    {
        public event Action<IHoverable> OnSelected;
        
        [SerializeField] private bool _staircaseMode;
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
                
                addedComponent.OnHoverEnter += OnHoverEnterCalled;
                addedComponent.OnHoverExit += OnHoverExitCalled;
                addedComponent.OnSelected += OnSelectedCalled;
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
                 addedComponent.OnHoverEnter += OnHoverEnterCalled;
                 addedComponent.OnHoverExit += OnHoverExitCalled;
                 addedComponent.OnSelected += OnSelectedCalled;
                 if (childItem == endItem) 
                 { 
                     break; 
                 } 
             }
        }
#endregion
        
        private List<List<IUiTween>> SetUITweens(Color targetColor, HoverItem hoveredItem)
        {
            var uiTweens = new List<List<IUiTween>>();
            
            //Handle main and inverted graphics
            uiTweens.AddRange(SetColorForGraphics(targetColor));
            
            //Handle chain items
            uiTweens.AddRange(SetColorForChainItems(targetColor, hoveredItem));
            
            return uiTweens;
        }
        
        private List<List<IUiTween>> SetColorForGraphics(Color targetColor)
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
            
            return uiTweens;
        }
        
        private List<List<IUiTween>> SetColorForChainItems(Color targetColor, HoverItem hoveredItem)
        {
            var uiTweens = new List<List<IUiTween>>();
            bool hoveredItemProcessed = false;
            
            for (int i = 1; i <= this._chainItems.Count; i++)
            {
                int j = this._chainItems.Count - i;
                HoverItem chainItem = this._chainItems[j];
                
                float interpolationFactor = CalculateInterpolationFactor(i);
                Color newTargetColor = this._invertBreadcrumb 
                                               ? Color.Lerp(targetColor, this._defaultMaterial, interpolationFactor) 
                                               : Color.Lerp(this._defaultMaterial, targetColor, interpolationFactor);

                chainItem.SetColor(newTargetColor, this._tweenDuration);
                uiTweens.Add(chainItem.SetAndGetTweens());
                
                
                if (_staircaseMode && chainItem.IsHovered && chainItem == hoveredItem)
                {
                    Debug.Log("Item already hovered, but setting color before breaking.");
                    hoveredItemProcessed = true;
                    continue; //Continue to process the last color change before breaking
                }
                
                //If the hovered item was processed, break after setting its color
                if (hoveredItemProcessed)
                {
                    break;
                }
            }
            
            return uiTweens;
        }
        
        private float CalculateInterpolationFactor(int index)
        {
            return this._invertBreadcrumb 
                           ? (float)(index - 1) / (this._chainItems.Count + 1) 
                           : (float)index / this._chainItems.Count;
        }
        
#region HoveringAndSelection
        private void OnSelectedCalled(HoverItem selectedHoveritem)
        { 
            //OnSelected?.Invoke(this);
            _isSelected = !_isSelected;
            SetSelected(selectedHoveritem);
        }
        
        private void OnHoverEnterCalled(HoverItem onHoverItem)
        {
            if (this._isSelected) return;
            SetHover(onHoverItem);
        }
        
        private void OnHoverExitCalled(HoverItem onHoverItem)
        { 
            if (this._isSelected) return;
            Deselect(onHoverItem);
        }
        
        public void SetSelection(bool isSelected)
        {
           // this._isSelected = isSelected;
        }
        
        private void SetSelected(HoverItem hoverItem)
        {
            if (this._isSelected)
            {
                List<List<IUiTween>> selected = SetUITweens(this._selectedMaterial, hoverItem);
                foreach (var uiTweens in selected)
                {
                    this._tweener.TweenAll(uiTweens, this._ease);   
                }
            }
            else if (!this._isSelected && !hoverItem.IsHovered)
            {
                Deselect(hoverItem);
            }
            else
            {
                SetHover(hoverItem);
            }
        }
        
        private void Deselect(HoverItem hoverItem)
        {
            List<List<IUiTween>> deselected = SetUITweens(this._defaultMaterial, hoverItem);
            foreach (var uiTweens in deselected)
            {
                this._tweener.TweenAll(uiTweens, this._ease);   
            }
        }
        
        private void SetHover(HoverItem hoverItem)
        {
            List<List<IUiTween>> hovered = SetUITweens(this._hoverMaterial, hoverItem);
            foreach (var uiTweens in hovered)
            {
                this._tweener.TweenAll(uiTweens, this._ease);   
            }
        }
#endregion
    }
}