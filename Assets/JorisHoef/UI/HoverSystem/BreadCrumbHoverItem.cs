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
        
        [SerializeField] private Graphic[] _graphics;
        [SerializeField] private Graphic[] _invertedGraphics;
        [SerializeField] private Graphic[] _inBetweeners;
        
        [Header("Colours")]
        [SerializeField] private Color _selectedMaterial;
        [SerializeField] private Color _defaultMaterial;
        [SerializeField] private Color _hoverMaterial;

        [Header("Tween Settings")]
        [SerializeField] private float _tweenDuration = 0.5f;
        [SerializeField] private AnimationCurve _ease;
        
        private readonly Tweener _tweener = new Tweener();
        private bool _isSelected;
        private bool _isHovered;
        
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
        
        private List<IUiTween> SetUITweens(Color targetColor)
        {
            var uiTweens = new List<IUiTween>();
            foreach (var graphic in this._graphics)
            {
                var colorTween = new ColorTween(graphic, targetColor, this._tweenDuration);
                uiTweens.Add(colorTween);
            }
            foreach (var invertedGraphic in this._invertedGraphics)
            {
                var colorTween = new ColorTween(invertedGraphic, this.GetContrastingColor(targetColor), this._tweenDuration);
                uiTweens.Add(colorTween);
            }
            
            for (int i = 1; i <= this._inBetweeners.Length; i++)
            {
                //Length == 100%
                //targetColor argument == 100%
                //Each amount == % increment
                int j = this._inBetweeners.Length - i;
                Graphic inBetweener = this._inBetweeners[j];
                float interpolationFactor = (float)(i - 1) / (this._inBetweeners.Length);
                
                Color newTargetColor = Color.Lerp(this._defaultMaterial, targetColor, interpolationFactor);

                var colorTween = new ColorTween(inBetweener, newTargetColor, this._tweenDuration);
                uiTweens.Add(colorTween);
            }
            return uiTweens;
        }
        
        private void SetSelected()
        {
            List<IUiTween> uiTweens = this.SetUITweens(this._selectedMaterial);
            this._tweener.TweenAll(uiTweens, this._ease);
        }

        private void Deselect()
        {
            List<IUiTween> uiTweens = this.SetUITweens(this._defaultMaterial);
            this._tweener.TweenAll(uiTweens, this._ease);
        }

        private void SetHover()
        {
            List<IUiTween> uiTweens = this.SetUITweens(this._hoverMaterial);
            this._tweener.TweenAll(uiTweens, this._ease);
        }
        
        private Color GetContrastingColor(Color backgroundColor)
        {
            // Calculate the relative luminance of the color
            float luminance = ((0.299f * backgroundColor.r) + (0.587f * backgroundColor.g) + (0.114f * backgroundColor.b));

            // Return black or white based on luminance
            return luminance > 0.5f ? Color.black : Color.white;
        }
    }
}