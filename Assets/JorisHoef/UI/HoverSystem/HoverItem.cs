using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private bool _isInverted;
        
        private Graphic _graphic;
        private Graphic[] _childGraphics;
        private Color _targetColor;
        private float _tweenDuration;
        
        private void Awake()
        {
            this._graphic = this.GetComponent<Graphic>();
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
            if (this._isInverted)
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
            
            foreach (var invertedGraphic in this._childGraphics)
            {
                colorTween = new ColorTween(invertedGraphic, GetContrastingColor(newTarget), this._tweenDuration);
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