using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JorisHoef.UI.HoverSystem
{
    public class HoverItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action<HoverItem> OnSelected;
        
        [Header("Assign button if this component isn't on the button itself")]
        
        [SerializeField] private Graphic[] _images;
        [SerializeField] private Graphic[] _invertedImages;

        [Header("Colours")]
        [SerializeField] private Color _selectedMaterial;
        [SerializeField] private Color _defaultMaterial;
        [SerializeField] private Color _hoverMaterial;

        [Header("Tween Settings")]
        [SerializeField] private float _tweenDuration = 0.5f;
        
        private bool _isSelected;

        private List<Graphic> AllGraphics => this._images.Concat(this._invertedImages).ToList();
        
        public void SetSelection(bool isSelected)
        {
            this._isSelected = isSelected;
            this.UpdateState();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.SetHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
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
            else
            {
                this.Deselect();
            }
        }

        private Tweener _tweener = new Tweener();
        
        private void SetSelected()
        {
            var targetColor = ColorManager.Instance.GetColor(this._selectedMaterial);
            var uiTweens = new List<IUiTween>();
            foreach (var graphic in this._images)
            {
                var colorTween = new ColorTween(graphic, targetColor, this._tweenDuration);
                uiTweens.Add(colorTween);
            }
            foreach (var invertedGraphic in this._invertedImages)
            {
                var colorTween = new ColorTween(invertedGraphic, this.GetContrastingColor(targetColor), this._tweenDuration);
                uiTweens.Add(colorTween);
            }
            
            this._tweener.AllTweenis(uiTweens);
        }

        private void Deselect()
        {
            var targetColor = ColorManager.Instance.GetColor(this._defaultMaterial);
            var uiTweens = new List<IUiTween>();
            foreach (var graphic in this._images)
            {
                var colorTween = new ColorTween(graphic, targetColor, this._tweenDuration);
                uiTweens.Add(colorTween);
            }
            foreach (var invertedGraphic in this._invertedImages)
            {
                var colorTween = new ColorTween(invertedGraphic, this.GetContrastingColor(targetColor), this._tweenDuration);
                uiTweens.Add(colorTween);
            }
            
            this._tweener.AllTweenis(uiTweens);
        }

        private void SetHover()
        {
            var targetColor = ColorManager.Instance.GetColor(this._hoverMaterial);
            var uiTweens = new List<IUiTween>();
            foreach (var graphic in this._images)
            {
                var colorTween = new ColorTween(graphic, targetColor, this._tweenDuration);
                uiTweens.Add(colorTween);
            }
            foreach (var invertedGraphic in this._invertedImages)
            {
                var colorTween = new ColorTween(invertedGraphic, this.GetContrastingColor(targetColor), this._tweenDuration);
                uiTweens.Add(colorTween);
            }
            
            this._tweener.AllTweenis(uiTweens);
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