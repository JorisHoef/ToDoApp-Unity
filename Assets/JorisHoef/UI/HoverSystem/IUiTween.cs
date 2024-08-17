using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.UI.HoverSystem
{
    public class Tweener
    {
        public void TweenAll(List<IUiTween> uiTweens, AnimationCurve easeType)
        {
            foreach (var uiTween in uiTweens)
            {
                uiTween.DoTween(easeType);
            }
        }
    }
    
    public interface IUiTween
    {
        public void DoTween(AnimationCurve ease);
    };
    
    [Serializable]
    public class ColorTween : IUiTween
    {
        private Graphic _graphic;
        private Color _targetColor;
        private float _duration;
        
        public ColorTween(Graphic graphic, Color targetColor, float duration)
        {
            this._graphic = graphic;
            this._targetColor = targetColor;
            this._duration = duration;
        }
        
        public void DoTween(AnimationCurve ease)
        {
            this._graphic.DOColor(this._targetColor, this._duration).SetEase(ease);
        }
    }

    // public class ScaleTween : IUiTween
    // {
    //     private Vector3 _targetScale;
    //     private float _duration;
    //     
    //     public ScaleTween(Vector3 targetScale, float duration)
    //     {
    //         this._targetScale = targetScale;
    //         this._duration = duration;
    //     }
    //     
    //     public void DoTween(Graphic graphic)
    //     {
    //         graphic.transform.DOScale(this._targetScale, this._duration);
    //     }
    // }
    //
    // public class MoveTween : IUiTween
    // {
    //     private Vector3 _targetPosition;
    //     private float _duration;
    //     
    //     public MoveTween(Vector3 targetPosition, float duration)
    //     {
    //         this._targetPosition = targetPosition;
    //         this._duration = duration;
    //     }
    //     
    //     public void DoTween(Graphic graphic)
    //     {
    //         graphic.rectTransform.DOAnchorPos(this._targetPosition, this._duration);
    //     }
    // }
}