using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.UI.HoverSystem
{
    /// <summary>
    /// Basic HoverItem component, will tween assigned graphics to and from the assigned colors
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    public class HoverItem : MonoBehaviour
    {
        private Graphic _graphic;
        private Graphic[] _childGraphics;
        private Color _targetColor;
        private float _tweenDuration;

        public bool IsInverted => this.GetComponent<TMP_Text>();

        private void Awake()
        {
            this._graphic = this.GetComponent<Graphic>();
        }

        private void Start()
        {
            this._childGraphics = this.GetComponentsInChildren<Graphic>().Where(c => c.gameObject != this.gameObject).ToArray(); //Exclude self
        }

        public void SetColor(Color color, float tweenDuration)
        {
            this._targetColor = color;
            this._tweenDuration = tweenDuration;
        }
        
        public List<IUiTween> SetAndGetTweens()
        {
            Color newTarget;
            if (this.IsInverted)
            {
                newTarget = GetContrastingColor(this._targetColor);
            }
            else
            {
                newTarget = this._targetColor;
            }
            
            var colorTween = new ColorTween(this._graphic, newTarget, this._tweenDuration);
            List<IUiTween> uiTweens = new List<IUiTween>
            {
                    colorTween
            };
            
            foreach (Graphic childGraphic in this._childGraphics)
            {
                if (childGraphic.GetComponent<HoverItem>() != null)//Do not change children who decide their own colors
                {
                    continue;
                }
                
                if (childGraphic is Image)//Do not invert images
                {
                    colorTween = new ColorTween(childGraphic, newTarget, this._tweenDuration);
                }
                else
                {
                    colorTween = new ColorTween(childGraphic, GetContrastingColor(newTarget), this._tweenDuration);
                }
                uiTweens.Add(colorTween);
            }
            return uiTweens;
        }
        
        private static Color GetContrastingColor(Color backgroundColor)
        {
            // Calculate the relative luminance of the color
            float luminance = ((0.299f * backgroundColor.r) + (0.587f * backgroundColor.g) + (0.114f * backgroundColor.b));

            // Return black or white based on luminance
            return luminance > 0.5f ? Color.black : Color.white;
        }
    }
}